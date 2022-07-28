using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class LearnersView : MonoBehaviour
{

    [SerializeField]
    public GameObject learner;

    [SerializeField]
    public GameObject add;

    public List<Learner> learners = new List<Learner>();

    public List<GameObject> loadedLearners = new List<GameObject>();

    private int numberOfLearners = 0;
    [SerializeField]
    private Menu learnerView;
    [SerializeField]
    private Menu addLearnerMenu;


    public void loadLearners(){

        for(int i = 0; i<loadedLearners.Count; i++){
             Destroy (loadedLearners[i]);
        }
         loadedLearners =  new List<GameObject>();

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
            button.onClick.AddListener(() => openLearner(learnerNow));
            myNewLearner.GetComponentsInChildren<TextMeshProUGUI>()[0].text = learners[i].getName();
            myNewLearner.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Last active: " + learnerNow.LastActive();
            loadedLearners.Add(myNewLearner);
         }

         GameObject addButton = Instantiate(add, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
         addButton.transform.SetParent(gameObject.transform);
         addButton.transform.localScale = new Vector3(1, 1, 1);
        Menu menu = GetComponentInParent<Menu>();
        addButton.GetComponentInChildren<Button>().onClick.AddListener(() => menu.loadAnother(addLearnerMenu));
       

         loadedLearners.Add(addButton);

         RectTransform rt = GetComponent<RectTransform>();
         rt.sizeDelta = new Vector2(100, Mathf.Ceil((float)(amount + 1) / 4f) * 212);
    }

    public void openLearner(Learner learner)
    {
        Data.currentLearner = learner;
        gameObject.GetComponentInParent<Menu>().moveTo(learnerView);
    }

    public void ButtonClicked(string id)
    {
        
        Debug.Log("Button clicked = " + id);
    }

    // Start is called before the first frame update
    public void Start()
    {
        loadLearners();
    }
}
