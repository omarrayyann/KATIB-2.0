using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour

{

    Vector3 endingPosition = new Vector3(0, 0.7f, 0);
    
    [SerializeField]
    private AudioClip marbleClip;
    private AudioSource audiosource;


    // Start is called before the first frame update
    void Start()
    {
       audiosource = GetComponent<AudioSource>();
       audiosource.clip = marbleClip;
    }

    // Update is called once per frame

    void Update()
    {
            Debug.DrawLine(transform.position, new Vector3 (0f, (float)0.7, 0f), Color.red);
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, gameObject.GetComponent<Rigidbody>().velocity.y,0);


            //  Vector3 mousePos = Input.mousePosition;
            // {
            //     Debug.Log(mousePos.x);
            //     Debug.Log(mousePos.y);
            // }
    }

    bool first = false;


    


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag=="Ground"){
            if (first){
                Debug.Log("PLAYY");
                audiosource.Play();
            }
            first = true;
        }

        if(collision.gameObject.tag=="Circle_Collider"){

            // New Marble 

            float x = 0f;
            float z = 0f;

            while((x*x + z*z)<(0.27f*0.27f)){
            x = UnityEngine.Random.Range(-0.78f,0.78f);
            z = UnityEngine.Random.Range(-0.45f,0.45f);
            }

            Vector3 startingPosition = new Vector3(x, 0.9f,z);
            
            // Line Equation

            float m = (startingPosition.y-endingPosition.y)/(startingPosition.x-endingPosition.x);
            float c = startingPosition.y - m*startingPosition.x;

            Marble newMarble = new Marble(startingPosition, endingPosition, m, c);
            Data.marbles.Add(newMarble);
            Debug.Log("Current Number of Marbles: " + Data.marbles.Count);
            
            transform.position = startingPosition;
            gameObject.GetComponent<Rigidbody>().useGravity = true;

        };
      


    }




}
