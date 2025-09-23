using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Add this for UI components
using System.Collections;

public class SceneChangerKeybindings : MonoBehaviour
{
    public static string sceneToLoad; // Static variable to hold the target scene name
    public string loadingSceneName = "LoadingScene"; // Name of your loading scene

    public AudioClip clickSound; // Assign in Inspector
    private AudioSource audioSource;

    public Image fadeImage; // Assign a full-screen UI Image in Inspector
    public float fadeDuration = 1f; // Duration of fade


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }


    public void ChangeScene(string targetSceneName)
    {
        PlayClickSound();
        sceneToLoad = targetSceneName; // Store the target scene name
        StartCoroutine(FadeAndLoad());
    }


    private IEnumerator FadeAndLoad()
    {
        if (fadeImage != null)
        {
            float t = 0f;
            Color c = fadeImage.color;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
                fadeImage.color = c;
                yield return null;
            }
            c.a = 1f;
            fadeImage.color = c;
        }
        SceneManager.LoadScene(loadingSceneName); // Load the loading scene
    }


    public void Exit()
    {
        PlayClickSound();
        Application.Quit();
    }


    private void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
