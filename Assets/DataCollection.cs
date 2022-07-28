using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class DataCollection : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> papers = new List<GameObject>();
    public Dictionary<string, GameObject> linkedPapers = new Dictionary<string, GameObject>();
    [SerializeField]
    private TMP_Text letterText = null;
    [SerializeField]
    private Draw draw = null;
    private string basePath = Application.dataPath;
    private string lettersFile = "Letters.csv";
    private string[] letterData;
    private List<Letter> letters = new List<Letter>();
    private int currentIndex = 0;
    private Letter currentLetter = null;

    private void Start()
    {
        LinkPapers();
        ExtractLetters();
        MoveToNext();
    }

    private void Update()
    {
        if (AndroidRuntimePermissions.CheckPermission("android.permission.WRITE_EXTERNAL_STORAGE").Equals(AndroidRuntimePermissions.Permission.ShouldAsk))
        {
            AndroidRuntimePermissions.Permission result = AndroidRuntimePermissions.RequestPermission("android.permission.WRITE_EXTERNAL_STORAGE");
            if (result == AndroidRuntimePermissions.Permission.Granted)
                Debug.Log("We have permission to access external storage!");
            else
                Debug.Log("Permission state: " + result);
        }
        else if (AndroidRuntimePermissions.CheckPermission("android.permission.WRITE_EXTERNAL_STORAGE").Equals(AndroidRuntimePermissions.Permission.Denied))
        {
            AndroidRuntimePermissions.OpenSettings();
        }
        if (AndroidRuntimePermissions.CheckPermission("android.permission.READ_EXTERNAL_STORAGE").Equals(AndroidRuntimePermissions.Permission.ShouldAsk))
        {
            AndroidRuntimePermissions.Permission result = AndroidRuntimePermissions.RequestPermission("android.permission.READ_EXTERNAL_STORAGE");
            if (result == AndroidRuntimePermissions.Permission.Granted)
                Debug.Log("We have permission to access external storage!");
            else
                Debug.Log("Permission state: " + result);
        }
        else if (AndroidRuntimePermissions.CheckPermission("android.permission.READ_EXTERNAL_STORAGE").Equals(AndroidRuntimePermissions.Permission.Denied))
        {
            AndroidRuntimePermissions.OpenSettings();
        }
    }

    private void LinkPapers()
    {
        foreach (GameObject paper in papers)
        {
            linkedPapers.Add(paper.name, paper);
        }
    }

    private void ExtractLetters()
    {
        letterData = File.ReadAllLines(basePath + '/' + lettersFile);
        for (int j = 1; j < letterData.Length; j++)
        {
            string[] currentRow = letterData[j].Split(',');
            while (currentRow[1].Equals("Custom") && j < letterData.Length)
            {
                currentRow = letterData[j].Split(',');
                j++;
            } 
            if (currentRow[1].Equals("Custom"))
                Debug.Log("End Of File");
            else
            {
                for (int i = 0; i < int.Parse(currentRow[3]); i++)
                {
                    string name = currentRow[0];
                    if (i != 0)
                    {
                        name += i;
                    }
                    letters.Add(new Letter(name, currentRow[1], currentRow[2]));
                }
            }
        }
        Debug.Log("Done extracting");
    }

    private void LoadNextLetter()
    {
        if (currentIndex < letterData.Length)
        {
            currentLetter = letters[currentIndex];
            currentIndex++;
        }
            
    }

    private void UpdateWorkSpace()
    {
        foreach (KeyValuePair<string, GameObject> paper in linkedPapers)
        {
            if (currentLetter.language.Equals(paper.Key))
            {
                paper.Value.SetActive(true);
            }
            else
            {
                paper.Value.SetActive(false);
            }
        }
        letterText.text = currentLetter.name + ": " + currentLetter.type;
    }

    public void MoveToNext()
    {
        if (currentIndex < letterData.Length)
        {
            LoadNextLetter();
            UpdateWorkSpace();
        }
    }

    public void Save()
    {
        List<Vector3> points = draw.drawnPoints;
        string filePath = basePath + "/Letters/" + currentLetter.language + "/" + currentLetter.type + "/Data";
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        filePath += '/' + currentLetter.name + ".csv";

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
        MoveToNext();
    }
}
