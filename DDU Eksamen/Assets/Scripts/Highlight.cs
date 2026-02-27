using UnityEngine;

public class Highlight : MonoBehaviour
{

    [SerializeField]
    private GameObject validSelectionHighlight;

    [SerializeField]
    private GameObject selectedHighlight;

    public void ToggleValidSelectionHighlight(bool state)
    {
        validSelectionHighlight.SetActive(state);
    }

    public void ToggleSelectedHighlight(bool state)
    {
        selectedHighlight.SetActive(state);
    }
}
