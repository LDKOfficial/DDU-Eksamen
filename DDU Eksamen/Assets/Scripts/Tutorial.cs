using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    private int screenNumber = 0;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject playerInput;

    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private GameObject endTurnButton;

    public void UpdateTutorialScreen()
    {
        if (screenNumber == 6 || screenNumber == 0) //it's both 0 and 6, since the last screen is the 5th, and the start tutorial button just calls this method and screen six is the hide everything
        {
            GameObject playerFocus = GameObject.FindGameObjectWithTag("Player");
            mainCamera.transform.position = new Vector3(playerFocus.transform.position.x, playerFocus.transform.position.y, -1);
            transform.position = playerFocus.transform.position;
            playerInput.SetActive(false);
            if (endTurnButton.TryGetComponent<Button>(out Button button))
            {
                button.enabled = false;
            }

            screenNumber = 0;
            Debug.Log("Starting tutorial");
        }
        else if (screenNumber == 5)
        {
            playerInput.SetActive(true);
            if (endTurnButton.TryGetComponent<Button>(out Button button))
            {
                button.enabled = true;
            }
            Debug.Log("Tutorial finsihed");
        }

        screenNumber++;
        animator.SetInteger("Screen", screenNumber);
        Debug.Log($"Screen number {screenNumber}");
    }
}
