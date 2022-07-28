using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using TMPro;

public class Draw : MonoBehaviour
{
    [SerializeField]
    private GameObject brush = null;

    private LineRenderer currentLineRenderer;

    [SerializeField]
    public float minDistance = 0.01f;

    [SerializeField]
    public bool needsFollow;

    public FollowMouse follower { get; set; }

    [SerializeField]
    private Camera DrawCamera = null;

    public GameObject strokes = null;

    public Transform canvas = null;
    public bool forceInactive = false;

    Vector2 lastPos;

    public List<Vector3> drawnPoints = new List<Vector3>();
    public DateTime drawingStartTime = new DateTime();

    private void Start()
    {
        strokes = new GameObject("Strokes");
    }

    public void setForceInactive(bool force)
    {
        forceInactive = force;
    }
    private void Update()
    {
        if (!forceInactive)
            Drawing();
    }

    /// <summary>
    /// Changes the draw state according to the criteria
    /// </summary>
    public void Drawing()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && InDrawArea() && (!needsFollow || follower.follow /*Short circuiting to avoid possible reading of empty object*/))
        {
            CreateBrush(); // Creates a new brush upon each new mouse click on the object
        }
        else if (currentLineRenderer != null && Input.GetKey(KeyCode.Mouse0) && InDrawArea() && (!needsFollow || follower.follow /*Short circuiting to avoid possible reading of empty object*/))
        {
            PointToMousePos(); // Adds points to the line renderer if the mouse is pressed while moving
        }
        else
        {
            currentLineRenderer = null;
            if (drawnPoints.Count>0)
            {
                if (drawnPoints[drawnPoints.Count-1].x != -9999f){
                    drawnPoints.Add(new Vector3(-9999f, -9999f, -9999f));
                }
            }
        }
    }

    private bool InDrawArea()
    {
        Vector2 mousePosition = DrawCamera.ScreenToWorldPoint(Input.mousePosition);
        bool answer = mousePosition.x >= canvas.position.x - (1 / 2.0f) * canvas.localScale.x && mousePosition.x <= canvas.position.x + (1 / 2.0f) * canvas.localScale.x;
        answer = answer && mousePosition.y >= canvas.position.y - (1 / 2.0f) * canvas.localScale.y && mousePosition.y <= canvas.position.y + (1 / 2.0f) * canvas.localScale.y;
        return answer;
    }

    /// <summary>
    /// Creates a new brush
    /// </summary>
    void CreateBrush()
    {
        // Creating a new brush
        GameObject brushInstance = Instantiate(brush);
        brushInstance.transform.SetParent(strokes.transform);
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();

        Vector2 mousePos = DrawCamera.ScreenToWorldPoint(Input.mousePosition);
        //At least 2 points are needed for the line renderer to render the path => the mouse position is added as the first and second points
        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);

    }

    /// <summary>
    /// Adds a point to the current line renderer
    /// </summary>
    /// <param name="pointPos">Point to be added</param>
    void AddAPoint(Vector2 pointPos)
    {
        currentLineRenderer.positionCount++;
        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(positionIndex, pointPos);
    }

    /// <summary>
    /// Adds the mouse position to the line renderer
    /// </summary>
    void PointToMousePos()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Distance(lastPos, mousePos) >= minDistance) // Only adds points upon movement
        {
            AddAPoint(mousePos);
            if (drawnPoints.Count == 0) {
                drawingStartTime = DateTime.Now;
            }
            var secondPassedSinceStartingToDraw = (DateTime.Now - drawingStartTime).TotalSeconds;
            float xDrawnScaled = (mousePos.x - canvas.position.x + (canvas.localScale.x)/2)/canvas.localScale.x ;
            float yDrawnScaled = ((canvas.localScale.y)/2 + mousePos.y - canvas.position.y) /canvas.localScale.y ;
            drawnPoints.Add(new Vector3 (xDrawnScaled, yDrawnScaled, (float)secondPassedSinceStartingToDraw));
            lastPos = mousePos;
        }
    }

    /// <summary>
    /// Clears them screen from the strokes by destroying the strokes object
    /// </summary>
    public void Clear()
    {
        GameObject.Destroy(strokes);
        strokes = new GameObject("Strokes");
        drawnPoints = new List<Vector3>();
    }


    public void AddCustomLetter(TMP_InputField input)
    {
        string name = input.text;
        Server.AddLetter(drawnPoints, name, "Custom", "All");
    }


    public void SaveWork()
    {
        Server.SavePerformance(drawnPoints, Data.currentTask.taskID);
        Debug.Log("Saved " + drawnPoints.Count + " points for task " + Data.currentTask.taskID);
    }

    private float Distance(Vector2 point1, Vector2 point2)
    {
        return Mathf.Sqrt(Mathf.Pow(point1.x - point2.x, 2) + Mathf.Pow(point1.y - point2.y, 2));
    }
}