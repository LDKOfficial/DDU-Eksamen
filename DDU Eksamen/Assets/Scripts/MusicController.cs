using System.Collections;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [Header("Fade in/out")]
    [SerializeField]
    private float fadeTimeOut;
    [SerializeField]
    private float fadeTimeIn;

    [Header("Audio Source")]
    [SerializeField]
    private AudioSource audioSource;

    [Header("Music")]
    [SerializeField]
    private AudioClip bossMusic;
    [SerializeField]
    private AudioClip normalMusic;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !collision.isTrigger)
        {
            Debug.Log("Player entered Boss Area");
            SwitchToBossMsuic();
            this.GetComponent<Collider2D>().enabled = false;
        }

    }

    public void SwitchToBossMsuic()
    {
        StartCoroutine(AudioSourceExit.CrossFade(audioSource, bossMusic, fadeTimeOut, fadeTimeIn));
    }

    public void SwitchToNormalMusic()
    {
        StartCoroutine(AudioSourceExit.CrossFade(audioSource, normalMusic, fadeTimeOut, fadeTimeIn));
    }
}

public static class AudioSourceExit
{
    public static IEnumerator CrossFade(AudioSource audioSource, AudioClip newMusic, float fadeTimeOut, float fadeTimeIn)
    {
        yield return FadeOut(audioSource, fadeTimeOut);
        audioSource.clip = newMusic;
        yield return FadeIn(audioSource, fadeTimeIn);
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;
        while(audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = 0;
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float fadeTime)
    {
        float startVolume = 0.2f;
        audioSource.volume = 0;
        audioSource.Play();
        while(audioSource.volume < 1)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        audioSource.volume = 1f;
    }
}