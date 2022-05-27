using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour

{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    void Update()
    {
       


        // if (circle_collider.){
        //     print("HELLO");
        //     float x = UnityEngine.Random.Range(-0.39f,0.39f);
        //     float radius = UnityEngine.Random.Range(0.08f,0.38f);

        //     if (UnityEngine.Random.Range(0f,10f)>5){
        //     float z = Mathf.Sqrt ((float)(Math.Pow(radius, 2.0f) - Math.Pow(x, 2.0f)));
        //     Vector3 temp = new Vector3(x, 0.688f, z);
        //     transform.position = temp;
        //     }
        //     else{
        //     float z = -1.0f*Mathf.Sqrt ((float)(Math.Pow(radius, 2.0f) - Math.Pow(x, 2.0f)));
        //     Vector3 temp = new Vector3(x, 0.688f, z);
        //     transform.position = temp;
        //     }
            
        // }
        // else{
        //                 print("NOOOO");

        // }
    }

int b = 1;
 void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Circle_Collider"){
            float x = 0;
            float z = 0;
            while((x*x + z*z)<(0.27f*0.27f)){
            x = UnityEngine.Random.Range(-0.78f,0.78f);
            z = UnityEngine.Random.Range(-0.45f,0.45f);
            }
            Vector3 newPosition = new Vector3(x, 0.6,z);
            transform.position = newPosition;
            gameObject.GetComponent<Rigidbody>().useGravity = true;

        };
      


    }

}
