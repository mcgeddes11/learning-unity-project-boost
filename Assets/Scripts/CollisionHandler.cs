using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelTransitionDelay = 2f;
    [SerializeField] AudioClip crashClip;
    [SerializeField] AudioClip successClip;
    AudioSource audioSource;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;
    MeshRenderer meshRenderer;


    bool isTransitioning = false;
    bool collisionsEnabled = true;

    void Start(){
        audioSource = GetComponent<AudioSource>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    void Update(){
        ProcessDebugKeys();
    }
    void OnCollisionEnter(Collision other) 
    {
        if (isTransitioning || !collisionsEnabled){ return; }

        Debug.Log(other.gameObject.tag);
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Nah worries bruv!");
                break;
            case "Finish":
                Debug.Log("You done dawg!");
                StartSuccessSequence();
                break;
            default:
                Debug.Log("Wipe yourself off boy, you dead!");
                StartCrashSequence();
                break;
        }
    }

    void StartSuccessSequence()
    {
        isTransitioning = true;
        successParticles.Play();
        meshRenderer.enabled = false;
        audioSource.Stop();
        audioSource.PlayOneShot(successClip);
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelTransitionDelay);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings){
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        crashParticles.Play();
        meshRenderer.enabled = false;
        audioSource.Stop();
        GetComponent<Movement>().enabled = false;
        audioSource.PlayOneShot(crashClip);
        Invoke("ReloadLevel", levelTransitionDelay);
    }

    void ProcessDebugKeys(){
        if (Input.GetKey(KeyCode.L)){
            LoadNextLevel();
        }

        if (Input.GetKeyUp(KeyCode.C)){
            Debug.Log("Toggled collisions to " + !collisionsEnabled);
                collisionsEnabled = !collisionsEnabled;
            }
    }

    
}
