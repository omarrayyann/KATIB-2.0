using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SizeToScroll : MonoBehaviour
{
    [SerializeField]
    public int numMax = 2;

    // Start is called before the first frame update
    public void Refit()
    {
        ContentSizeFitter fitter = this.gameObject.GetComponent<ContentSizeFitter>();
        RectTransform[] cards = gameObject.GetComponentsInChildren<RectTransform>();
        if (cards.Length > numMax)
        {
            fitter.enabled = true;
        }
        else
        {
            fitter.enabled = false;
        }
    }

}
