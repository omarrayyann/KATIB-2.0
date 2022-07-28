using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet_Simulator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Data.collectables[Data.collectables.Count - 1].path.Count > 0)
        {
            transform.position = Data.collectables[Data.collectables.Count - 1].path[0];
        }
    }
}
