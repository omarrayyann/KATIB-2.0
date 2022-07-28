using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour
{
    [SerializeField]
    private Sprite[] sprites = null;
    private int current = 0;

    private void Start()
    {
        Image image = this.GetComponent<Image>();
        image.sprite = sprites[current];
    }

    /// <summary>
    /// Switches the button's sprite to another one of the predefined sprites
    /// </summary>
    /// <param name="i">The index difference between the current sprite and the required one</param>
    public void ChangeBy(int i)
    {
        Image image = this.GetComponent<Image>();
        current += i;
        current %= sprites.Length;
        image.sprite = sprites[current];
    }
}
