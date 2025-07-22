using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required for UI elements like Slider/Text


public class LoadingScreenController : MonoBehaviour
{
    [Header("UI References")]
    public Slider progressBar; // Assign in Inspector
    public Text progressText;  // Optional: Assign in Inspector


    [Header("Loading Settings")]
    [Tooltip("Minimum time in seconds the loading screen should be displayed after loading finishes.")]
    [Range(0f, 10f)] // Adjust range as needed
    public float minDisplayTime = 1.0f; // Default minimum display time of 1 second


    void Start()
    {
        // Ensure UI references are set if you intend to use them
        if (progressBar == null) {
             Debug.LogWarning("LoadingScreenController: ProgressBar is not assigned.");
        }
        if (progressText == null) {
             Debug.LogWarning("LoadingScreenController: ProgressText is not assigned.");
        }


        // Make sure a scene name was passed
        if (string.IsNullOrEmpty(SceneChangerKeybindings.sceneToLoad))
        {
            Debug.LogError("LoadingScreenController: No scene name specified to load! Check SceneChangerKeybindings.");
            // Optionally load a default scene or handle the error
            // SceneManager.LoadScene("MainMenu"); // Example fallback
            return;
        }


        // Start the asynchronous loading process
        StartCoroutine(LoadSceneAsync(SceneChangerKeybindings.sceneToLoad));
    }


    IEnumerator LoadSceneAsync(string sceneName)
    {
        Debug.Log($"Loading scene: {sceneName}");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);


        // Prevent the scene from activating immediately upon loading completion
        asyncLoad.allowSceneActivation = false;


        // --- Loading Phase ---
        // Update the progress bar while the scene is loading in the background
        // Loop until asyncLoad.progress reaches 0.9f (which means loading is complete)
        while (asyncLoad.progress < 0.9f)
        {
            // Normalize progress to be between 0.0 and 1.0
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            // Debug.Log("Loading progress: " + (progress * 100f) + "%");


            if (progressBar != null)
            {
                progressBar.value = progress;
            }
            if (progressText != null)
            {
                progressText.text = "Loading... " + (progress * 100f).ToString("F0") + "%";
            }


            yield return null; // Wait until the next frame
        }


        // --- Loading Complete Phase ---
        Debug.Log("Scene has finished loading. Starting minimum display time.");


        // Update UI to show 100% completion
        if (progressBar != null)
        {
            progressBar.value = 1f;
        }
        if (progressText != null)
        {
            // You might want a different message here, e.g., "Loading Complete!"
            progressText.text = "Loading... 100%";
        }


        // --- Delay Phase ---
        // Wait for the specified minimum display time
        if (minDisplayTime > 0)
        {
            yield return new WaitForSeconds(minDisplayTime);
        }


        // --- Activation Phase ---
        Debug.Log("Minimum display time elapsed. Activating the new scene.");
        asyncLoad.allowSceneActivation = true;


        // No need for 'while (!asyncLoad.isDone)' anymore, as setting
        // allowSceneActivation = true will let Unity handle the final activation step.
    }
}
