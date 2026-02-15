using UnityEngine;

public class SliderUpdate : MonoBehaviour
{

    public UnityEngine.UI.Slider Slider;

    public SimpleBackendConnector backendConnector;


    // Update is called once per frame
    void Update()
    {
        Slider.value = (backendConnector.happiness_score)/ 100f;
    }
}
