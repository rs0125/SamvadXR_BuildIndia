using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class MicController : MonoBehaviour
{
    public AudioSource playbackSource;
    public SimpleBackendConnector backendConnector;

    [Header("UI References")]
    public Button recordButton;
    public TextMeshProUGUI buttonLabel;
    public TextMeshProUGUI transcriptionText;

    [Header("XR Controller Input")]
    [Tooltip("Right controller B button – toggle recording / send")]
    public InputActionReference toggleRecordingAction;
    [Tooltip("Right controller A button – cancel recording")]
    public InputActionReference cancelRecordingAction;

    private bool isRecording = false;
    private AudioClip recordedClip;
    private string microphoneDevice;
    private int maxRecordingLength = 300; // 5 minutes

    void Start()
    {
        if (Microphone.devices.Length > 0)
        {
            microphoneDevice = Microphone.devices[0];
        }
        else
        {
            Debug.LogError("No microphone found!");
        }

        if (backendConnector == null)
        {
            Debug.LogError("SimpleBackendConnector is not assigned in the Inspector!");
        }

        // UI button kept for status display only – remove the click listener
        // (the button no longer drives recording)
    }

    void OnEnable()
    {
        // --- B button (toggle) ---
        if (toggleRecordingAction != null && toggleRecordingAction.action != null)
        {
            toggleRecordingAction.action.Enable();
            toggleRecordingAction.action.performed += OnToggleRecording;
        }

        // --- A button (cancel) ---
        if (cancelRecordingAction != null && cancelRecordingAction.action != null)
        {
            cancelRecordingAction.action.Enable();
            cancelRecordingAction.action.performed += OnCancelRecording;
        }
    }

    void OnDisable()
    {
        if (toggleRecordingAction != null && toggleRecordingAction.action != null)
        {
            toggleRecordingAction.action.performed -= OnToggleRecording;
        }

        if (cancelRecordingAction != null && cancelRecordingAction.action != null)
        {
            cancelRecordingAction.action.performed -= OnCancelRecording;
        }
    }

    // ── B button handler ──────────────────────────────────────────────
    private void OnToggleRecording(InputAction.CallbackContext ctx)
    {
        ToggleRecording();
    }

    // ── A button handler ──────────────────────────────────────────────
    private void OnCancelRecording(InputAction.CallbackContext ctx)
    {
        if (!isRecording) return; // nothing to cancel

        isRecording = false;
        Microphone.End(microphoneDevice);
        recordedClip = null;

        if (buttonLabel != null)
            buttonLabel.text = "Cancelled";

        Debug.Log("Recording cancelled by user.");

        // Reset label back to idle after a short delay
        Invoke(nameof(ResetLabelToIdle), 1.5f);
    }

    // ── Core toggle (start / stop-and-send) ───────────────────────────
    public void ToggleRecording()
    {
        if (!isRecording)
        {
            isRecording = true;

            if (buttonLabel != null)
                buttonLabel.text = "Recording…";

            Debug.Log("Recording started...");
            recordedClip = Microphone.Start(microphoneDevice, false, maxRecordingLength, 16000);
        }
        else
        {
            isRecording = false;

            if (buttonLabel != null)
                buttonLabel.text = "Sending…";

            Debug.Log("Recording stopped. Trimming and sending to backend...");

            int position = Microphone.GetPosition(microphoneDevice);
            Microphone.End(microphoneDevice);

            float[] soundData = new float[position];
            recordedClip.GetData(soundData, 0);

            AudioClip trimmedClip = AudioClip.Create("TrimmedSound", position, 1, 16000, false);
            trimmedClip.SetData(soundData, 0);
            recordedClip = trimmedClip;

            if (trimmedClip != null)
            {
                backendConnector.SendAudioToBackend(trimmedClip, transcriptionText);
            }

            // Reset label back to idle after a short delay
            Invoke(nameof(ResetLabelToIdle), 2f);
        }
    }

    private void ResetLabelToIdle()
    {
        if (buttonLabel != null)
            buttonLabel.text = "Start";
    }
}
