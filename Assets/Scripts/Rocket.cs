using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidbody;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
        Drunk();
    }

    private void Drunk()
    {
       // transform.Rotate(Vector3.forward);
    }

    private void ProcessInput()
    {
       if(Input.GetKey(KeyCode.Space)) //checking if  space is pressed
        {
            // print("Thrust");
            rigidbody.AddRelativeForce(Vector3.up);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

        }
       else
        {
            audioSource.Stop();
        }


        if(Input.GetKey(KeyCode.A)) // checking if A is pressed
        {
            // print("rotate left");
            transform.Rotate(Vector3.forward);
               
        }
        else if (Input.GetKey(KeyCode.D)) //checking if d is pressed
        {
            //print("rotate Right");
            transform.Rotate(-Vector3.forward);
        }
    }
}
