using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marble : MonoBehaviour
{

    public Vector3 startingPosition = new Vector3();
    public Vector3 endingPosition = new Vector3();
    public float m = 0.0f;
    public float c = 0.0f;
    public bool movedFirst = false;

    public Marble(Vector3 start, Vector3 end, float m , float c){
        this.startingPosition = start;
        this.endingPosition = end;
        this.m = m;
        this.c = c;
    }

  
}
