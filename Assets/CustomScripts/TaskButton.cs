using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using TMPro;

public class TaskButton : MonoBehaviour
{
    [SerializeField]
    public bool assignmentButton = true;
    public Task task = null;

    public void DeleteTask()
    {
        if (task.DeleteTask())
            Destroy(gameObject);
    }

    public void Clicked()
    {
        if (name.Contains("Collect"))
        {
            if (assignmentButton)
                Data.currentTask = task;
            else
                Data.currentTask = GenerateCollectingTask();
            SceneManager.LoadScene("CollectingTask");
        }
        else if (name.Contains("Maze"))
        {
            if (assignmentButton)
            {
                Data.currentTask = task;
            }
            else
            {
                Data.currentTask = GenerateMazeTask();
            }
            SceneManager.LoadScene("MazeTask");
        }
        else if (name.Contains("Hand"))
        {
            if (assignmentButton)
                Data.currentTask = task;
            else
                Data.currentTask = GenerateLetterTask();
            SceneManager.LoadScene("Writing");
        }
    }

    private Task GenerateCollectingTask()
    {
        Task task = new Task("");
        task.collectTask(System.DateTime.Now, false, 0, 0, 10, 1);
        return task;
    }

    private Task GenerateMazeTask()
    {
        Task task = new Task("");
        task.MazeTask(System.DateTime.Now, false, 0, 0, 1, -1, true);
        return task;
    }

    private Task GenerateLetterTask()
    {
        Task task = new Task("");
        task.LetterTask(System.DateTime.Now, false, 0, 1, new Letter("Ayn", "Arabic", "Alone"), true);
        return task;
    }

    public void UpdateCard()
    {
        TMP_Text[] texts = gameObject.GetComponentsInChildren<TMP_Text>();
        if (texts.Length == 3)
        {
            if (texts[0].text.Equals("Collecting Task"))
            {
                texts[1].text = texts[1].text.Replace("<HapticLevel>", task.hapticLevel + "");
                texts[1].text = texts[1].text.Replace("<Time>", task.timed ? task.time + "" : "Not timed");
                texts[1].text = texts[1].text.Replace("<Degree>", task.pathsType + "");
                texts[1].text = texts[1].text.Replace("<numTargets>", task.quantity + "");
            }
            else if (texts[0].text.Equals("Maze Task"))
            {
                texts[1].text = texts[1].text.Replace("<HapticLevel>", task.hapticLevel + "");
                texts[1].text = texts[1].text.Replace("<Time>", task.timed ? task.time + "" : "Not timed");
                texts[1].text = texts[1].text.Replace("<Difficulty>", task.difficulty + "");
                texts[1].text = texts[1].text.Replace("<Show>", task.showSolution + "");
            }
            else if (texts[0].text.Equals("Letter Task"))
            {
                texts[1].text = texts[1].text.Replace("<HapticLevel>", task.hapticLevel + "");
                texts[1].text = texts[1].text.Replace("<Time>", task.timed ? task.time + "" : "Not timed");
                texts[1].text = texts[1].text.Replace("<Letter>", task.letter + "");
                texts[1].text = texts[1].text.Replace("<Show>", task.showPath + "");
            }
            texts[2].text = "Created: " + task.AssigningTime();
        }
    }
}
