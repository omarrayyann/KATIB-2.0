using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Letter
{

    public String name { get; set; }
    public String language { get; set; }
    public String type { get; set; }
    public Sprite image { get; set; }
    public List<Vector2> path = new List<Vector2>();

    public Letter(String name, String language, String type)
    {
        this.name = name;
        this.language = language;
        this.type = type;
        image = null;
    }

    public Letter(String name, String language, String type, Sprite image){
        this.name = name;
        this.language = language;
        this.type = type;
        this.image = image;
    }  

    public string LetterToPath()
    {
        return "/Resources/Letters/" + language + "/" + type + "/Data/" + name + ".csv";
    }
}
