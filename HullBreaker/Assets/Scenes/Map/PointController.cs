using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;

public class PointController : MonoBehaviour
{
    public string type;
    public string planetName;
    public bool visited = false;
    public bool landed = false;
    public bool current = false;
    public bool isSelected = false;
    public bool isCompleted = false;
    public List<string> connectedPoints = new List<string>();
    public List<GameObject> ships = new List<GameObject>();
    public EnumHolder.PlanetStatus planetStatus;

    // Animation variables
    [SerializeField]
    private Animator animator;

    void Start()
    {
        this.gameObject.name = planetName;

        switch (type) {
            case "Start":
                this.GetComponent<SpriteRenderer>().color = Color.green;
                //current = true;
                landed = true;
                visited = true;
                isCompleted = true;
                break;
            case "Planet":
                this.GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case "Encounter":
                this.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case "Oddity":
                this.GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case "Merchant":
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
        // If a connected point is visited, this enable sprite renderer, else disable

        // If Completed Set all bools to true, and make connected points as visited
        if (isCompleted) {
            visited = true;
            landed = true;
            foreach (string connectedPoint in connectedPoints) {
                GameObject point = GameObject.Find(connectedPoint);
                if (point != null) {
                    point.GetComponent<PointController>().visited = true;
                }
            }
        }
        // If this point is the current point, mark its connections as visited
        if (current) {
            animator.SetBool("isCurrent", true);
        } else animator.SetBool("isCurrent", false);

        if (!visited) {
            // Disable the animator
            animator.enabled = false;
            this.GetComponent<SpriteRenderer>().enabled = false;

            this.GetComponent<SpriteRenderer>().color = Color.gray;
            this.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(1, 1, 1);
        } else {
            // Enable the animator
            animator.enabled = true;
            this.GetComponent<SpriteRenderer>().enabled = true;

            switch (type) {
                case "Start":
                    this.GetComponent<SpriteRenderer>().color = Color.green;
                    SetVisuals();
                    break;
                case "Planet":
                    this.GetComponent<SpriteRenderer>().color = Color.white;
                    SetVisuals();
                    break;
                case "Encounter":
                    this.GetComponent<SpriteRenderer>().color = Color.red;
                    SetVisuals();
                    break;
                case "Oddity":
                    this.GetComponent<SpriteRenderer>().color = Color.yellow;
                    SetVisuals();
                    break;
                case "Merchant":
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

    public IEnumerator OnFirstLoad() {
        yield return new WaitForNextFrameUnit();
        ConnectToPoints();
        
        // If start DrawLinks
        if (isCompleted) {
            DrawLinks();
        }
    }

    public IEnumerator OnReload() {
        yield return new WaitForNextFrameUnit();
        ConnectToPoints();
        SetVisuals();

        if (isCompleted) {
            DrawLinks();
        }
    }

    public void ConnectToPoints() {
        //Debug.Log("Started Connecting Points");
        // Check for points within a radius of 20 units and connect to the nearest 3
        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, 20);
        int count = 0;
        
        foreach (Collider2D collider in colliders) {
            if (collider.gameObject.tag == "Point" && collider.gameObject != this.gameObject) {
                // Get the 3 points with the shortest distance to this point
                if (count < 3) {
                    connectedPoints.Add(collider.gameObject.GetComponent<PointController>().planetName);
                    //Debug.Log("Connected: " + collider.gameObject.GetComponent<PointController>().planetName);
                    count++;
                } else {
                    float maxDistance = 0;
                    int maxIndex = 0;
                    for (int i = 0; i < connectedPoints.Count; i++) {
                        // Find the point with the PlanetName in connectedPoints
                        GameObject connectedPoint = GameObject.Find(connectedPoints[i]);
                        if (connectedPoint != null) {
                            if (Vector2.Distance(this.transform.position, connectedPoint.transform.position) > maxDistance) {
                                maxDistance = Vector2.Distance(this.transform.position, connectedPoint.transform.position);
                                maxIndex = i;
                            }
                        }
                    }
                    if (Vector2.Distance(this.transform.position, collider.gameObject.transform.position) < maxDistance) {
                        connectedPoints[maxIndex] = collider.gameObject.GetComponent<PointController>().planetName;
                    }
                }
            }
        }

        // Debug.Log("Finished Connecting Points");
        // Debug.Log("Connected Points: " + connectedPoints.Count);
        // foreach (string connectedPoint in connectedPoints) {
        //     Debug.Log("Connected: " + connectedPoint);
        // }
        CheckForCoLinks();
        RemoveDuplicateLinks();
    }

    public bool CheckIfconnected(string planetName) {
        return connectedPoints.Contains(planetName);
    }

    public void DrawLinks() {
        foreach (string connectedPoint in connectedPoints) {
            GameObject point = GameObject.Find(connectedPoint);
            if (point != null && point.GetComponent<PointController>().connectedPoints.Contains(this.planetName)) {
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
                lineRenderer.SetPosition(1, point.transform.position);
                lineRenderer.material.mainTextureScale = new Vector2(1f / lineRenderer.endWidth, 1.0f);
            }
        }
        //Debug.Log("Links Drawn for: " + this.planetNa-me);
    }

    public void CheckForCoLinks() {
        // Checks connected points. If that connected point does not contain this point in its connected points, add it. Also check for vice versa
        foreach (string connectedPoint in connectedPoints) {
            GameObject point = GameObject.Find(connectedPoint);
            if (point != null) {
                // If other point does not contain this point in its connected points, add it
                if (!point.GetComponent<PointController>().CheckIfconnected(this.planetName)) {
                    point.GetComponent<PointController>().connectedPoints.Add(this.planetName);
                }

                // If this point does not contain the other point in its connected points, add it
                if (!this.CheckIfconnected(point.GetComponent<PointController>().planetName)) {
                    this.connectedPoints.Add(point.GetComponent<PointController>().planetName);
                }
            }
        }
    }

    public void RemoveDuplicateLinks() {
        // Remove duplicate links
        List<string> uniqueConnectedPoints = new List<string>();
        foreach (string connectedPoint in connectedPoints) {
            if (!uniqueConnectedPoints.Contains(connectedPoint)) {
                uniqueConnectedPoints.Add(connectedPoint);
            }
        }
        connectedPoints = uniqueConnectedPoints;
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

    public void PointCompleted() {
        isCompleted = true;
        isSelected = true;
        SetVisuals();
        DrawLinks();
    }

    public void AssignName() {
        planetName = PlanetNames.GeneratePlanetName();
        // Set GameObject name to planetName
        this.gameObject.name = planetName;
    }

    public void AddShip(GameObject ship) {
        ships.Add(ship);

        // Get the ship's faction and set the planet's status accordingly (> 25 = friendly, < -25 = hostile)
        EnumHolder.Faction faction = ship.GetComponent<PointShip>().faction;
        if (FactionInfo.factionRelations[faction] > 25) {
            planetStatus = EnumHolder.PlanetStatus.Friendly;
        } else if (FactionInfo.factionRelations[faction] < -25) {
            planetStatus = EnumHolder.PlanetStatus.Hostile;
        } else {
            planetStatus = EnumHolder.PlanetStatus.Neutral;
        }

        SetStatusColor();
    }

    public void RemoveShip(GameObject ship) {
        ships.Remove(ship);
    }

    public void SetStatusColor() {
        switch (planetStatus) {
            case EnumHolder.PlanetStatus.Friendly:
                this.GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case EnumHolder.PlanetStatus.Neutral:
                this.GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case EnumHolder.PlanetStatus.Hostile:
                this.GetComponent<SpriteRenderer>().color = Color.red;
                break;
        }
    }
}
