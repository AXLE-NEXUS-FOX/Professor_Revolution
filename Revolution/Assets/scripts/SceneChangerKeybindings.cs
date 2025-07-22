using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneChangerKeybindings : MonoBehaviour
{
    public static string sceneToLoad; // Static variable to hold the target scene name
    public string loadingSceneName = "LoadingScene"; // Name of your loading scene

    public AudioClip clickSound; // Assign in Inspector
    private AudioSource audioSource;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }


    public void ChangeScene(string targetSceneName)
    {
        PlayClickSound();
        sceneToLoad = targetSceneName; // Store the target scene name
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
