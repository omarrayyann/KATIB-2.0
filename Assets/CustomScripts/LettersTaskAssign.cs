using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LettersTaskAssign : MonoBehaviour
{

    [SerializeField]
    public Toggle noHapticToggle;
    [SerializeField]
    public Toggle partialHapticToggle;
    [SerializeField]
    public Toggle fullHapticToggle;
    [SerializeField]
    public Toggle timedToggle;
    [SerializeField]
    public TMP_InputField timeField;
    [SerializeField]
    public Toggle showToggle;



    int hapticLevel = 0;
    bool timed = false;
    int time = 0;
    int difficulty = 1;


    // Start is called before the first frame update
    void Start()
    {
        if (hapticLevel == 0)
        {
            noHapticToggle.isOn = true;
        }
        else if (hapticLevel == 1)
        {
            partialHapticToggle.isOn = true;
        }
        else if (hapticLevel == 2)
        {
            fullHapticToggle.isOn = true;
        }


        timeField.GetComponent<TMP_InputField>().enabled = false;
        // quantityField.GetComponent<TMP_InputField>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        timed = timedToggle.isOn;

        if (timed)
        {
            timeField.GetComponent<TMP_InputField>().enabled = true;
        }
        else
        {
            timeField.text = "";
            timeField.GetComponent<TMP_InputField>().enabled = false;
        }
    }

    public void toggledHaptic()
    {
        if (partialHapticToggle.isOn == true)
        {
            hapticLevel = 1;
        }
        else if (noHapticToggle.isOn == true)
        {
            hapticLevel = 0;
        }
        else if (fullHapticToggle.isOn == true)
        {
            hapticLevel = 2;
        }
    }


    public void AssignTask()
    {
        if ((timed && timeField.text == ""))
            Debug.LogError("Missing Info");
        else
        {
            for(int i = 0; i<Data.selectedLetters.Count; i++)
            {
                Task task = new Task("Letter Task");
                time = timed ? int.Parse(timeField.text) : 0;
                Debug.Log(time);
                task.LetterTask(System.DateTime.Now, timed, time, hapticLevel, Data.selectedLetters[i], showToggle.isOn);
                task.AddTask();
                Server.AddTask(task.taskID);
            }

        }
    }

    public void AssignClassTask()
    {
        if ((timed && timeField.text == ""))
            Debug.LogError("Missing Info");
        else
        {
            for (int i = 0; i < Data.selectedLetters.Count; i++)
            {
                Task task = new Task("Letter Task");
                time = timed ? int.Parse(timeField.text) : 0;
                Debug.Log(time);
                task.LetterTask(System.DateTime.Now, timed, time, hapticLevel, Data.selectedLetters[i], showToggle.isOn);
                task.isClassTask = true;
                task.AddClassTask(Data.currentClass);
            }

        }
    }

    // public void AssignTaskToClass()
    // {
    //     if ((timed && timeField.text == ""))
    //         Debug.LogError("Missing Info");
    //     else
    //     {
    //         Task task = new Task("Maze");
    //         time = timed ? int.Parse(timeField.text) : 0;
    //         difficulty = (int)difficultySlider.value;
    //         task.MazeTask(System.DateTime.Now, timed, time, hapticLevel, difficulty, (int)DateTime.Now.Ticks, showToggle.isOn);
    //         task.isClassTask = true;
    //         task.AddClassTask(Data.currentClass);
    //         Data.lastActiveScene[SceneManager.GetActiveScene().name].Push("AssignedTaskSuccessfully");
    //     }
    // }
}
