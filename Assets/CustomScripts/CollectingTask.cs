using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class CollectingTask : MonoBehaviour
{
    [SerializeField]
    public LineRenderer lineRenderer;
    private Vector3 screenPoint;
    private Vector3 offset;
    Collectable currentCollectable = null;

    Task currentTask = null;
    int howMany = 0;

    bool assignedTask = false;

    public bool completed = false;
    [SerializeField]
    public GameObject saveMenu;
    [SerializeField]
    public GameObject endMenu;

    [SerializeField]
    private TMP_Text TimeText;

    private System.DateTime end;

    // Start is called before the first frame update
    void Start()
    {
        System.DateTime start = System.DateTime.Now;
        if (Data.currentTask.taskType.Equals(""))
        {
            assignedTask = false;
        }
        else
        {
            assignedTask = true;
        }
        currentTask = Data.currentTask;
        howMany = currentTask.quantity;
        TimeText.gameObject.SetActive(currentTask.timed);
        if (currentTask.timed)
            end = start.AddSeconds(currentTask.time);
        NewSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        DrawLine(currentCollectable.path);
        if (Data.currentTask.timed && !completed)
            UpdateTime();
    }

    private void UpdateTime()
    {
        System.TimeSpan difference = end - System.DateTime.Now;
        string timeText = difference.Hours + ":" + difference.Minutes + ":" + difference.Seconds;
        TimeText.text = timeText;
        if (difference.TotalSeconds <= 0)
            Win();
    }

   void OnMouseDown() {

    	offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}

    void OnMouseDrag()
	{

        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
    	Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        if (Vector2.Distance(currentCollectable.path[0], new Vector2(curPosition.x, curPosition.y))<0.2){
            transform.position = currentCollectable.path[0];
            currentCollectable.path.RemoveAt(0);
        }


	}


    void DrawLine(List<Vector3> vertexPositions)
    {
        
        lineRenderer.positionCount = vertexPositions.Count;
        lineRenderer.SetPositions(vertexPositions.ToArray());
    }

    void OnMouseUp(){
        if ((Mathf.Pow(transform.position.x,2f)+Mathf.Pow(transform.position.y, 2f))<1.5){
            if (howMany>0/* || !assignedTask*/){
            NewSpawn();}
            else{
                Win();
            }
        }

    }

    void NewSpawn(){

        howMany -= 1;

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

            int pathDegree = 3;

            /*if (assignedTask){
            pathDegree = currentTask.pathsType;}*/

            if (pathDegree == 3){
                float randomNumber = Random.Range(0f,1f);
                if (randomNumber<0.5f){
                    pathDegree=1;
                }
                else{
                    
                    pathDegree=2;
                }
                
            }

            // Path Equation and Points Genereator
            switch(pathDegree){

                // Linear Line
                case 1:
                    path.Clear();

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
            currentCollectable = new Collectable(startingPosition, endingPosition, path);
            transform.position = startingPosition;
    }

    public void Win()
    {
        completed = true;
        GameObject.FindGameObjectWithTag("DrawCamera").GetComponent<Draw>().forceInactive = true;
        if (assignedTask)
            saveMenu.SetActive(true);
        else
            endMenu.SetActive(true); 
    }

    public void Save()
    {
        if (completed)
            GameObject.FindGameObjectWithTag("DrawCamera").GetComponent<Draw>().SaveWork();
    }
}
