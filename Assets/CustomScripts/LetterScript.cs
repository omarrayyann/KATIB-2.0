using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class LetterScript : MonoBehaviour
{

    public Letter letter = null;
    public Boolean selected = false;

    [SerializeField]
    public Image image;
    
    [SerializeField]
    public Image tick;
    

    // Start is called before the first frame update
    void Start()
    {
        Outline outline = gameObject.AddComponent<Outline>();
        outline.useGraphicAlpha = false;
        outline.effectColor = new Color(0f, 0f, 0f, 1f);
        outline.effectDistance = new Vector2(2f, -2f);
        tick.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void clickedLetter(){
        selected = !selected;
        Debug.Log("clicked letter: " + letter.name);
        if (selected){
            tick.enabled = true;
            Data.selectedLetters.Add(letter);
            gameObject.GetComponent<Outline>().effectColor = new Color(0.6f, 0f, 0f, 1f);
            gameObject.GetComponent<Outline>().effectDistance = new Vector2(2.5f, -2.5f);
        }
        else{
            Data.selectedLetters.RemoveAll(p => p == letter);
            gameObject.GetComponent<Outline>().effectColor = new Color(0f, 0f, 0f, 1f);
            tick.enabled = false;
        }

    }
}
