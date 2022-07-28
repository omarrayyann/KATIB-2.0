using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WriteLetters : MonoBehaviour
{
    public List<Vector2[]> strokes = new List<Vector2[]>();

    [SerializeField]
    private LineRenderer brushPrefab = null;
    [SerializeField]
    private LineRenderer animatorBrushPrefab = null;
    private List<LineRenderer> lineRenderers = new List<LineRenderer>();
    private LineRenderer currentLineRenderer = null;
    public string path = "";
    /*[SerializeField]
    private float padding = 0f;*/
    [SerializeField]
    private GameObject saveProgress;
    public bool completed = false;
    [SerializeField]
    private TMP_Text TimeText;
    private System.DateTime end;
    [SerializeField]
    public GameObject saveMenu;
    [SerializeField]
    public GameObject endMenu;
    private bool assignedTask;
    [SerializeField]
    private Button noButton;

    public void Start()
    {
        System.DateTime start = System.DateTime.Now;
        if (Data.currentTask.taskType.Equals(""))
        {
            assignedTask = false;
            saveProgress.SetActive(false);
        }
        else
        {
            assignedTask = true;
        }
        PointsFromFile(Data.currentTask.letter.LetterToPath());
        TimeText.gameObject.SetActive(Data.currentTask.timed);
        if (Data.currentTask.timed)
        {
            end = start.AddSeconds(Data.currentTask.time);
        }
        if (Data.currentTask.showPath)
            DrawPoints();
    }

    /// <summary>
    /// Loads all the point data from the given filepath into the strokes List according to the delimiters ' ' for coordinate seperation, '\n' for point seperation, and empty lines for stroke seperation
    /// </summary>
    /// <param name="filepath">The path of the csv file containing the points</param>
    public void PointsFromFile(string filepath)
    {
        string[] filelines = Server.ReadAllStringsFrom(filepath);
        GameObject magnet = GameObject.FindGameObjectWithTag("Magnet");
        KatibInformation katib = new KatibInformation(new List<Vector2>(), magnet, true);
        for (int i = 0; i < filelines.Length; i++)
        {
            List<Vector2> currentStroke = new List<Vector2>();
            for (; i < filelines.Length && !filelines[i].Equals(""); i++)
            {
                string[] coord = filelines[i].Split(',');
                float coord0 = (float)double.Parse(coord[0]);
                float coord1 = float.Parse(coord[1]);
                currentStroke.Add(new Vector2((coord0 * gameObject.transform.localScale[0]) - gameObject.transform.localScale[0] / 2, coord1 * gameObject.transform.localScale[1] - gameObject.transform.localScale[1] / 2));
                katib.points.Add(new Vector2(coord0 * gameObject.transform.localScale[0] - gameObject.transform.localScale[0] / 2, coord1 * gameObject.transform.localScale[1] - gameObject.transform.localScale[1] / 2));
            }
            strokes.Add(currentStroke.ToArray());
        }
        //KatibInteraction katibManager = GameObject.FindGameObjectWithTag("Katib").GetComponent<KatibInteraction>();
        //katibManager.katibInformation.Add(katib);
    }


    /// <summary>
    /// Draws the points in the given brush onto the assigned draw area
    /// </summary>
    public void DrawPoints()
    {
        GameObject SolutionStrokes = new GameObject("SolutionStrokes");
        Vector2 startPos = gameObject.transform.position;

        // Draws the magnet at its specified position
        GameObject magnet = GameObject.FindGameObjectWithTag("Magnet");
        magnet.transform.position = strokes[0][0] + startPos;

        strokes.ForEach(stroke =>
       {
           if (stroke.Length > 0)
           {
               currentLineRenderer = Instantiate(brushPrefab);
               currentLineRenderer.transform.SetParent(SolutionStrokes.transform);
               currentLineRenderer.SetPosition(0, stroke[0] + startPos);
               currentLineRenderer.SetPosition(1, stroke[0] + startPos);
               for (int i = 2; i < stroke.Length; i++)
               {
                   currentLineRenderer.SetPosition(currentLineRenderer.positionCount++, stroke[i] + startPos);
               }
               lineRenderers.Add(currentLineRenderer);
           }
       });
    }

    /// <summary>
    /// Creates an object with an animator component that animates the same points loaded in the strokes List and allows for pausing if the animation task is already running
    /// </summary>
    public void AnimatePoints()
    {
        GameObject PreviousAnimator = GameObject.FindGameObjectWithTag("Animator");
        if (PreviousAnimator == null)
        {
            GameObject Animator = new GameObject("Animator");
            AnimatedWriting animatedWriting = Animator.AddComponent<AnimatedWriting>();
            Animator.tag = "Animator";
            animatedWriting.brushPrefab = animatorBrushPrefab;
            animatedWriting.startPos = gameObject.transform.position;
            animatedWriting.LoadWithPoints(strokes);
        }
        else
        {
            PreviousAnimator.GetComponent<AnimatedWriting>().TogglePause();
        }
       
    }

    /// <summary>
    /// Toggles between showing and hiding the letter path
    /// </summary>
    public void SolutionToggle()
    {
        lineRenderers.ForEach(lineRenderer =>
        {
            lineRenderer.enabled = !lineRenderer.enabled;
        });
    }

    /*public string PickALetter()
    {
        return "/Resources/Letters/English/LowerCase/Data/a.csv";
    }*/

    private void Update()
    {
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

    public void Win()
    {
        completed = true;
        if (assignedTask)
        {
            saveMenu.GetComponent<MenuGroup>().disableOnClick = false;
            saveMenu.GetComponentsInChildren<Button>()[0].onClick.RemoveAllListeners();
            saveMenu.SetActive(true);
            noButton.onClick.RemoveAllListeners();
            noButton.onClick.AddListener(() => SceneManager.LoadScene("Main"));
        }
        else
        {
            endMenu.SetActive(true);
        }
    }

    public void Save()
    {
        if (completed)
            GameObject.FindGameObjectWithTag("DrawCamera").GetComponent<Draw>().SaveWork();
    }
}
