using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Linq;

public class Task
{
    public string taskID { get; set; }

    // Task Type:-
    // Collect: Drag items
    // Maze: Follow Path
    // Letters: Trace Letter
    // Word: Trace Word
    public String taskType { get; set; }

    // Completed Task: False/True
    public bool completed { get; set; }

    // Timed Task: False/True
    public bool timed { get; set; }

     // Letter: 
    public Letter letter { get; set; }

    //Time in Seconds if timed
    public int time { get; set; }

    // Haptic Level 0: Non
    // Haptic Level 1: Partial
    // Haptic Level 2: Full
    public int hapticLevel { get; set; }

    // Collect Task: Number of items to collect
    // Maze Task: Number of mazes
    public int quantity { get; set; }

    // Paths Type 1: Straight Lines
    // Paths Type 2: Parabola Lines
    // Paths Type 3: Random Lines
    public int pathsType { get; set; }

    public int difficulty { get; set; }

    // Assigning Time: Time of assigning the task to the learner
    public DateTime assigningTime { get; set; }

    public bool showSolution { get; set; }

    public bool showPath { get; set; }

    public bool isClassTask = false;

    public int seed = -1;

    public List<string> duplicateTasks = new List<string>();
    public Task(String taskType)
    {
        this.taskType = taskType;
        taskID = System.Guid.NewGuid().ToString();
        completed = false;
    }
    public Task(String taskType, string ID)
    {
        this.taskType = taskType;
        taskID = ID;
        completed = false;
    }

    public Task(String taskType, string ID, bool complete)
    {
        this.taskType = taskType;
        taskID = ID;
        completed = complete;
    }


    public Task(Task taskToClone)
    {
        taskID = System.Guid.NewGuid().ToString();
        taskType = taskToClone.taskType;
        isClassTask = taskToClone.isClassTask;
        if (taskType.Equals("Collecting Task"))
        {
            completed = taskToClone.completed;
            hapticLevel = taskToClone.hapticLevel;
            timed = taskToClone.timed;
            time = taskToClone.time;
            assigningTime = taskToClone.assigningTime;
            quantity = taskToClone.quantity;
            pathsType = taskToClone.pathsType;
        }
        else if (taskType.Equals("Maze Task"))
        {
            completed = taskToClone.completed;
            hapticLevel = taskToClone.hapticLevel;
            timed = taskToClone.timed;
            time = taskToClone.time;
            assigningTime = taskToClone.assigningTime;
            difficulty = taskToClone.difficulty;
            seed = taskToClone.seed;
            showSolution = taskToClone.showSolution;
        }
        else if (taskType.Equals("Letter Task"))
        {
            completed = taskToClone.completed;
            hapticLevel = taskToClone.hapticLevel;
            timed = taskToClone.timed;
            time = taskToClone.time;
            assigningTime = taskToClone.assigningTime;
            letter = taskToClone.letter;
            showPath = taskToClone.showPath;
        }
    }

    // Called to fill the collect task required options
    public void collectTask(DateTime assigningTime, bool timed, int time, int hapticLevel, int quantity, int pathsType)
    {
        //taskType = "Collecting Task";
        this.hapticLevel = hapticLevel;
        this.timed = timed;
        this.time = time;
        this.assigningTime = assigningTime;
        this.quantity = quantity;
        this.pathsType = pathsType;
    }

    public void MazeTask(DateTime assigningTime, bool timed, int time, int hapticLevel, int difficulty, int seed, bool showSolution)
    {
        //taskType = "Maze Task";
        this.hapticLevel = hapticLevel;
        this.timed = timed;
        this.time = time;
        this.assigningTime = assigningTime;
        this.difficulty = difficulty;
        this.seed = seed;
        this.showSolution = showSolution;
    }

    public void LetterTask(DateTime assigningTime, bool timed, int time, int hapticLevel, Letter letter, bool showPath)
    {
        //taskType = "Letter Task";
        this.assigningTime = assigningTime;
        this.letter = letter;
        this.timed = timed;
        this.time = time;
        this.hapticLevel = hapticLevel;
        this.showPath = showPath;
    }

    public void CustomTask(int hapticLevel, Vector2[] points, bool timed, int time, DateTime assigningTask)
    {
        //taskType = "Custom Task";
        taskID = System.Guid.NewGuid().ToString();
        completed = false;
        this.hapticLevel = hapticLevel;
        this.timed = timed;
        this.time = time;
        this.assigningTime = assigningTime;
    }

    public void WordTask(int hapticLevel, string word, bool timed, int time, DateTime assigningTime)
    {
        //taskType = "Word Task";
        taskID = System.Guid.NewGuid().ToString();
        completed = false;
        this.hapticLevel = hapticLevel;
        this.timed = timed;
        this.time = time;
        this.assigningTime = assigningTime;
        foreach (char letter in word)
        {

        }
    }

    private string pathOf(char letter)
    {
        return "";
    }

    public void AddTask()
    {
        /*if (!Directory.Exists(Application.dataPath + "/Server/Tasks.csv"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Server/Tasks.csv");
        }*/
        string filePath = Application.dataPath + "/Server/Tasks.csv";
        string toAdd = StringToAdd();
        List<string> toAdds = new List<string>();
        toAdds.Add(toAdd);
        File.AppendAllLines(filePath, toAdds);
    }
    
    public string StringToAdd()
    {
        string toAdd = completed + "," + taskID + "," + taskType + "," + assigningTime + ",";
        toAdd += time + ",";
        toAdd += hapticLevel + ",";
        if (taskType.Equals("Collecting Task"))
        {
            toAdd += quantity + "," + pathsType + ",";
        }
        else if (taskType.Equals("Maze Task"))
        {
            toAdd += difficulty + "," + seed + "," + showSolution + ",";
        }
        else if (taskType.Equals("Letter Task"))
        {
            toAdd += letter.name + "," + letter.language + "," + letter.type + "," + showPath + ",";
        }
        toAdd += isClassTask + ",";
        // toAdd += isClassTask? Data.currentClass.classID + ",": "";
        return toAdd;
    }

    public void AddClassTask(Class classToAddTo)
    {
        isClassTask = true;
        foreach (string learner in classToAddTo.learnerIDs)
        {
            Task newTask = new Task(this);
            newTask.AddTask();
            Server.AddTask(newTask.taskID, learner);
            duplicateTasks.Add(newTask.taskID);
        }/*
        if (!Directory.
        (Application.dataPath + "/Server/Tasks.csv"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Server/Tasks.csv");
        }*/
        string filePath = Application.dataPath + "/Server/Tasks.csv";
        string toAdd = StringToAdd();
        toAdd += Data.currentClass.classID + ',';
        foreach (string duplicate in duplicateTasks)
        {
            toAdd += duplicate + " ";
        }
        toAdd += ",";
        List<string> strings = new List<string>();
        strings.Add(toAdd);
        File.AppendAllLines(filePath, strings);
    }

    public override string ToString()
    {
        return taskID + ": " + taskType + ", " + timed + ", " + time + ", " + hapticLevel + ", " + seed;
    }

    public string AssigningTime()
    {
        const int SECOND = 1;
        const int MINUTE = 60 * SECOND;
        const int HOUR = 60 * MINUTE;
        const int DAY = 24 * HOUR;
        const int MONTH = 30 * DAY;
        var ts = new TimeSpan(DateTime.Now.Ticks - assigningTime.Ticks);
        double delta = Math.Abs(ts.TotalSeconds);

        if (delta < 1 * MINUTE)
            return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

        if (delta < 2 * MINUTE)
            return "a minute ago";

        if (delta < 45 * MINUTE)
            return ts.Minutes + " minutes ago";

        if (delta < 90 * MINUTE)
            return "an hour ago";

        if (delta < 24 * HOUR)
            return ts.Hours + " hours ago";

        if (delta < 48 * HOUR)
            return "yesterday";

        if (delta < 30 * DAY)
            return ts.Days + " days ago";

        if (delta < 12 * MONTH)
        {
            int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
            return months <= 1 ? "one month ago" : months + " months ago";
        }
        else
        {
            int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "one year ago" : years + " years ago";
        }
    }

    public bool DeleteTask()
    {
        /*if (!Directory.Exists(Application.dataPath + "/Server/Tasks.csv"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Server/Tasks.csv");
        }*/
        List<string> learners = Server.ReadAllStringsFrom(SpreadSheets.Learners).ToList<string>();
        for (int i = 0; i < learners.Count; i++)
        {
            if (learners[i].Contains(taskID))
            {
                int index = learners[i].IndexOf(taskID);
                learners[i].Remove(index, taskID.Length + 1);
                break;
            }
        }
        List<string> associatedTasks = new List<string>();
        List<string> tasks = File.ReadAllLines(Application.dataPath + "/Server/Tasks.csv").ToList<string>();
        if (duplicateTasks.Count != 0)
        {
            if (!SceneManager.GetActiveScene().name.Equals("Classes"))
            {
                Debug.LogError("Class Tasks can only be deleted from the Classes tab");
                return false;
            }
            for (int i = 0; i < tasks.Count; i++)
            {
                foreach (string duplicate in duplicateTasks)
                {
                    if (tasks[i].Contains(duplicate))
                        tasks.RemoveAt(i);
                }
            }
            for (int i = 0; i < learners.Count; i++)
            {
                foreach (string duplicate in duplicateTasks)
                {
                    if (learners[i].Contains(duplicate))
                    {
                        int index = learners[i].IndexOf(duplicate);
                        learners[i].Remove(index, duplicate.Length + 1);
                    }
                }
            }
        }
        for (int i = 0; i < tasks.Count; i++)
        {
            if (tasks[i].Contains(taskID))
            {
                tasks.RemoveAt(i);
                break;
            }
        }
        File.WriteAllLines(Application.dataPath + "/Server/Tasks.csv", tasks);
        File.WriteAllLines(Application.dataPath + "/Server/Learners.csv", learners);
        /*if (!Directory.Exists(Application.dataPath + "/Server/Learners.csv"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Server/Learners.csv");
        }*/
        return true;
    }
}
