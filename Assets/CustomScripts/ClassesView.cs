using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class ClassesView : MonoBehaviour
{
    [SerializeField]
    public GameObject classPrefab;

    [SerializeField]
    public GameObject add;

    public List<Class> classes = new List<Class>();

    public List<GameObject> loadedClasses = new List<GameObject>();

    private int numberOfClasses = 0;

    [SerializeField]
    private Menu pickLearnersMenu;
    [SerializeField]
    private Menu classView;

    public void LoadClasses()
    {
        UnloadClasses();

        classes = Server.GetClasses(Data.currentInstructor.uid);
        int amount = classes.Count;
        numberOfClasses = amount;
        for (int i = 0; i < amount; i++)
        {
            GameObject myNewClass = Instantiate(classPrefab);
            myNewClass.transform.SetParent(gameObject.transform);
            myNewClass.transform.localScale = new Vector3(1, 1, 1);
            Button button = myNewClass.GetComponentInChildren<Button>();
            Class classNow = classes[i];
            button.onClick.AddListener(() => openClass(classNow));
            TMP_Text[] texts = myNewClass.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = classes[i].className;
            texts[1].text = classes[i].learnerIDs.Count.ToString() + " members";
            texts[2].text = "Created: " + classNow.DateCreated();
            loadedClasses.Add(myNewClass);
        }
        GameObject addButton = Instantiate(add);
        Menu menu = GetComponentInParent<Menu>();
        addButton.GetComponentInChildren<Button>().onClick.AddListener(() => menu.moveTo(pickLearnersMenu));
        addButton.transform.SetParent(gameObject.transform);
        addButton.transform.localScale = new Vector3(1, 1, 1);

        loadedClasses.Add(addButton);

        RectTransform rt = GetComponent<RectTransform>();
    }

    public void openClass(Class classToOpen)
    {
        Data.currentClass = classToOpen;
        gameObject.GetComponentInParent<Menu>().moveTo(classView);
        
    }

    private void OnEnable()
    {
        LoadClasses();
    }

    private void OnDisable()
    {
        UnloadClasses();
    }

    public void UnloadClasses()
    {
        if (loadedClasses.Count != 0)
        {
            for (int i = loadedClasses.Count - 1; i >= 0; i--)
            {
                Destroy(loadedClasses[i]);
            }
            loadedClasses.Clear();
        }
    }
}
