using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PointShip : MonoBehaviour
{
    public string currentPointName;
    public int relations = 0;
    private Animator animator;
    private SpriteRenderer spriteColor;
    public GameObject Trail;

    private GameObject currentPoint;
    private GameObject shipIcon;
    public Encounter encounter;
    public int encounterIndex;
    public EnumHolder.Faction faction;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake() {
        animator = this.GetComponent<Animator>();
        spriteColor = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        shipIcon = this.transform.GetChild(0).gameObject;
        Trail.SetActive(false);

        StartCoroutine(WaitToTurnOnTrail());
    }

    public void OnFirstLoad() {
    
        // For current difficulty, get a random encounter from the resource folder (resource/encounters)
        // Get the current difficulty from the player info and then search for all encounters with that difficulty and get a random one
        int currentDifficulty = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>().currentDifficulty;
        Encounter[] encounters = Resources.LoadAll<Encounter>("Encounters");
        List<Encounter> possibleEncounters = new List<Encounter>();
        if (currentDifficulty > 3) {
            // Load all encounters - For Endless Mode
            foreach (Encounter e in encounters) {
                possibleEncounters.Add(e);
            }
        } else {
            foreach (Encounter e in encounters) {
                if (e.difficulty == currentDifficulty) {
                    possibleEncounters.Add(e);
                }
            }
        }
        int randEncounter = UnityEngine.Random.Range(0, possibleEncounters.Count);
        encounter = possibleEncounters[randEncounter];
        encounterIndex = randEncounter;
        faction = encounter.encounterFaction;

        SetPositionToCurrentPoint();

        // Set the ship's color to the faction's color
        SetupColor();
    }

    public IEnumerator OnReload() {
        yield return new WaitForNextFrameUnit();

        // Get the encounter from the encounter index
        encounter = ResourceLoader.GetEncounterByIndex(encounterIndex);
        Debug.Log("Encounter: " + encounter.encounterName);

        faction = encounter.encounterFaction;

        // Set relations to be the relations between the player and the faction
        relations = FactionInfo.factionRelations[faction];

        SetPositionToCurrentPoint();
        SetupColor();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if current point is visited (if not hide ship + stop animating)
        if (currentPoint != null) {
            if (!currentPoint.GetComponent<PointController>().visited) {
                // Disable the shipIcon
                shipIcon.SetActive(false);
            } else {
                // Enable the shipIcon
                shipIcon.SetActive(true);
            }
        }
    }

    public void SetPositionToCurrentPoint() {
        // Find the current point by name and set the ship's position to it
        currentPoint = GameObject.Find(currentPointName);
        if (currentPoint != null) {
            this.transform.position = currentPoint.transform.position;
            currentPoint.GetComponent<PointController>().AddShip(this.gameObject);
        }
        // Add this ship to the current point's list of ships
        // Debug.Log("Current Point: " + currentPoint.GetComponent<PointController>().ships);
    }

    public void SetupColor() {
        // Set the ship's color to the faction's color
        switch (faction) {
            case EnumHolder.Faction.Enforcers:
                spriteColor.color = new Color(0.0f, 0.0f, 1.0f, 1.0f);
                break;
            case EnumHolder.Faction.Merchants:
                spriteColor.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
                break;
            case EnumHolder.Faction.Outlaws:
                spriteColor.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                break;
            case EnumHolder.Faction.Cultists:
                spriteColor.color = new Color(1.0f, 0.0f, 1.0f, 1.0f);
                break;
            case EnumHolder.Faction.ExEmployees:
                spriteColor.color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
                break;
            case EnumHolder.Faction.HullBreakers:
                spriteColor.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                break;
        }
    }

    IEnumerator WaitToTurnOnTrail() {
        yield return new WaitForSeconds(0.5f);
        Trail.SetActive(true);
    }

}
