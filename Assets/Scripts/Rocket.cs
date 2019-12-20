using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidbody;
    AudioSource audioSource;


    [SerializeField] float mainThrust = 50f;
    [SerializeField] float drunkThrust = 600f;
    [SerializeField] float rcsThrust = 150f;  //reactionControl System
    [SerializeField] int loadLevelDelay = 1;

    [SerializeField] AudioClip mainEngineSound;
    [SerializeField] AudioClip doneLevelSound;
    [SerializeField] AudioClip deathSound;

    System.Random rnd = new System.Random();
    


    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem doneLevelParticles;
    [SerializeField] ParticleSystem deathParticles;

    bool checkInDrunk = true;
    enum  State { Alive,Dying,Transcending};
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(DrunkRotation());
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
        
    }

    private  void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }
    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space)) //checking if  space is pressed
        {
            
            rigidbody.AddRelativeForce(Vector3.up* mainThrust*Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngineSound);
                mainEngineParticles.Play();

            }
            //mainEngineParticles.Play();

        }
        else
        {
            audioSource.Stop();
           mainEngineParticles.Stop();
        }
    }

    private void Rotate()
    {
        rigidbody.freezeRotation = true;// take manual control of rotation
        
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A)) // checking if A is pressed
        {
            
            transform.Rotate(Vector3.forward*rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D)) //checking if d is pressed
        {
            //print("rotate Right");
            
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidbody.freezeRotation = false;// resume physics control of  rotation
    }

    IEnumerator DrunkRotation()
    {
        int drunkDirection = rnd.Next(2);
        float rotationThisFrame = drunkThrust * Time.deltaTime;
        if (drunkDirection==0)
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
            checkInDrunk = false;
        }
        else
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
            checkInDrunk = true;
        }


        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(1);

        //After we have waited 5 seconds print the time again.
        StartCoroutine(DrunkRotation());

    }

    void OnCollisionEnter(Collision collision)
    {
        if(state!=State.Alive){ return;}

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default: 
                // kill player
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        doneLevelParticles.Play();
        audioSource.PlayOneShot(doneLevelSound);
        Invoke("LoadNextScene", loadLevelDelay);
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        deathParticles.Play();
        audioSource.PlayOneShot(deathSound);
        Invoke("LoadFirstScene", loadLevelDelay);
    }
}
