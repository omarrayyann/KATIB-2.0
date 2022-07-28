using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LearnerView : MonoBehaviour
{
    [SerializeField]
    private AssignedTasks assigned = null;


    private void OnEnable()
    {
        if (Data.currentLearner != null)
        {
            assigned.LoadTasks();
            UpdateSmartText();
        }
    }

    void UpdateSmartText(){
        SmartText smart = gameObject.GetComponentInParent<SmartText>();
        smart.AddDisctionaryEntry("<Name>", Data.currentLearner.getName());
        smart.AddDisctionaryEntry("<FirstName>", Data.currentLearner.firstName);
        smart.AddDisctionaryEntry("<LastActive>", Data.currentLearner.LastActive());
        smart.UpdateSmartTexts();
    }
}
