using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float levelLoadDelay = 2f;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip crashSound;
    [SerializeField] private ParticleSystem successParticles;
    [SerializeField] private ParticleSystem crashParticles;
    [SerializeField] private AudioSource audioSource; 
    [SerializeField] private Movement movement;

    private bool isControllable = true;
    private bool isCollidable = true;

    private void Update()
    {
        HandleDebugKeys();
    }

    private void HandleDebugKeys()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            LoadNextScene();
        }
        else if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            ReloadScene();
        }
        else if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            isCollidable = !isCollidable;
            Debug.Log($"Collision toggled: {(isCollidable ? "Enabled" : "Disabled")}");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isControllable || !isCollidable) return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Friendly collision detected.");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            case "Fuel":
                Debug.Log("Fuel collected!");
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        isControllable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(successSound);
        successParticles.Play();
        movement.enabled = false;
        Invoke(nameof(LoadNextScene), levelLoadDelay);
        Debug.Log("Success sequence started!");
    }

    private void StartCrashSequence()
    {
        isControllable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(crashSound);
        crashParticles.Play();
        movement.enabled = false;
        Invoke(nameof(ReloadScene), levelLoadDelay);
        Debug.Log("Crash sequence started!");
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
