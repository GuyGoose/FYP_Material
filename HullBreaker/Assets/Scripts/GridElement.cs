using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GridElement : MonoBehaviour
{
    public bool isOccupied = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If mouse is over a UI element and E is pressed, set the grid element to occupied
        if (EventSystem.current.IsPointerOverGameObject() && Input.GetKeyDown(KeyCode.E))
        {
            SetOccupied(true);
        }
    }

    public void SetOccupied(bool occupied)
    {
        isOccupied = occupied;
        if (isOccupied)
        {
            // Set the UI Image alpha to 100
            gameObject.GetComponent<Image>().color = new Color(0.5096f, 0.4888f, 0.52f, 1);
            
        }
        else
        {
            // Set the UI Image alpha to 20
            gameObject.GetComponent<Image>().color = new Color(0.5096f, 0.4888f, 0.52f, 0.2f);
        }
    }
}
