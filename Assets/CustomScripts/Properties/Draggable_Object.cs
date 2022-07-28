using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable_Object : MonoBehaviour
{

    private Vector3 screenPoint;
    private Vector3 offset;
    
    void Start()
    {

    }
    
    // Update is called once per frame
    void Update()
    {
       
    }

       
    void OnMouseDown() {

    	offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}

	void OnMouseDrag()
	{

        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
    	Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        if (Vector2.Distance(Data.collectables[Data.collectables.Count-1].path[0], new Vector2(curPosition.x, curPosition.y))<0.2){
            transform.position = Data.collectables[Data.collectables.Count-1].path[0];
            Data.collectables[Data.collectables.Count-1].path.RemoveAt(0);
        }



	}


}