using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapGenerationController : MonoBehaviour
{
    public int pointRadius = 5;
    public int regionSize = 100;
    public enum DestinationType {
        Start,
        Empty,
        Encounter,
        Event,
        Shop,
        Boss
    }
    public GameObject pointPrefab;

    // Statistics for point generation
    public float emptyChance = 0.4f;
    public float encounterChance = 0.3f;
    public float eventChance = 0.2f;
    public float shopChance = 0.1f;

    public GameObject destinationMenu;
    public GameObject landButton;
    private Animator destinationMenuAnimator;
    public TextMeshProUGUI destinationName;
    public TextMeshProUGUI planetName;
    
    private PointController currentlySelectedPoint;
    public GameObject pointShipPrefab;

    public GameObject cameraController;

    public int minShops = 3;
    public int maxShops = 5;	

    List<Vector2> points;

    // Start is called before the first frame update
    void Start()
    {
        points = PoissonDiscSampling.GeneratePoints(pointRadius, new Vector2(regionSize, regionSize), 30);
        destinationMenuAnimator = destinationMenu.GetComponent<Animator>();
        CloseDestinationMenu();

        // Generate points 
        // Rules for generation:
        // 1. Start point is always the first point
        // 2. Boss point is always the last point
        // 3. The number of shops is random should be between minShops and maxShops

        DestinationType lastType = DestinationType.Empty;
        foreach (Vector2 point in points) {
            GameObject pointObject = Instantiate(pointPrefab, point, Quaternion.identity);
            pointObject.transform.parent = this.transform;
            if (point == points[0]) {
                pointObject.GetComponent<PointController>().type = DestinationType.Start.ToString();
            } else if (point == points[points.Count - 1]) {
                pointObject.GetComponent<PointController>().type = DestinationType.Boss.ToString();
            } else {
                float random = Random.value;
                if (random < emptyChance) {
                    pointObject.GetComponent<PointController>().type = DestinationType.Empty.ToString();
                    lastType = DestinationType.Empty;
                } else if (random < emptyChance + encounterChance) {
                    pointObject.GetComponent<PointController>().type = DestinationType.Empty.ToString();
                    // Create a ship at this point
                    GameObject ship = Instantiate(pointShipPrefab, point, Quaternion.identity);
                    ship.GetComponent<PointShip>().currentPoint = pointObject;
                    lastType = DestinationType.Empty;
                } else if (random < emptyChance + encounterChance + eventChance) {
                    pointObject.GetComponent<PointController>().type = DestinationType.Event.ToString();
                    lastType = DestinationType.Event;
                    // Max of maxShops can be generated + cannot genrate two shops in a row
                } else if (random < emptyChance + encounterChance + eventChance + shopChance && maxShops > 0 && lastType != DestinationType.Shop) {
                    pointObject.GetComponent<PointController>().type = DestinationType.Shop.ToString();
                    maxShops--;
                    lastType = DestinationType.Shop;
                } else {
                    pointObject.GetComponent<PointController>().type = DestinationType.Empty.ToString();
                    lastType = DestinationType.Empty;
                }
            }
        }

        // Connect points
        foreach (Transform point in this.transform) {
            point.GetComponent<PointController>().ConnectToPoints();
        }
        // Check for unconnected points
        foreach (Transform point in this.transform) {
            point.GetComponent<PointController>().CheckForCoLinks();
        }

    }

    // Update is called once per frame
    void Update()
    {
        // If a point is clicked, set it as the current point and un-current the previous point
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider != null ) {
                // If a ui element is clicked, ignore
                if (hit.collider.gameObject.GetComponent<Button>() != null) {
                    Debug.Log("Clicked UI Element");
                    return;
                }
                if (hit.collider.gameObject.tag == "Point" && hit.collider.gameObject.GetComponent<PointController>().visited) {
                    Debug.Log("Clicked Point");
                    foreach (Transform point in this.transform) {
                        if (point.GetComponent<PointController>().current) {
                            point.GetComponent<PointController>().current = false;
                        }
                        if(point.GetComponent<PointController>().isSelected) {
                            point.GetComponent<PointController>().isSelected = false;
                        }
                    }
                    hit.collider.gameObject.GetComponent<PointController>().current = true;
                    hit.collider.gameObject.GetComponent<PointController>().isSelected = true;
                    currentlySelectedPoint = hit.collider.gameObject.GetComponent<PointController>();
                    OpenDestinationMenu();
                    // Calls the tween to position function in the camera controller with the points x and y position but keeps the z position the same
                    cameraController.GetComponent<MapCamMovement>().TweenToPosition(new Vector3(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y, cameraController.transform.position.z));
                    //hit.collider.gameObject.GetComponent<PointController>().DrawLinks();
                }
            } else {
                Debug.Log("Clicked Nothing");
                foreach (Transform point in this.transform) {
                    if(point.GetComponent<PointController>().isSelected) {
                        point.GetComponent<PointController>().isSelected = false;
                    }
                }
            }
        }
        // If right click is pressed, close the destination menu
        if (Input.GetMouseButtonDown(1)) {
            CloseDestinationMenu();
        }

        // Debug Buttons
        // Press P to make all points current
        if (Input.GetKeyDown(KeyCode.P)) {
            foreach (Transform point in this.transform) {
                point.GetComponent<PointController>().PointCompleted();
            }
        }

    }

    public void OpenDestinationMenu() {
        destinationMenuAnimator.SetBool("isOn", true);
        landButton.SetActive(true);
        destinationName.text = currentlySelectedPoint.type;
        planetName.text = currentlySelectedPoint.planetName;
    }

    public void CloseDestinationMenu() {
        destinationMenuAnimator.SetBool("isOn", false);
        landButton.SetActive(false);
    }

    public void MarkAsCompleted() {
        currentlySelectedPoint.PointCompleted();
        // Mark point as current and unmark all other current points
        foreach (Transform point in this.transform) {
            if (point.GetComponent<PointController>().current) {
                point.GetComponent<PointController>().current = false;
            }
            if(point.GetComponent<PointController>().isSelected) {
                point.GetComponent<PointController>().isSelected = false;
            }
        }
        currentlySelectedPoint.current = true;
        // TODO - See PointShip.cs
        // All point ships move to a random connected point
        // foreach (PointShip ship in currentlySelectedPoint.ships) {
        //     ship.MoveToConnectedPointRandom();
        // }
        CloseDestinationMenu();
    }
}
