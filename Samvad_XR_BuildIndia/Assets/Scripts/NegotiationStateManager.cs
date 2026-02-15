using UnityEngine;

public class NegotiationStateManager : MonoBehaviour
{

    public  UnityEngine.UI.Text text;

    public SimpleBackendConnector simpleBackendConnector;

    // Update is called once per frame
    void Update()
    {
        text.text = simpleBackendConnector.negotiation_state;
    }
}
