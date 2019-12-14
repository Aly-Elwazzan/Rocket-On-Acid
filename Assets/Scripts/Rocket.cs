using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidbody;
    AudioSource audioSource;


    [SerializeField] float mainThrust = 50f;
    [SerializeField] float drunkThrust = 600f;
    [SerializeField] float rcsThrust = 150f;  //reactionControl System

    bool checkInDrunk = true;

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

        Thrust();
        Rotate();
        
    }

    

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space)) //checking if  space is pressed
        {
            
            // print("Thrust");
            rigidbody.AddRelativeForce(Vector3.up* mainThrust);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

        }
        else
        {
            audioSource.Stop();
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
        float rotationThisFrame = drunkThrust * Time.deltaTime;
        if (checkInDrunk)
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
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                print("OK"); //todo remove
                break;
            case "Finish":
                print("Finish"); //todo remove
                break;
            default:
                print("Dead");
                // kill player
                break;
        }
    }
}
