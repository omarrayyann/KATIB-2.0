using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MazeRenderer : MonoBehaviour
{
    [SerializeField]
    private bool randomStartEnd = false;

    [SerializeField]
    public int width = 5;

    [SerializeField]
    public int height = 5;

    [SerializeField]
    public float cellSize = 1.5f;

    [SerializeField]
    public float wallWidth = 0.01f;

    [SerializeField]
    private GameObject wallPrefab = null;

    [SerializeField]
    private GameObject floorPrefab = null;

    [SerializeField]
    private GameObject playerPrefab = null;

    [SerializeField]
    private GameObject goalPrefab = null;

    private LineRenderer lineRenderer = null;

    private List<Vector2> points = new List<Vector2>();

    [SerializeField]
    private float minDistance = 0.1f;

    [SerializeField]
    public bool pathVisible = true;

    public int seed = -1;

    [SerializeField]
    private GameObject showSolutionButton = null;

    [SerializeField]
    public bool preview = false;

    [SerializeField]
    public GameObject previewWindow = null;

    [SerializeField]
    public GameObject saveMenu;
    [SerializeField]
    public GameObject endMenu;

    public bool completed = false;

    private static float[] difficultyWidths = new float[] { 1.5f, 2f, 3f };
    private static List<Vector2> difficultyDimensions = new List<Vector2>() { new Vector2(10f, 5f), new Vector2(8f, 4f), new Vector2(6f, 3f) };

    [SerializeField]
    private TMP_Text TimeText;

    private System.DateTime end;
    private bool assignedTask;

    // public int seed = -1;

    /*[SerializeField]
    private GameObject solutionBlock = null;*/

    private void Start()
    {
        if (Data.currentTask.taskType.Equals(""))
            assignedTask = false;
        else
            assignedTask = true;
        System.DateTime start = System.DateTime.Now;
        lineRenderer = GetComponent<LineRenderer>();
        /*if (Data.currentTask.seed != -1)
        {
            
        }*/
        cellSize = difficultyWidths[Data.currentTask.difficulty - 1];
        width = (int)difficultyDimensions[Data.currentTask.difficulty - 1][0];
        height = (int)difficultyDimensions[Data.currentTask.difficulty - 1][1];
        lineRenderer.enabled = Data.currentTask.showSolution;
        showSolutionButton.SetActive(Data.currentTask.showSolution);
        seed = Data.currentTask.seed;
        if (!preview)
        {
            TimeText.gameObject.SetActive(Data.currentTask.timed);
            if (Data.currentTask.timed)
            {
                end = start.AddSeconds(Data.currentTask.time);
            }
            lineRenderer.startWidth = 0.667f * cellSize;
            lineRenderer.endWidth = 0.667f * cellSize;

            WallState[,] maze = MazeGenerator.Generate(width, height, randomStartEnd, seed); // Generate a maze
            Draw(maze);
            List<Cell> solution = MazeGenerator.GetSolution();
            GameObject player = Instantiate(playerPrefab); // Create the player object
            player.transform.localScale *= cellSize;
            player.transform.SetParent(gameObject.transform);

            // Make the draw tool only draw upon activating the player
            Draw draw = GameObject.FindObjectOfType<Draw>();
            draw.needsFollow = true;
            draw.follower = player.GetComponent<FollowMouse>();

            GameObject goal = Instantiate(goalPrefab); // Create the Goal Object
            goal.transform.localScale *= cellSize;
            goal.transform.SetParent(gameObject.transform);

            // Move the player and goal to the correct position according to the solution
            player.transform.position = new Vector2((-width / 2.0f + 0.5f + solution[0].x) * cellSize, (height / 2.0f - 0.5f - solution[0].y) * cellSize);
            goal.transform.position = new Vector2((-width / 2.0f + 0.5f + solution[solution.Count - 1].x) * cellSize, (height / 2.0f - 0.5f - solution[solution.Count - 1].y) * cellSize);
        }
    }

    public void Preview()
    {
        cellSize *= previewWindow.transform.localScale[0] / 25;
        wallWidth *= previewWindow.transform.localScale[0] / 25;

        lineRenderer = this.GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.667f * cellSize;
        lineRenderer.endWidth = 0.667f * cellSize;

        WallState[,] maze = MazeGenerator.Generate(width, height, randomStartEnd, -1); // Generate a maze
        Draw(maze);
        List<Cell> solution = MazeGenerator.GetSolution();
        GameObject player = Instantiate(playerPrefab); // Create the player object
        player.transform.localScale *= cellSize;
        // player.GetComponent<FollowMouse>().enabled = false;

        // Make the draw tool only draw upon activating the player
        Draw draw = GameObject.FindObjectOfType<Draw>();
        draw.enabled = false;

        GameObject goal = Instantiate(goalPrefab); // Create the Goal Object
        goal.transform.localScale *= cellSize;

        // Move the player and goal to the correct position according to the solution
        player.transform.position = new Vector2((-width / 2.0f + 0.5f + solution[0].x) * cellSize, (height / 2.0f - 0.5f - solution[0].y) * cellSize);
        goal.transform.position = new Vector2((-width / 2.0f + 0.5f + solution[solution.Count - 1].x) * cellSize, (height / 2.0f - 0.5f - solution[solution.Count - 1].y) * cellSize);
    }

    /// <summary>
    /// Draws a maze from the given Wallstate array using the given prefabs
    /// </summary>
    /// <param name="maze">2D Wallstate array representing the maze's wall</param>
    private void Draw(WallState[,] maze)
    {
        // Create and manage the floor object
        GameObject floor = Instantiate(floorPrefab);
        floor.transform.position = new Vector2(0, 0);
        floor.transform.localScale = new Vector2(width * cellSize, height * cellSize);
        floor.transform.SetParent(this.transform);

        if (!preview)
        {
            Draw draw = GameObject.FindObjectOfType<Draw>();
            draw.canvas = floor.transform;
        }
        
        // Nested for loops for drawing every cell
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                WallState cell = maze[i, j];
                Vector2 position = new Vector2((-width / 2.0f + i) * cellSize, (height / 2.0f - j) * cellSize); // Turn the cell positions to world coordinates

                // Draw all walls according to the Wallstates
                if (cell.HasFlag(WallState.UP))
                {
                    GameObject topWall = Instantiate(wallPrefab);
                    topWall.transform.SetParent(this.transform);
                    topWall.transform.localScale = new Vector2(cellSize, wallWidth);
                    topWall.transform.position = position + new Vector2(cellSize / 2, -cellSize);
                }

                if (cell.HasFlag(WallState.LEFT))
                {
                    GameObject leftWall = Instantiate(wallPrefab);
                    leftWall.transform.SetParent(this.transform);
                    leftWall.transform.localScale = new Vector2(wallWidth, cellSize);
                    leftWall.transform.position = position + new Vector2(0, -cellSize / 2);
                }

                if (i == width - 1) // Only draw the right wall for the right-most cells
                {
                    if (cell.HasFlag(WallState.RIGHT))
                    {
                        GameObject rightWall = Instantiate(wallPrefab);
                        rightWall.transform.SetParent(this.transform);
                        rightWall.transform.localScale = new Vector2(wallWidth, cellSize);
                        rightWall.transform.position = position + new Vector2(cellSize, -cellSize / 2);
                    }
                }

                if (j == 0) // Only draw the bottom wall for the bottom-most cells
                {
                    if (cell.HasFlag(WallState.DOWN))
                    {
                        GameObject bottomWall = Instantiate(wallPrefab);
                        bottomWall.transform.SetParent(this.transform);
                        bottomWall.transform.localScale = new Vector3(cellSize, wallWidth);
                        bottomWall.transform.position = position + new Vector2(cellSize / 2, 0);
                    }
                }
            }

        }

        if (pathVisible)
        {
            List<Cell> solution = MazeGenerator.GetSolution();
            points = new List<Vector2>();
            points.Add(new Vector2((-width / 2.0f + 1 / 2.0f + solution[0].x) * cellSize, (height / 2.0f - 1 / 2.0f - solution[0].y) * cellSize));
            points.Add(new Vector2((-width / 2.0f + 1 / 2.0f + solution[1].x) * cellSize, (height / 2.0f - 1 / 2.0f - solution[1].y) * cellSize));
            lineRenderer.SetPosition(0, points[0]);
            lineRenderer.SetPosition(1, points[1]);
            for (int i = 2; i < solution.Count; i++)
            {
                points.Add(new Vector2((-width / 2.0f + 1 / 2.0f + solution[i].x) * cellSize, (height / 2.0f - 1 / 2.0f - solution[i].y) * cellSize));
                lineRenderer.SetPosition(lineRenderer.positionCount++, points[points.Count - 1]);
            }
            RefinePoints();
        }
        
    }

    private void RefinePoints()
    {
        int numPoints = Mathf.CeilToInt(cellSize / minDistance);
        if (numPoints > 1)
        {
            for (int i = points.Count - 1; i > 0; i--)
            {
                List<Vector2> pointsToAdd = SplitInto(numPoints, points[i - 1], points[i]);
                points.RemoveAt(i - 1);
                points.InsertRange(i - 1, pointsToAdd);
            }
        }
        /*points.ForEach(point =>
        {
            Debug.Log(point);
        });*/
    }


    public List<Vector2> SplitInto(int n, Vector2 point1, Vector2 point2)
    {
        List<Vector2> pointsToAdd = new List<Vector2>();
        if (point1.x == point2.x)
        {
            for (int i = 0; i < n; i++)
            {
                Vector2 point = new Vector2(point1.x, point1.y + i * ((point2.y - point1.y) / n));
                pointsToAdd.Add(point);
            }
        }
        else if (point1.y == point2.y) 
        {
            for (int i = 0; i < n; i++)
            {
                Vector2 point = new Vector2(point1.x + i * ((point2.x - point1.x) / n), point1.y);
                pointsToAdd.Add(point);
            }
        }
        else
        {
            pointsToAdd.Add(point1);
        }
        return pointsToAdd;
    }

    public void SolutionToggle()
    {

        lineRenderer.enabled = !lineRenderer.enabled;
    }

    public void Clear()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<FollowMouse>().enabled = true;
        GameObject goal = GameObject.FindGameObjectWithTag("Goal");
        List<Cell> solution = MazeGenerator.GetSolution();

        Draw draw = GameObject.FindObjectOfType<Draw>();
        draw.Clear();

        // Move the player and goal to the correct position according to the solution
        player.transform.position = new Vector2((-width / 2.0f + 0.5f + solution[0].x) * cellSize, (height / 2.0f - 0.5f - solution[0].y) * cellSize);
        goal.transform.position = new Vector2((-width / 2.0f + 0.5f + solution[solution.Count - 1].x) * cellSize, (height / 2.0f - 0.5f - solution[solution.Count - 1].y) * cellSize);
    }

    public void Win()
    {
        completed = true;
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
}