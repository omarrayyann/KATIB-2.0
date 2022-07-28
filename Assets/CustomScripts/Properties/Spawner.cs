using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        NewSpawn(2);
    }

    // Update is called once per frame
    void Update()
    {
        // Magnet Simulator
      



        // DrawLine(positions);}
        // else{
        // Vector3[] positions = new Vector3[0] {};
        // DrawLine(positions);}


        
        // if (new Vector2(transform.position.x, transform.position.y)==Data.collectables[Data.collectables.Count-1].endingPosition){
        //     NewSpawn(1);
        // }
        
    }

    void OnMouseUp(){
        if ((Mathf.Pow(transform.position.x,2f)+Mathf.Pow(transform.position.y, 2f))<1.5){
            NewSpawn(2);
        }

    }

    void NewSpawn(int pathDegree){


            // Random Starting Point
            float x = 0f;
            float y = 0f;
            while((x*x + y*y)<(1.6f*1.6f)){
            y = UnityEngine.Random.Range(Config.minY,Config.maxY);
            x = UnityEngine.Random.Range(Config.minX,Config.maxX);
            }

            Vector2 startingPosition = new Vector2(x, y);
            Vector2 endingPosition = new Vector2(0, 0);
            List<Vector3> path = new List<Vector3>();




            float domainRange = endingPosition.x - startingPosition.x;

            float numberOfPoints = 100;

            float xStepSize = domainRange/numberOfPoints;




            // Path Equation and Points Genereator
            switch(pathDegree){

                // Linear Line
                case 1:
                    path.Clear();
                    Debug.Log("HELLO");

                    float m = (startingPosition.y-endingPosition.y)/(startingPosition.x-endingPosition.x);
                    float c = startingPosition.y - m*startingPosition.x;
                    Debug.Log(m);
                    Debug.Log(c);

                    float distance = Mathf.Pow( Mathf.Pow((startingPosition.x - endingPosition.x),2f) + Mathf.Pow((startingPosition.y - endingPosition.y),2f), 0.5f );


                    // float numberOfPoints = (int)(distance/requiredStepDisatnce) + 1;
            
                    for (int i = 0; i<numberOfPoints; i=i+1){
                        x = startingPosition.x + xStepSize*i;
                        y = m*x+c;
                        Vector3 point = new Vector3(x,y, 100);
                        path.Add(point);

                    }


                    break;
                // Quadratic Line
                case 2:
                
                float stationaryPointY = 999999;
                float stationaryPointX = 999999;

                Debug.Log("maxx:" + Config.maxX + " maxy " + Config.maxY);

                while(stationaryPointY>Config.maxY || stationaryPointY<Config.minY || stationaryPointX<Config.minX || stationaryPointX>Config.maxX){
                    float betweenPointX = UnityEngine.Random.Range(startingPosition.x, endingPosition.x);
                    float betweenPointY = UnityEngine.Random.Range(startingPosition.y, endingPosition.y);
                    
                    Vector2 betweenPoint = new Vector2(betweenPointX, betweenPointY);
                    path.Clear();

                    float x1 = startingPosition.x;
                    float x2 = betweenPoint.x;
                    float x3 = endingPosition.x;
                    float y1 = startingPosition.y;
                    float y2 = betweenPoint.y;
                    float y3 = endingPosition.y;
                    
                    float A = (y1-y3)/(Mathf.Pow(x1,2f)-2*x2*x1-Mathf.Pow(x3,2f)+2*x2*x3);
                    float B = -2*A*x2;
                    float C = y1 - A*Mathf.Pow(x1,2f) - B*x1;

                    stationaryPointX = -B/(2*A);
                    stationaryPointY = A*(Mathf.Pow(stationaryPointX,2))+B*stationaryPointX+C;
                

                for (int i = 0; i<numberOfPoints; i=i+1){
                    x = startingPosition.x + xStepSize*i;
                    y = A*(Mathf.Pow(x,2))+B*x+C;
                    Vector3 point = new Vector3(x,y,100);
                    path.Add(point);

                                    }
                }


    
                    break;
                default:
                    break;

            }

            Collectable newCollectable = new Collectable(startingPosition, endingPosition, path);
            Data.collectables.Add(newCollectable);
            
            transform.position = startingPosition;


    }


    
}
