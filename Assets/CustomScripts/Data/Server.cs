using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using System.Text;

public enum SpreadSheets
{
    [Description("/Server/Instructors.csv")]
    Instructors,
    [Description("/Server/Learners.csv")]
    Learners,
    [Description("/Server/Classes.csv")]
    Classes,
    [Description("/Server/Tasks.csv")]
    Tasks,

}


public struct Range
{
    public int start;
    public int end;
}

public static class Server
{
    private static string Root = Application.dataPath;

    public static string DescriptionAttr<T>(this T source)
    {
        FieldInfo fi = source.GetType().GetField(source.ToString());

        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
            typeof(DescriptionAttribute), false);

        if (attributes != null && attributes.Length > 0) return attributes[0].Description;
        else return source.ToString();
    }

    // Generic Functions:
    // The functions below can be used regardless of what specific spreadsheet is necessary

    /// <summary>
    /// Reads all strings from the specified spreadsheet
    /// </summary>
    /// <param name="path">The spreadsheet to read from</param>
    /// <returns>String array of all the lines in the spreadsheet</returns>
    public static string[] ReadAllStringsFrom(SpreadSheets path)
    {
        /*if (!Directory.Exists(Root + DescriptionAttr<SpreadSheets>(SpreadSheets.Learners)))
        {
            Directory.CreateDirectory(Root + DescriptionAttr<SpreadSheets>(SpreadSheets.Learners));
        }*/
        string[] output = File.ReadAllLines(Root + DescriptionAttr<SpreadSheets>(path));

        return output;
    }

    public static string[] ReadAllStringsFrom(string path)
    {
        /*if (!Directory.Exists(Root + DescriptionAttr<SpreadSheets>(SpreadSheets.Learners)))
        {
            Directory.CreateDirectory(Root + DescriptionAttr<SpreadSheets>(SpreadSheets.Learners));
        }*/
        string[] output = File.ReadAllLines(Root + path);

        return output;
    }

    /// <summary>
    /// Reads strings in the specified range from the specified spreadsheet
    /// </summary>
    /// <param name="path">The spreadsheet to read from</param>
    /// <param name="range">Range to read from</param>
    /// <returns>String array of the lines in the specified range in the spreadsheet</returns>
    public static string[] ReadCertainStringsFrom(SpreadSheets path, Range range)
    {
        /*if (!Directory.Exists(Root + DescriptionAttr<SpreadSheets>(SpreadSheets.Learners)))
        {
            Directory.CreateDirectory(Root + DescriptionAttr<SpreadSheets>(SpreadSheets.Learners));
        }*/
        string[] output = File.ReadAllLines(Root + DescriptionAttr<SpreadSheets>(path));
        List<string> listOutput = output.ToList<string>();
        listOutput.RemoveRange(range.end, listOutput.Count - 1);
        listOutput.RemoveRange(0, range.start);
        return listOutput.ToArray();
    }

    public static string[] ReadSplitLine(SpreadSheets path, int index)
    {
        /*if (!Directory.Exists(Root + DescriptionAttr<SpreadSheets>(SpreadSheets.Learners)))
        {
            Directory.CreateDirectory(Root + DescriptionAttr<SpreadSheets>(SpreadSheets.Learners));
        }*/
        string[] output = File.ReadAllLines(Root + DescriptionAttr<SpreadSheets>(path));
        return output[index].Split(',');
    }


    /// <summary>
    /// Checks whether the user login information matches with an entry in the specified spreadsheet
    /// </summary>
    /// <param name="path">The spreadsheet to look in</param>
    /// <param name="usernameOrEmail">The input username or email</param>
    /// <param name="password">The input password</param>
    /// <returns>Whether the information is valid or not</returns>
    public static bool Authenticate(SpreadSheets path, string usernameOrEmail, string password)
    {
        string[] lines = ReadAllStringsFrom(path);
        for (int i = 0; i < lines.Length; i++)
        {
            switch (path)
            {
                case SpreadSheets.Instructors:
                    {
                        string[] entry = lines[i].Split(',');
                        if ((entry[1].Equals(usernameOrEmail) || entry[2].Equals(usernameOrEmail)) && entry[3].Equals(password))
                        {
                            Data.currentInstructor = GetInstructor(i);
                            return true;
                        }
                    }
                    break;
                case SpreadSheets.Learners:
                    {
                        string[] entry = lines[i].Split(',');
                        if ((entry[3].Equals(usernameOrEmail) || entry[4].Equals(usernameOrEmail)) && entry[5].Equals(password))
                        {
                            Data.currentLearner = GetLearner(i);
                            return true;
                        }
                    }
                    break;
                default:
                    Debug.LogError("Authentication services not available for specified spreadsheet");
                    return false;
            }
        }
        return false;
    }

    // Instructor-Specific Functions:

    public static Instructor GetInstructor(int index)
    {
        string[] instructorsData = ReadSplitLine(SpreadSheets.Instructors, index);
        List<string> learnersUids = new List<string>();
        if (!instructorsData[6].Trim().Equals(""))
            learnersUids = instructorsData[6].Trim().Split(' ').ToList<string>();
        Instructor instructor = new Instructor(instructorsData[0], instructorsData[1], instructorsData[2], instructorsData[3], instructorsData[4], instructorsData[5], learnersUids);
        return instructor;
    }


    // Learner-Specific Functions:

    /// <summary>
	/// Transforms a list of learner IDs into Learner objects with proper parameter values
	/// </summary>
	/// <returns>List of Learners associated with the current instructor in the same order</returns>
    public static List<Learner> GetLearners()
    {
        /*if (!Directory.Exists(Root + DescriptionAttr<SpreadSheets>(SpreadSheets.Learners)))
        {
            Directory.CreateDirectory(Root + DescriptionAttr<SpreadSheets>(SpreadSheets.Learners));
        }*/
        string[] filelines = File.ReadAllLines(Root + DescriptionAttr<SpreadSheets>(SpreadSheets.Learners));
        Data.learners.Clear();
        for (int i = 0; i < filelines.Length; i++)
        {
            string[] learnersData = filelines[i].Split(',');
            if (learnersData[0].Equals(""))
                continue;
            if (Data.currentInstructor.learnersUIDs.Contains(learnersData[0]))
            {
                List<string> tasks = new List<string>();
                if (!learnersData[8].Trim().Equals(""))
                {
                    tasks = learnersData[8].Trim().Split(' ').ToList<string>();
                }
                Learner learner = new Learner(learnersData[0], learnersData[1], learnersData[2], learnersData[3], learnersData[4], learnersData[5], DateTime.Parse(learnersData[6]), learnersData[7], tasks);
                Data.learners.Add(learner);
            }
        }
        return Data.learners;
    }

    /// <summary>
	/// Transforms a list of learner IDs into Learner objects with proper parameter values
	/// </summary>
	/// <param name="instructorsLearners">List of learner IDs</param>
	/// <returns>List of Learners associated with the given IDs in the same order</returns>
    public static List<Learner> GetLearners(List<string> instructorsLearners)
    {
        /*if (!Directory.Exists(Root + DescriptionAttr<SpreadSheets>(SpreadSheets.Learners)))
        {
            Directory.CreateDirectory(Root + DescriptionAttr<SpreadSheets>(SpreadSheets.Learners));
        }*/
        string[] filelines = File.ReadAllLines(Root + DescriptionAttr<SpreadSheets>(SpreadSheets.Learners));
        Data.learners.Clear();
        for (int i = 0; i < filelines.Length; i++)
        {
            string[] learnersData = filelines[i].Split(',');
            if (learnersData[0].Equals(""))
                continue;
            if (instructorsLearners.Contains(learnersData[0]))
            {
                List<string> tasks = new List<string>();
                if (!learnersData[8].Trim().Equals(""))
                {
                    tasks = learnersData[8].Trim().Split(' ').ToList<string>();
                }
                Learner learner = new Learner(learnersData[0], learnersData[1], learnersData[2], learnersData[3], learnersData[4], learnersData[5], DateTime.Parse(learnersData[6]), learnersData[7], tasks);
                Data.learners.Add(learner);
            }
        }
        return Data.learners;
    }

    public static Learner GetLearner(int index)
    {
        string[] learnersData = ReadSplitLine(SpreadSheets.Learners, index);
        List<string> tasks = new List<string>();
        if (!learnersData[8].Trim().Equals(""))
        {
            tasks = learnersData[8].Trim().Split(' ').ToList<string>();
        }
        Learner learner = new Learner(learnersData[0], learnersData[1], learnersData[2], learnersData[3], learnersData[4], learnersData[5], DateTime.Parse(learnersData[6]), learnersData[7], tasks);
        return learner;
    }


    // Class-Specific Functions:

    /// <summary>
	/// Prodices a list of classes managed by the signed in instructor
	/// </summary>
	/// <returns>List of classes managed by the signed in instructor</returns>
    public static List<Class> GetClasses()
    {
        Data.classes.Clear();
        /*if (!Directory.Exists(Root + SpreadSheets.Classes))
        {
            Directory.CreateDirectory(Root + SpreadSheets.Classes);
        }*/
        string[] filelines = Server.ReadAllStringsFrom(SpreadSheets.Classes);
        for (int i = 0; i < filelines.Length; i++)
        {
            if (filelines[i].Contains(Data.currentInstructor.uid))
            {
                string[] cells = filelines[i].Split(",");
                List<string> learnerIDs = new List<string>();
                if (!cells[4].Trim().Equals(""))
                {
                    learnerIDs = cells[4].Trim().Split(' ').ToList<string>();
                }
                Class classNow = new Class(cells[0], cells[1], DateTime.Parse(cells[2]), cells[3], learnerIDs);
                Data.classes.Add(classNow);
            }
        }
        return Data.classes;
    }

    /// <summary>
	/// Prodices a list of classes managed by the given instructor
	/// </summary>
	/// <param name="instructorID">ID of the instructor</param>
	/// <returns>List of classes managed by the given instructor</returns>
    public static List<Class> GetClasses(string instructorID)
    {
        Data.classes.Clear();
        /*if (!Directory.Exists(Root + SpreadSheets.Classes))
        {
            Directory.CreateDirectory(Root + SpreadSheets.Classes);
        }*/
        string[] filelines = Server.ReadAllStringsFrom(SpreadSheets.Classes);
        for (int i = 0; i < filelines.Length; i++)
        {
            if (filelines[i].Contains(instructorID))
            {
                string[] cells = filelines[i].Split(",");
                List<string> learnerIDs = new List<string>();
                if (!cells[4].Trim().Equals(""))
                {
                    learnerIDs = cells[4].Trim().Split(' ').ToList<string>();
                }
                Class classNow = new Class(cells[0], cells[1], DateTime.Parse(cells[2]), cells[3], learnerIDs);
                Data.classes.Add(classNow);
            }
        }
        return Data.classes;
    }

    // type "" returns all language letters regardless of type
    // lagnuage "" returns all letters
    public static List<Letter> LoadLetters(string languageNeeded, string typeNeeded)
    {
        List<Letter> letters = new List<Letter>();
        /*if (!Directory.Exists(Application.dataPath + "/Server/Letters.csv"))
		{
			Directory.CreateDirectory(Application.dataPath + "/Server/Letters.csv");
		}*/
        string[] filelines = File.ReadAllLines(Application.dataPath + "/Server/Letters.csv");


        for (int i = 0; i < filelines.Length; i++)
        {
            string[] pathData = filelines[i].Split(',');
            string name = pathData[0];
            string language = pathData[1];
            string type = pathData[2];

            List<Vector2> letterPath = new List<Vector2>();

            if (languageNeeded == "" || (typeNeeded == "" && languageNeeded == language) || (languageNeeded == language && typeNeeded == type))
            {
                string[] letterLines = File.ReadAllLines(Application.dataPath + "/Resources/Letters/" + language + "/" + type + "/Data/" + name + ".csv");
                // Debug.Log(letterLines);
                for (int b = 0; b < letterLines.Length && !letterLines[b].Equals(""); b++)
                {
                    string[] letterData = letterLines[b].Split(',');
                    float x = (float)double.Parse(letterData[0]);
                    float y = float.Parse(letterData[1]);
                    letterPath.Add(new Vector2(x, y));
                }
                Sprite spriteNow = Resources.Load("Letters/" + language + "/" + type + "/Image/" + name, typeof(Sprite)) as Sprite;
                letters.Add(new Letter(name, language, type, spriteNow));
            }


        }
        // Debug.Log("Returned " + letters.Count + " letter");
        return letters;
    }


    public static void AddTask(string taskID, string learnerUid)
    {
        /*if (!Directory.Exists(Application.dataPath + "/Server/Learners.csv"))
		{
			Directory.CreateDirectory(Application.dataPath + "/Server/Learners.csv");
		}*/
        string[] filelines = Server.ReadAllStringsFrom(SpreadSheets.Learners);
        for (int i = 0; i < filelines.Length; i++)
        {
            string[] learnersData = filelines[i].Split(',');
            if (learnerUid.Equals(learnersData[0]))
            {
                string[] cells = filelines[i].Split(',');
                cells[8] += taskID + " ";
                filelines[i] = "";
                foreach (string cell in cells)
                {
                    filelines[i] += cell + ",";
                }
            }
        }
        File.WriteAllLines(Application.dataPath + "/Server/Learners.csv", filelines);

        
    }

    public static void AddTask(string taskID)
    {
        /*if (!Directory.Exists(Application.dataPath + "/Server/Learners.csv"))
		{
			Directory.CreateDirectory(Application.dataPath + "/Server/Learners.csv");
		}*/
        string[] filelines = Server.ReadAllStringsFrom(SpreadSheets.Learners);
        Data.currentLearner.tasks.Add(taskID);
        for (int i = 0; i < filelines.Length; i++)
        {
            string[] learnersData = filelines[i].Split(',');
            if (Data.currentLearner.uid.Equals(learnersData[0]))
            {
                string[] cells = filelines[i].Split(',');
                cells[8] += taskID + " ";
                filelines[i] = "";
                foreach (string cell in cells)
                {
                    filelines[i] += cell + ",";
                }
            }
        }
        File.WriteAllLines(Application.dataPath + "/Server/Learners.csv", filelines);
    }

    public static void AddLetter(List<Vector3> points, string name, string language, string type){
    
        string letterUID = System.Guid.NewGuid().ToString();
        // Create new csv file

        string filePath = Application.dataPath + "/Resources/Letters/" + language + "/" + type + "/Data/" + name + ".csv";
        
        // Fill Data
        using (var stream = File.CreateText(filePath))
        {
            for (int i = 0; i < points.Count; i++)
            {
                float x = points[i].x;
                float y = points[i].y;
                float timeStamp = points[i].z;
                string csvRow = string.Format(x + "," + y + "," + timeStamp);
                if (x==-9999f && y==-9999f){
                    csvRow = string.Format("");
                }
                stream.WriteLine(csvRow);
            }
        }

        //Add it to letters csv

        filePath = Application.dataPath + "/Server/Letters.csv";

        string toAdd = name + "," + language + "," + type + "," + letterUID;
        List<string> add = new List<string>();
        add.Add(toAdd);
        File.AppendAllLines(filePath, add);
    }

    public static List<Task> LoadLearnerTasks()
    {
        /*if (!Directory.Exists(Application.dataPath + "/Server/Tasks.csv"))
		{
			Directory.
		(Application.dataPath + "/Server/Tasks.csv");
		}*/
        string[] filelines = File.ReadAllLines(Application.dataPath + "/Server/Tasks.csv");
        List<Task> tasks = new List<Task>();
        if (Data.currentLearner.tasks.Count != 0)
        {
            for (int i = 0; i < filelines.Length; i++)
            {
                string[] taskData = filelines[i].Split(',');
                if (taskData[1].Equals(""))
                    continue;
                if (Data.currentLearner.tasks.Contains(taskData[1]))
                {
                    Task task = new Task(taskData[2], taskData[1], bool.Parse(taskData[0]));
                    if (taskData[2].Equals("Collecting Task"))
                    {
                        task.collectTask(DateTime.Parse(taskData[3]), !taskData[4].Equals("0"), int.Parse(taskData[4]), int.Parse(taskData[5]), int.Parse(taskData[6]), int.Parse(taskData[7]));
                        task.isClassTask = bool.Parse(taskData[8]);
                    }
                    else if (taskData[2].Equals("Maze Task"))
                    {
                        task.MazeTask(DateTime.Parse(taskData[3]), !taskData[4].Equals("0"), int.Parse(taskData[4]), int.Parse(taskData[5]), int.Parse(taskData[6]), int.Parse(taskData[7]), bool.Parse(taskData[8]));
                        task.isClassTask = bool.Parse(taskData[9]);
                    }
                    else if (taskData[2].Equals("Letter Task"))
                    {
                        task.LetterTask(DateTime.Parse(taskData[3]), !taskData[4].Equals("0"), int.Parse(taskData[4]), int.Parse(taskData[5]), new Letter(taskData[6], taskData[7], taskData[8]), bool.Parse(taskData[9]));
                        task.isClassTask = bool.Parse(taskData[10]);
                    }
                    // elses
                    tasks.Add(task);
                }
            }
        }
        return tasks;
    }

    public static List<Task> LoadClassesTasks()
    {
        /*if (!Directory.Exists(Application.dataPath + "/Server/Tasks.csv"))
		{
			Directory.CreateDirectory(Application.dataPath + "/Server/Tasks.csv");
		}*/
        string[] filelines = Server.ReadAllStringsFrom(SpreadSheets.Tasks);
        List<Task> tasks = new List<Task>();
        for (int i = 4; i < filelines.Length; i++)
        {
            string[] taskData = filelines[i].Split(',');
            if (taskData[1].Equals(""))
                continue;
            if (filelines[i].Contains(Data.currentClass.classID))
            {
                Task task = new Task(taskData[2], taskData[1], bool.Parse(taskData[0]));
                if (taskData[2].Equals("Collecting Task"))
                {
                    task.collectTask(DateTime.Parse(taskData[3]), !taskData[4].Equals("0"), int.Parse(taskData[4]), int.Parse(taskData[5]), int.Parse(taskData[6]), int.Parse(taskData[7]));
                    task.isClassTask = bool.Parse(taskData[8]);
                    List<string> duplicates = new List<string>();
                    if (!taskData[10].Trim().Equals(""))
                        duplicates = taskData[10].Trim().Split(' ').ToList<string>();
                    task.duplicateTasks = duplicates;
                }
                else if (taskData[1].Equals("Maze Task"))
                {
                    task.MazeTask(DateTime.Parse(taskData[3]), !taskData[4].Equals("0"), int.Parse(taskData[4]), int.Parse(taskData[5]), int.Parse(taskData[6]), int.Parse(taskData[7]), bool.Parse(taskData[8]));
                    task.isClassTask = bool.Parse(taskData[9]);
                    List<string> duplicates = new List<string>();
                    if (!taskData[10].Trim().Equals(""))
                        duplicates = taskData[11].Trim().Split(' ').ToList<string>();
                    task.duplicateTasks = duplicates;
                }
                else if (taskData[1].Equals("Letter Task"))
                {
                    task.LetterTask(DateTime.Parse(taskData[3]), !taskData[4].Equals("0"), int.Parse(taskData[4]), int.Parse(taskData[5]), new Letter(taskData[6], taskData[7], taskData[8]), bool.Parse(taskData[9]));
                    task.isClassTask = bool.Parse(taskData[10]);
                    List<string> duplicates = new List<string>();
                    if (!taskData[10].Trim().Equals(""))
                        duplicates = taskData[12].Trim().Split(' ').ToList<string>();
                    task.duplicateTasks = duplicates;
                }
                // elses
                tasks.Add(task);
            }
        }
        return tasks;
    }
    
    public static void SavePerformance(List<Vector3> points, string taskID)
    {
        string letterUID = System.Guid.NewGuid().ToString();
        // Create new csv file

        string filePath = Application.dataPath + "/Server/Performance/" + taskID + ".csv";

        // Fill Data
        using (var stream = File.CreateText(filePath))
        {
            for (int i = 0; i < points.Count; i++)
            {
                float x = points[i].x;
                float y = points[i].y;
                float timeStamp = points[i].z;
                string csvRow = string.Format(x + "," + y + "," + timeStamp);
                if (x == -9999f && y == -9999f)
                {
                    csvRow = string.Format("");
                }
                stream.WriteLine(csvRow);
            }
        }

        //Add it to letters csv

        string[] filelines = ReadAllStringsFrom(SpreadSheets.Tasks);
        for (int i = 0; i < filelines.Length; i++)
        {
            if (filelines[i].Contains(taskID))
            {
                string[] cells = filelines[i].Split(',');
                cells[0] = true.ToString();
                filelines[i] = "";
                foreach (string cell in cells)
                {
                    filelines[i] += cell + ",";
                }
                break;
            }
        }
        File.WriteAllLines(Root + DescriptionAttr<SpreadSheets>(SpreadSheets.Tasks), filelines);
    }

}
