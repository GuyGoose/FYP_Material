using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MapGenerationController : MonoBehaviour
{
    public int pointRadius = 5;
    public int regionSize = 100;
    public enum DestinationType {
        Start,
        Planet,
        Encounter,
        Oddity,
        Merchant,
        Boss
    }
    public GameObject pointPrefab;

    // Statistics for point generation
    public float PlanetChance = 0.4f;
    public float encounterChance = 0.3f;
    public float OddityChance = 0.2f;
    public float MerchantChance = 0.1f;

    public GameObject destinationMenu;
    public GameObject landButton;
    private Animator destinationMenuAnimator;
    public TextMeshProUGUI destinationName;
    public TextMeshProUGUI planetName;
    public TextMeshProUGUI PlayerName, HP, Credits;
    public PlayerInfo playerInfo;
    public DataManager dataManager;
    
    private PointController currentlySelectedPoint;
    public GameObject pointShipPrefab;

    public GameObject cameraController;

    public GameObject pointContainer;
    public GameObject pointShipContainer;

    public int minMerchants = 3;
    public int maxMerchants = 5;	

    private List<Vector2> points;
    private GameObject currentPoint;

    private GameObject DialogueBox, FadeScreen;

    void Awake() {

        pointContainer = GameObject.Find("Points");
        pointShipContainer = GameObject.Find("PointShips");

        // Check if mapSave.json is empty
        if (File.Exists(Application.persistentDataPath + "/mapSave.json")) {
            Debug.Log("Loading Map...");
            dataManager.LoadMap();
            Debug.Log("Map Loaded");
        } else {
            Debug.Log("Generating Map...");
        }

        DialogueBox = GameObject.Find("DialogueBox");
        destinationMenuAnimator = destinationMenu.GetComponent<Animator>();
        CloseDestinationMenu();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Check if the map has been generated
        if (pointContainer.transform.childCount == 0) {
            GenerateMap();
        }

        // Tween to the current point
        foreach (Transform point in pointContainer.transform) {
            if (point.GetComponent<PointController>().current) {
                cameraController.GetComponent<MapCamMovement>().TweenToPosition(new Vector3(point.transform.position.x, point.transform.position.y, cameraController.transform.position.z));
            }
        }

        FadeScreen = GameObject.Find("Fade");

        // Set the player's name, health and credits
        PlayerName.text = playerInfo.playerName;
        HP.text = "HP: " + playerInfo.currentHealth + "/" + playerInfo.maxHealth;
        Credits.text = "Credits: " + playerInfo.credits + "c";

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
                    foreach (Transform point in pointContainer.transform) {
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
                    currentPoint = hit.collider.gameObject;
                    OpenDestinationMenu();
                    // Set the dialogue box to show Information about the point
                    DisplayInformation();
                    // If the current is start or completed, disable the land button
                    if (currentlySelectedPoint.type == DestinationType.Start.ToString() || currentlySelectedPoint.isCompleted){
                        DisableLandButton();
                    } else {
                        EnableLandButton();
                    }
                    // Calls the tween to position function in the camera controller with the points x and y position but keeps the z position the same
                    cameraController.GetComponent<MapCamMovement>().TweenToPosition(new Vector3(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y, cameraController.transform.position.z));
                    //hit.collider.gameObject.GetComponent<PointController>().DrawLinks();
                }
            } else {
                Debug.Log("Clicked Nothing");
                foreach (Transform point in pointContainer.transform) {
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
            foreach (Transform point in pointContainer.transform) {
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

    void DisableLandButton() {
        landButton.SetActive(false);
        // landButton.GetComponent<Button>().interactable = false;
        // landButton.GetComponent<Animator>().enabled = false;
    }

    void EnableLandButton() {
        landButton.SetActive(true);
        // landButton.GetComponent<Button>().interactable = true;
        // landButton.GetComponent<Animator>().enabled = true;
    }

    public void Land() {
        // If the point is the empty space with no ships, mark as completed
        switch (currentlySelectedPoint.type) {
            case "Start":
                MarkAsCompleted();
                break;
            case "Planet":
                // If the point has a point ship that is hostile, go to encounter
                playerInfo.isBossFight = false;
                if (currentPoint.GetComponent<PointController>().ships.Count > 0) {
                    if (currentPoint.GetComponent<PointController>().planetStatus == EnumHolder.PlanetStatus.Hostile) {
                        // Set the current encounter in PlayerInfo
                        playerInfo.currentEncounter = currentPoint.GetComponent<PointController>().ships[0].GetComponent<PointShip>().encounter;
                        // Destroy the point ship
                        Destroy(currentPoint.GetComponent<PointController>().ships[0]);
                        MarkAsCompleted();
                        StartCoroutine(GoToEncounter(playerInfo.currentEncounter));
                    } else {
                        MarkAsCompleted();
                    }
                } else {
                    MarkAsCompleted();
                }
                break;
            case "Oddity":
                MarkAsCompleted();
                break;
            case "Merchant":
                MarkAsCompleted();
                StartCoroutine(GoToMerchant());
                break;
            case "Boss":
                playerInfo.isBossFight = true;
                playerInfo.currentEncounter = ResourceLoader.GetRandomBossEncounterByDifficulty(1);
                MarkAsCompleted();
                StartCoroutine(GoToBossEncounter(playerInfo.currentEncounter));
                break;
        }

    }

    public IEnumerator GoToBossEncounter(Encounter encounter) {
        // Load the encounter scene
        Debug.Log("Loading Boss Encounter");

        // Set the current encounter in PlayerInfo
        playerInfo.currentEncounter = encounter;
        playerInfo.SavePlayerInfo();

        // Save the map state
        dataManager.SaveMap();

        FadeScreen.GetComponent<FadeScreenController>().StartCoroutine("FadeOut");
        //wait for a for 2 seconds
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("BossCombatScene");
    }

    public IEnumerator GoToEncounter(Encounter encounter) {
        // Load the encounter scene
        Debug.Log("Loading Encounter: " + encounter.encounterName);

        // Set the current encounter in PlayerInfo
        playerInfo.currentEncounter = encounter;
        playerInfo.SavePlayerInfo();

        // Save the map state
        dataManager.SaveMap();

        FadeScreen.GetComponent<FadeScreenController>().StartCoroutine("FadeOut");
        //wait for a for 2 seconds
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("CombatScene");
    }

    public IEnumerator GoToMerchant() {
        // Load the encounter scene
        Debug.Log("Loading Merchant Encounter");

        // Save the map state and player info
        playerInfo.SavePlayerInfo();
        dataManager.SaveMap();

        FadeScreen.GetComponent<FadeScreenController>().StartCoroutine("FadeOut");
        //wait for a for 2 seconds
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("ShopScene");
    }

    public void MarkAsCompleted() {
        currentlySelectedPoint.PointCompleted();
        // Mark point as current and unmark all other current points
        foreach (Transform point in pointContainer.transform) {
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

    public void DisplayInformation() {
        /*
        - Display the planet name
        - The planet status
        - Whether the planet is occupied by a faction or not

        Structure:
        - Alpha Centauri
        - Status: Hostile/Neutral/Friendly
        - Occupied by: Faction Name

        */
        //

        // - OLD 
        // If the point has a point ship, display the faction name of the ship
        // if (currentPoint.GetComponent<PointController>().ships.Count > 0) {
        //     string factionName = currentPoint.GetComponent<PointController>().ships[0].GetComponent<PointShip>().faction.ToString();
        //     DialogueBox.GetComponent<DialogueBox>().DisplayDialogue(currentlySelectedPoint.planetName + "\n\nStatus: Hostile\n\nOccupied by: " + factionName);
        // } else {
        //     DialogueBox.GetComponent<DialogueBox>().DisplayDialogue(currentlySelectedPoint.planetName + "\n\nStatus: Neutral\n\nOccupied by: None");
        // }

        // - NEW
        // If the point has a point ship, display the faction name of the ship and the status of the planet
        if (currentPoint.GetComponent<PointController>().ships.Count > 0) {
            string factionName = currentPoint.GetComponent<PointController>().ships[0].GetComponent<PointShip>().faction.ToString();
            string status = currentPoint.GetComponent<PointController>().planetStatus.ToString();
            DialogueBox.GetComponent<DialogueBox>().DisplayDialogue(currentlySelectedPoint.planetName + "\n\nStatus: " + status + "\n\nOccupied by: " + factionName);
        } else {
            DialogueBox.GetComponent<DialogueBox>().DisplayDialogue(currentlySelectedPoint.planetName + "\n\nStatus: Neutral\n\nOccupied by: None");
        }
    }

    public void GenerateMap() {
        points = PoissonDiscSampling.GeneratePoints(pointRadius, new Vector2(regionSize, regionSize), 30);

        // Generate points 
        // Rules for generation:
        // 1. Start point is always the first point
        // 2. Boss point is always the last point
        // 3. The number of Merchants is random should be between minMerchants and maxMerchants

        DestinationType lastType = DestinationType.Planet;
        foreach (Vector2 point in points) {
            GameObject pointObject = Instantiate(pointPrefab, point, Quaternion.identity);
            pointObject.transform.parent = pointContainer.transform;
            pointObject.GetComponent<PointController>().AssignName();
            if (point == points[0]) {
                pointObject.GetComponent<PointController>().type = DestinationType.Start.ToString();
                // set start point as current
                pointObject.GetComponent<PointController>().current = true;
            } else if (point == points[points.Count - 1]) {
                pointObject.GetComponent<PointController>().type = DestinationType.Boss.ToString();
            } else {
                float random = Random.value;
                if (random < PlanetChance) {
                    pointObject.GetComponent<PointController>().type = DestinationType.Planet.ToString();
                    lastType = DestinationType.Planet;
                } else if (random < PlanetChance + encounterChance) {
                    pointObject.GetComponent<PointController>().type = DestinationType.Planet.ToString();
                    // Create a ship at this point
                    GameObject ship = Instantiate(pointShipPrefab, point, Quaternion.identity);
                    ship.GetComponent<PointShip>().currentPointName = pointObject.GetComponent<PointController>().planetName;
                    ship.transform.parent = pointShipContainer.transform;
                    ship.GetComponent<PointShip>().OnFirstLoad();
                    lastType = DestinationType.Planet;
                } else if (random < PlanetChance + encounterChance + OddityChance) {
                    pointObject.GetComponent<PointController>().type = DestinationType.Oddity.ToString();
                    lastType = DestinationType.Oddity;
                    // Max of maxMerchants can be generated + cannot genrate two Merchants in a row
                } else if (random < PlanetChance + encounterChance + OddityChance + MerchantChance && maxMerchants > 0 && lastType != DestinationType.Merchant) {
                    pointObject.GetComponent<PointController>().type = DestinationType.Merchant.ToString();
                    maxMerchants--;
                    lastType = DestinationType.Merchant;
                } else {
                    pointObject.GetComponent<PointController>().type = DestinationType.Planet.ToString();
                    lastType = DestinationType.Planet;
                }
            }
            StartCoroutine(pointObject.GetComponent<PointController>().OnReload());
        }
    }
}
