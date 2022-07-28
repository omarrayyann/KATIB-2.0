using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AssignedTasks : MonoBehaviour
{
    [SerializeField]
    private GameObject CollectingTaskButton;
    [SerializeField]
    private GameObject MazeTaskButton;
    [SerializeField]
    private GameObject LetterTaskButton;

    private List<GameObject> buttons = new List<GameObject>();

    [SerializeField]
    private GameObject NoTaskMessage = null;

    [SerializeField]
    private bool onStart = true;

    private void Start()
    {
        NoTaskMessage.SetActive(false);
        if (onStart)
            LoadTasks();
    }

    public void LoadTasks()
    {
        UnloadTasks();
        Data.tasks = Server.LoadLearnerTasks();
        if (Data.tasks.Count == 0)
        {
            NoTaskMessage.SetActive(true);
        }
        else
        {
            NoTaskMessage.SetActive(false);
        }
        foreach (Task task in Data.tasks)
        {
            if (!task.completed)
            {
                if (task.taskType.Equals("Collecting Task"))
                {
                    buttons.Add(Instantiate(CollectingTaskButton));
                    buttons[buttons.Count - 1].transform.SetParent(transform);
                    buttons[buttons.Count - 1].transform.localScale = Vector2.one;
                    buttons[buttons.Count - 1].GetComponent<TaskButton>().task = task;
                    buttons[buttons.Count - 1].GetComponent<TaskButton>().UpdateCard();
                }
                else if (task.taskType.Equals("Maze Task"))
                {
                    buttons.Add(Instantiate(MazeTaskButton));
                    buttons[buttons.Count - 1].transform.SetParent(transform);
                    buttons[buttons.Count - 1].transform.localScale = Vector2.one;
                    buttons[buttons.Count - 1].GetComponent<TaskButton>().task = task;
                    buttons[buttons.Count - 1].GetComponent<TaskButton>().UpdateCard();
                }
                else if (task.taskType.Equals("Letter Task"))
                {
                    buttons.Add(Instantiate(LetterTaskButton));
                    buttons[buttons.Count - 1].transform.SetParent(transform);
                    buttons[buttons.Count - 1].transform.localScale = Vector2.one;
                    buttons[buttons.Count - 1].GetComponent<TaskButton>().task = task;
                    buttons[buttons.Count - 1].GetComponent<TaskButton>().UpdateCard();
                }
            }
        }
        this.gameObject.GetComponent<SizeToScroll>().Refit();
    }

    public void LoadClassesTasks()
    {
        UnloadTasks();
        Data.tasks = Server.LoadClassesTasks();
        if (Data.tasks.Count == 0)
        {
            NoTaskMessage.SetActive(true);
        }
        else
        {
            NoTaskMessage.SetActive(false);
        }
        foreach (Task task in Data.tasks)
        {
            if (!task.completed)
            {
                if (task.taskType.Equals("Collecting Task"))
                {
                    buttons.Add(Instantiate(CollectingTaskButton));
                    buttons[buttons.Count - 1].transform.SetParent(transform);
                    buttons[buttons.Count - 1].transform.localScale = Vector2.one;
                    buttons[buttons.Count - 1].GetComponent<TaskButton>().task = task;
                    buttons[buttons.Count - 1].GetComponent<TaskButton>().UpdateCard();
                }
                else if (task.taskType.Equals("Maze Task"))
                {
                    buttons.Add(Instantiate(MazeTaskButton));
                    buttons[buttons.Count - 1].transform.SetParent(transform);
                    buttons[buttons.Count - 1].transform.localScale = Vector2.one;
                    buttons[buttons.Count - 1].GetComponent<TaskButton>().task = task;
                    buttons[buttons.Count - 1].GetComponent<TaskButton>().UpdateCard();
                }
                else if (task.taskType.Equals("Letter Task"))
                {
                    buttons.Add(Instantiate(LetterTaskButton));
                    buttons[buttons.Count - 1].transform.SetParent(transform);
                    buttons[buttons.Count - 1].transform.localScale = Vector2.one;
                    buttons[buttons.Count - 1].GetComponent<TaskButton>().task = task;
                    buttons[buttons.Count - 1].GetComponent<TaskButton>().UpdateCard();
                }
            }
        }
        this.gameObject.GetComponent<SizeToScroll>().Refit();
    }

    public void UnloadTasks()
    {
        int childs = transform.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
    }
}
