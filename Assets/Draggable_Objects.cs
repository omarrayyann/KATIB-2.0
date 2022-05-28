using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable_Objects : MonoBehaviour
{

    bool heightSet = false;

    [SerializeField]
    public LineRenderer lineRenderer;

    private Transform[] points;


    void Start()
    {

    }
    
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y == 0.7f){
        Vector3[] positions = new Vector3[2] {transform.position, new Vector3(0f, (float)0.7, 0f) };
        DrawLine(positions);}
        else{
         Vector3[] positions = new Vector3[0] {};
        DrawLine(positions);}
    }

       
     
    void OnMouseExit()
    {
       

    } 
    
 void OnMouseUp()
 {
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        heightSet = false;
 } //MouseUp

   void OnMouseDrag()
{
     if (heightSet == false){
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            Vector3 temp = new Vector3(transform.position.x, (float)0.7, transform.position.z);
            transform.position = temp;
            heightSet = true;
            
        }

    //    Draw Line 
   

    Plane dragPlane = new Plane(Camera.main.transform.forward, transform.position);
    Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    float enter = 0.0f;
    if (dragPlane.Raycast(camRay, out enter)) 
    {
        float currentX = transform.position.x;
        float currentZ = transform.position.z;

        Marble currentMarble = Data.marbles[Data.marbles.Count - 1];
        
        float supposedZ = currentMarble.m * currentX + currentMarble.c;

        float differenceZ = supposedZ-currentZ;


        
        Vector3 fingerPosition = camRay.GetPoint(enter);
        Vector3 temp = new Vector3(fingerPosition.x, (float)0.7, fingerPosition.z);
        transform.position = temp;

        

       
    }
}

    void DrawLine(Vector3[] vertexPositions)
    {
        
        lineRenderer.positionCount = vertexPositions.Length;
        lineRenderer.SetPositions(vertexPositions);
    }



}