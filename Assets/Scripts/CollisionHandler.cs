using NUnit.Framework.Constraints;
using Unity.Cinemachine;
using Unity.Jobs;
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

    private AudioSource audioSource;
    private bool isControllable = true;
    private bool isCollidable = true;
    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on the CollisionHandler GameObject.");
        }
    }

    private void Update()
    {
        ResponseDebugKey();
    }
    private void ResponseDebugKey()
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
            Debug.Log("Collision toggled: " + (isCollidable ? "Enabled" : "Disabled"));
        }
        else if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            isControllable = !isControllable;
            Debug.Log("Control toggled: " + (isControllable ? "Enabled" : "Disabled"));
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
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextScene", levelLoadDelay);
        Debug.Log("Success sequence started!");
    }
    private void StartCrashSequence()
    {
        isControllable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(crashSound);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadScene", levelLoadDelay);
        Debug.Log("Crash sequence started!");
        
    }
    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneCount = currentSceneIndex + 1;
        if (nextSceneCount == SceneManager.sceneCountInBuildSettings) {
            nextSceneCount = 0;
        }
        SceneManager.LoadScene(nextSceneCount);
    }
    private void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
