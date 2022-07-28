using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClassView : MonoBehaviour
{

    public static Class Class = new Class("", "", System.DateTime.Now, "", new List<string>());
    [SerializeField]
    private AssignedTasks assigned = null;


    private void OnEnable()
    {
        if (Data.currentClass != null)
        {
            assigned.LoadClassesTasks();
            LoadClasses();
        }
    }

    void LoadClasses()
    {
        SmartText smart = gameObject.GetComponent<SmartText>();
        smart.AddDisctionaryEntry("<ClassName>", Data.currentClass.className);
        smart.AddDisctionaryEntry("<MemberCount>", Data.currentClass.learnerIDs.Count.ToString());
        smart.UpdateSmartTexts();
    }
}
