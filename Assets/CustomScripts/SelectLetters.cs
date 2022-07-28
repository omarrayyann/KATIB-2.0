using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using UnityEngine.UI;


public class SelectLetters : MonoBehaviour
{


    [SerializeField]
    public GameObject letterPrefab;
    [SerializeField]
    public GameObject addPrefab;
    [SerializeField]
    private Menu addCustomMenu;


    List<GameObject> loadedLettersObjects = new List<GameObject>();

    void Start(){
        Data.resetLetters = false;
        List<Letter> letters = Server.LoadLetters(Data.lettersLanguage, Data.lettersType);
        // Debug.Log("Number of Letters: " + letters.Count);
        for(int i = 0; i<letters.Count; i++){
            GameObject letterScreen = Instantiate(letterPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            letterScreen.transform.SetParent(gameObject.transform);
            loadedLettersObjects.Add(letterScreen);
            letterScreen.transform.localScale = new Vector3(1, 1, 1);
            LetterScript letterScript = letterScreen.GetComponent(typeof(LetterScript)) as LetterScript;
            letterScript.image.sprite = letters[i].image;
            letterScript.letter = letters[i];
        }
        if (Data.lettersLanguage=="Custom"){
            GameObject addButton = Instantiate(addPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            addButton.transform.SetParent(gameObject.transform);
            loadedLettersObjects.Add(addButton);
            Menu parentMenu = addButton.GetComponentInParent<Menu>();
            Button button = addButton.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => parentMenu.moveTo(addCustomMenu));
            button.onClick.AddListener(() => GameObject.FindGameObjectWithTag("DrawCamera").GetComponent<Draw>().forceInactive = false);
            addButton.transform.localScale = new Vector3(1, 1, 1);
        }

    }

    public void clickedLetters(string languageAndType)
    {
        string[] languageAndTypeArray = languageAndType.Split('-');
        Data.resetLetters = true;
        Data.lettersLanguage = languageAndTypeArray[0];
        Data.lettersType = languageAndTypeArray[1];
        // Data.selectedLetters = new List<Letters>();
    }

    void Update(){
        if (Data.resetLetters && loadedLettersObjects.Count > 0)
        {
            Data.resetLetters = false;
            for(int i = 0; i<loadedLettersObjects.Count; i++){
                Destroy(loadedLettersObjects[i]);
            }
            Start();
        }
    }




    
}
