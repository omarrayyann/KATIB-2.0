using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable
{

    public Vector2 startingPosition = new Vector2();
    public Vector2 endingPosition = new Vector2();
    public List<Vector3> path = new List<Vector3>();

    public Collectable(Vector2 start, Vector2 end, List<Vector3> path){
        this.startingPosition = start;
        this.endingPosition = end;
        this.path = path;
    }

  
}
