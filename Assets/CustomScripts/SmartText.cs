using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SmartText : MonoBehaviour
{
    private Dictionary<string, string> smartReplacement = new Dictionary<string, string>();
    private float bigDelta = 0;
    private float waitTime = 30f;
    public bool maintainMemory = false;

    public void AddDisctionaryEntry(string oldString, string newString)
    {
        if (newString.Trim().Equals("") || oldString.Trim().Equals(""))
            return;
        newString = " " + newString + " ";
        if (smartReplacement.ContainsKey(oldString))
        {
            Debug.LogWarning("The key " + oldString + " already exists. Update method invoked instead");
            UpdateDictionaryEntry(oldString, newString);
        }
        else 
            smartReplacement.Add(oldString, newString);
    }

    public void RemoveDictionaryEntry(string key)
    {
        string value = smartReplacement.GetValueOrDefault<string, string>(key);
        smartReplacement.Add(value , key);
        smartReplacement.Remove(key);
        UpdateSmartTexts();
        smartReplacement.Remove(value);
    }

    public void RevertTexts()
    {
        for (int i = smartReplacement.Count - 1; i >= 0; i--)
        {
            KeyValuePair<string, string> entry = smartReplacement.ElementAt(i);
            RemoveDictionaryEntry(entry.Key);
        }
    }

    public void UpdateDictionaryEntry(string key, string newString)
    {
        smartReplacement.Remove(key);
        smartReplacement.Add(key, newString);
        string some = "";
        smartReplacement.TryGetValue(key, out some);
        // Debug.Log(some);
    }

    public void UpdateSmartTexts()
    {
        // Debug.Log(smartReplacement.Count);
        TMP_Text[] TMP_Texts = gameObject.GetComponentsInChildren<TMP_Text>();
        for (int i = 0; i < TMP_Texts.Length; i++)
        {
            foreach (KeyValuePair<string, string> entry in smartReplacement)
            {
                TMP_Texts[i].text = TMP_Texts[i].text.Replace(entry.Key, entry.Value);
            }
        }
        Text[] texts = gameObject.GetComponentsInChildren<Text>();
        for (int i = 0; i < texts.Length; i++)
        {
            foreach (KeyValuePair<string, string> entry in smartReplacement)
            {
                texts[i].text = texts[i].text.Replace(entry.Key, entry.Value);
            }
        }
    }

    private void OnDisable()
    {

    }

    private void OnEnable()
    {
        UpdateSmartTexts();
    }

    private void Update()
    {
        bigDelta += Time.deltaTime;
        if (timePassed(waitTime)) // If all necessary prerequisites are fulfilled
        {
            UpdateSmartTexts();
            bigDelta = 0;
        }
    }

    private bool timePassed(float minTime)
    {
        return bigDelta >= minTime;
    }
}
