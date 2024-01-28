using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointController : MonoBehaviour
{
    public string type;
    public string planetName;
    public bool visited = false;
    public bool landed = false;
    public bool current = false;
    public bool isSelected = false;
    public List<GameObject> connectedPoints = new List<GameObject>();

    // Animation variables
    [SerializeField]
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {

        planetName = "Planet " + Random.Range(0, 1000).ToString(); // TODO: Generate planet names - Temporary placeholder

        switch (type) {
            case "Start":
                this.GetComponent<SpriteRenderer>().color = Color.green;
                current = true;
                DrawLinks();
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
                this.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(4, 4, 4);
                break;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (current) landed = true;
        if (landed) visited = true;
        // If this point is the current point, mark its connections as visited
        if (current) {
            animator.SetBool("isCurrent", true);
            foreach (GameObject connectedPoint in connectedPoints) {
                connectedPoint.GetComponent<PointController>().visited = true;
            }
        } else animator.SetBool("isCurrent", false);

        if (!visited) {
            // Disable the animator
            animator.enabled = false;

            this.GetComponent<SpriteRenderer>().color = Color.gray;
            this.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(1, 1, 1);
        } else {
            // Enable the animator
            animator.enabled = true;

            switch (type) {
                case "Start":
                    this.GetComponent<SpriteRenderer>().color = Color.green;
                    SetVisuals();
                    break;
                case "Empty":
                    this.GetComponent<SpriteRenderer>().color = Color.white;
                    SetVisuals();
                    break;
                case "Encounter":
                    this.GetComponent<SpriteRenderer>().color = Color.red;
                    SetVisuals();
                    break;
                case "Event":
                    this.GetComponent<SpriteRenderer>().color = Color.yellow;
                    SetVisuals();
                    break;
                case "Shop":
                    this.GetComponent<SpriteRenderer>().color = Color.blue;
                    SetVisuals();
                    break;
                case "Boss":
                    this.GetComponent<SpriteRenderer>().color = Color.green;
                    SetVisuals();
                    break;
            }
        }
    }

    public void ConnectToPoints() {
        Debug.Log("Started Connecting Points");
        // Check for points within a radius of 20 units and connect to the nearest 3
        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, 20);
        int count = 0;
        
        foreach (Collider2D collider in colliders) {
            if (collider.gameObject.tag == "Point" && collider.gameObject != this.gameObject) {
                // Get the 3 points with the shortest distance to this point
                if (count < 3) {
                    connectedPoints.Add(collider.gameObject);
                    count++;
                } else {
                    float maxDistance = 0;
                    int maxIndex = 0;
                    for (int i = 0; i < connectedPoints.Count; i++) {
                        if (Vector2.Distance(this.transform.position, connectedPoints[i].transform.position) > maxDistance) {
                            maxDistance = Vector2.Distance(this.transform.position, connectedPoints[i].transform.position);
                            maxIndex = i;
                        }
                    }
                    if (Vector2.Distance(this.transform.position, collider.gameObject.transform.position) < maxDistance) {
                        connectedPoints[maxIndex] = collider.gameObject;
                    }
                }
            }
        }
    }

    public bool CheckIfconnected(GameObject point) {
        foreach (GameObject connectedPoint in connectedPoints) {
            if (connectedPoint == point) {
                return true;
            }
        }
        return false;
    }

    public void DrawLinks() {
        foreach (GameObject connectedPoint in connectedPoints) {
            GameObject line = new GameObject();
            line.transform.parent = this.transform;
            line.AddComponent<LineRenderer>();
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.textureMode = LineTextureMode.Tile;
            lineRenderer.alignment = LineAlignment.View;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            // Line color is (141A1D)
            lineRenderer.startColor = new Color(0.078f, 0.102f, 0.114f);
            lineRenderer.endColor = new Color(0.078f, 0.102f, 0.114f);
            lineRenderer.startWidth = 0.2f;
            lineRenderer.endWidth = 0.2f;
            lineRenderer.SetPosition(0, this.transform.position);
            lineRenderer.SetPosition(1, connectedPoint.transform.position);
            lineRenderer.material.mainTextureScale = new Vector2(1f / lineRenderer.endWidth, 1.0f);
            // Make line dotted //TODO
            
        }
    }

    public void CheckForCoLinks() {
        // Checks connected points. If that connected point does not contain this point in its connected points, add it
        foreach (GameObject connectedPoint in connectedPoints) {
            if (!connectedPoint.GetComponent<PointController>().CheckIfconnected(this.gameObject)) {
                connectedPoint.GetComponent<PointController>().connectedPoints.Add(this.gameObject);
            }
        }
    }

    public void SetVisuals() {
        if (current) {
            this.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
            // Make the point non transparent
            Color color = this.GetComponent<SpriteRenderer>().color;
            color.a = 1f;
            this.GetComponent<SpriteRenderer>().color = color;
        } else if (!landed)
        {
            this.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(3, 3, 3);
            // Make the point transparent
            Color color = this.GetComponent<SpriteRenderer>().color;
            color.a = 0.2f;
            this.GetComponent<SpriteRenderer>().color = color;
        }
    }
}
