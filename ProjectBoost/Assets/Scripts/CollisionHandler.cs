using UnityEngine.SceneManagement;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float delay = 1f;
    [SerializeField] AudioClip successAudio;
    [SerializeField] AudioClip crashAudio;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    AudioSource audioSource;
    bool collisionDisabled = false;

    bool isTransitioning = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    private void Update()
    {
        RespondToDebugKeys();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collisionDisabled) { return; }

       switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Friendly Item");
                break;
            //case "Fuel":
            //    Debug.Log("Fuel");
            //    break;
            case "Finish":
                Debug.Log("Success");
                StartFinishSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void RespondToDebugKeys()
    {
        SkipToNextLevel();
        DisableCollisions();
    }

    void SkipToNextLevel()
    {
        if (Input.GetKey(KeyCode.L))
        {
            LoadNextLevel();
        }
    }

    void DisableCollisions()
    {
        if (Input.GetKey(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled; // toggle collision
        }
    }

    void StartFinishSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        GetComponent<Movement>().enabled = false;
        audioSource.PlayOneShot(successAudio);
        successParticles.Play();
        Invoke("LoadNextLevel", delay);

    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        GetComponent<Movement>().enabled = false;
        audioSource.PlayOneShot(crashAudio);
        crashParticles.Play();
        Invoke("ReloadLevel", delay);

    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}

