using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    [SerializeField]
    private float secondsToWaitBetweenLetters = 0.1f;

    [SerializeField]
    private TextMeshProUGUI dialogBox;

    [SerializeField]
    private GameObject endButton;

    [SerializeField]
    private AudioSource dialogSound;

    private void Awake()
    {
        StartCoroutine(DialogScrollCorutine());
    }

    private IEnumerator DialogScrollCorutine()
    {
        char[] charactersInDialog = dialogBox.text.ToCharArray();

        Queue<char> charctersToAdd = new Queue<char>();

        foreach (char c in charactersInDialog)
        {
            charctersToAdd.Enqueue(c);
        }

        dialogBox.text = "";

        dialogSound.Play();

        while (charctersToAdd.Count > 0)
        {
            dialogBox.text += charctersToAdd.Dequeue();

            yield return new WaitForSecondsRealtime(secondsToWaitBetweenLetters);
        }

        dialogSound.Stop();

        endButton.SetActive(true);
    }

    public void HideDialog()
    {
        Destroy(this.gameObject);
    }
}
