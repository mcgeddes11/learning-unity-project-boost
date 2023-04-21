using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    AudioSource audioSource;
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float lateralThrust = 100f;
    [SerializeField] AudioClip mainEngine;

    [SerializeField] ParticleSystem leftBoosterParticles;
    [SerializeField] ParticleSystem rightBoosterParticles;
    [SerializeField] ParticleSystem mainBoosterParticles;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();

    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * mainThrust* Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            mainBoosterParticles.Play();
            audioSource.PlayOneShot(mainEngine);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            mainBoosterParticles.Stop();
            audioSource.Pause();
        }


    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(lateralThrust);
            if (!rightBoosterParticles.isPlaying){
                rightBoosterParticles.Play();
            }
        } 
        else if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-lateralThrust);
            if (!leftBoosterParticles.isPlaying){
                leftBoosterParticles.Play();
            }
        }
         else {
            rightBoosterParticles.Stop();
            leftBoosterParticles.Stop();
         }
        
    }

    void ApplyRotation(float rotationThisFrame)
    {
        // need to freeze physics system rotation while we're controlling the rocket
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false;
    }
}
