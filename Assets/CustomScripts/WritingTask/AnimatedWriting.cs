using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedWriting : MonoBehaviour
{
    public List<Vector2[]> strokes = null;

    public LineRenderer brushPrefab = null;

    private List<LineRenderer> lineRenderers = new List<LineRenderer>();

    private Vector2[] currentStroke;

    private LineRenderer currentLineRenderer = null;

    private int currentPointIndex = 0;

    private int currentStrokeIndex = 0;

    private float bigDelta = 0;

    public bool paused = false;

    public Vector2 startPos = new Vector2();

    [SerializeField]
    private float waitTime = 0.01f;


    /// <summary>
    /// Loads the animator component with the points it needs to draw out. Each stroke is a Vector2 array
    /// </summary>
    /// <param name="strokes">List of Vector2 arrays that representing the individual strokes of the writing</param>
    public void LoadWithPoints(List<Vector2[]> strokes)
    {
        if (strokes.Count > 0)
        {
            this.strokes = strokes;
            currentStroke = strokes[currentStrokeIndex];
            currentLineRenderer = Instantiate(brushPrefab);
            currentLineRenderer.transform.SetParent(transform);
            lineRenderers.Add(currentLineRenderer);
            currentLineRenderer.SetPosition(0, currentStroke[currentPointIndex] + startPos);
            currentLineRenderer.SetPosition(1, currentStroke[currentPointIndex] + startPos);
        }
    }


    /// <summary>
    /// Checks whether the strokes parameter has been loaded with the necessary data
    /// </summary>
    /// <returns>Whether the strokes parameter is loaded or not</returns>
    public bool Loaded()
    {
        return strokes != null;
    }

    private void Update()
    {
        bigDelta += Time.deltaTime;
        if (!paused && Loaded() && timePassed(waitTime)) // If all necessary prerequisites are fulfilled
        {
            AddPoint();
            bigDelta = 0; // Reset time counter
        }
    }

    /// <summary>
    /// Adds points to the appropriate line renderer
    /// </summary>
    private void AddPoint()
    {
        if (currentPointIndex < currentStroke.Length) // If there are still points in the current stroke
            currentLineRenderer.SetPosition(currentLineRenderer.positionCount++, currentStroke[currentPointIndex++] + startPos);
        else if (currentStrokeIndex < strokes.Count - 1) // If there are still any more strokes
        {
            currentLineRenderer = Instantiate(brushPrefab);
            currentLineRenderer.transform.SetParent(this.transform);
            lineRenderers.Add(currentLineRenderer);
            currentStroke = strokes[++currentStrokeIndex];
            currentPointIndex = 0;
            currentLineRenderer.SetPosition(0, currentStroke[currentPointIndex] + startPos);
            currentLineRenderer.SetPosition(1, currentStroke[currentPointIndex] + startPos);
        }
        else // If all points of all strokes have been drawn
        {
            StartCoroutine(end(1f));
        }
    }


    /// <summary>
    /// Toggles the pause state
    /// </summary>
    public void TogglePause()
    {
        paused = !paused;
    }

    /// <summary>
    /// Checks if it has been at least minTime since it was last called
    /// </summary>
    /// <param name="minTime">The minimum amount of time for the function to return a true value</param>
    /// <returns>Whether the time has passed or not</returns>
    private bool timePassed(float minTime)
    {
        return bigDelta >= minTime;
    }

    /// <summary>
    /// Keeps the animated strokes visible for a little before completely destroying the script and all the strokes completely
    /// </summary>
    /// <param name="waitTime">How long to keep the strokes before destroying them</param>
    /// <returns></returns>
    IEnumerator end(float waitTime)
    {
        GameObject button = GameObject.FindGameObjectWithTag("AnimatorButton");
        button.GetComponent<Button>().enabled = false;
        yield return new WaitForSeconds(waitTime);
        button.GetComponentInChildren<ChangeImage>().ChangeBy(1);
        button.GetComponent<Button>().enabled = true;
        Destroy(this.gameObject);
    }

}
