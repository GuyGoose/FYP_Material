using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointController : MonoBehaviour
{
    public string type;
    public GameObject[] connectedPoints;
    // Start is called before the first frame update
    void Start()
    {
        switch (type) {
            case "Start":
                this.GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case "Empty":
                this.GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case "Encounter":
                this.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case "Event":
                this.GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case "Shop":
                this.GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            case "Boss":
                this.GetComponent<SpriteRenderer>().color = Color.green;
                this.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(2, 2, 2);
                break;
        }
        
    }

    void Awake() {
        ConnectPoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConnectPoints() {

    }
}
