using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] float mainThrust = 1200f;
    [SerializeField] float rotationThrust = 250f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;
    [SerializeField] ParticleSystem rocketJetParticles;

    Rigidbody myRigidbody;
    AudioSource audioSource;
    

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
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
            StartThrusting();
        }
        else
        {
            audioSource.Stop();
            rocketJetParticles.Stop();
        }

    }

    void StartThrusting()
    {
        myRigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if (!rocketJetParticles.isPlaying)
        {
            rocketJetParticles.Play();
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.D))
        {
            StartLeftThrust();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            StartRightThrust();
        }
        else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            StopSideThrusters();
        }
        else
        {
            StopSideThrusters();
        }

    }

    private void StopSideThrusters()
    {
        rightThrusterParticles.Stop();
        leftThrusterParticles.Stop();
    }

    private void StartRightThrust()
    {
        ApplyRotation(-rotationThrust);
        if (!rightThrusterParticles.isPlaying)
        {
            rightThrusterParticles.Play();
        }
    }

    private void StartLeftThrust()
    {
        ApplyRotation(rotationThrust);
        if (!leftThrusterParticles.isPlaying)
        {
            leftThrusterParticles.Play();
        }
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        myRigidbody.freezeRotation = true; // freezing rotation so that we can override physics.
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        myRigidbody.freezeRotation = false; // unfreezing rotation so that we can let physics take over.
    }

}
