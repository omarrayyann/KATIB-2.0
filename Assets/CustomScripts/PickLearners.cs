using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class PickLearners : MonoBehaviour
{

    [SerializeField]
    public GameObject learner;

    public List<Learner> learners = new List<Learner>();

    public List<string> learnersToAdd = new List<string>();

    public List<GameObject> loadedLearners = new List<GameObject>();

    private int numberOfLearners = 0;

    public void loadLearners()
    {
        learnersToAdd = new List<string>();
        for (int i = 0; i < loadedLearners.Count; i++)
        {
            Destroy(loadedLearners[i]);
        }
        loadedLearners = new List<GameObject>();

        learners = Server.GetLearners(Data.currentInstructor.learnersUIDs);
        int amount = learners.Count;
        numberOfLearners = amount;
        for (int i = 0; i < amount; i++)
        {
            GameObject myNewLearner = Instantiate(learner, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            myNewLearner.transform.SetParent(gameObject.transform);
            myNewLearner.transform.localScale = new Vector3(1, 1, 1);


            Button button = myNewLearner.GetComponentInChildren<Button>();
            Learner learnerNow = learners[i];
            button.onClick.AddListener(() => ClickedCard(learnerNow.uid, button.gameObject.transform.parent.gameObject));
            myNewLearner.GetComponentsInChildren<TextMeshProUGUI>()[0].text = learners[i].getName();
            myNewLearner.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Last active: " + learnerNow.LastActive();
            loadedLearners.Add(myNewLearner);
        }

        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(100, Mathf.Ceil((float)(amount + 1) / 4f) * 212);
    }

    public void ClickedCard(string id, GameObject card)
    {
        Image[] icons = card.GetComponentsInChildren<Image>();
        if (learnersToAdd.Contains(id))
        {
            learnersToAdd.Remove(id);
            icons[icons.Length - 1].transform.localScale = Vector3.zero;
        }
        else
        {
            learnersToAdd.Add(id);
            icons[icons.Length - 1].transform.localScale = Vector3.one;
        }            
    }

    // Start is called before the first frame update
    public void OnEnable()
    {
        loadLearners();
    }

    public void ClearSelection()
    {
        List<Learner> learners = new List<Learner>();
        for (int i = 0; i < loadedLearners.Count; i++)
        {
            Destroy(loadedLearners[i]);
        }
        loadedLearners = new List<GameObject>();
        List<string> learnersToAdd = new List<string>();
    }
}
