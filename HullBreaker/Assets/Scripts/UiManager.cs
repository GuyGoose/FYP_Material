using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    public GameObject[] shipButtons;
    public ActionManager actionManager;
    public GameObject pointer;
    public GameObject[] enemyShips;
    public GameObject GameOverPanel;
    public GameObject CombatWinPanel;
    public TextMeshProUGUI ShipsDestroyedText;
    public TextMeshProUGUI CreditsEarnedText;

    // Start is called before the first frame update
    void Start()
    {
        // Get ship buttons (tagged as "ShipButton")
        shipButtons = GameObject.FindGameObjectsWithTag("ShipTab");
        actionManager = FindObjectOfType<ActionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupShipButtons(List<Ship> ships) {
        // Set the ship buttons to the ships in the player's inventory
        for (int i = 0; i < ships.Count; i++) {
            shipButtons[i].GetComponent<ShipTab>().AssignShip(ships[i]);
        }
        // Set the first ship in the list as the current ship
        actionManager.UpdateCurrentShip(ships[0]);
        ChangeSelectedShips(shipButtons[0]);

        // Set the pointer to the first shipbuttons shipCharacter
        pointer.transform.position = shipButtons[0].GetComponent<ShipTab>().shipCharacter.transform.position;
    }

    public void ChangeSelectedShips(GameObject shipTab) {
        // Set all ship buttons to not selected
        foreach (GameObject button in shipButtons) {
            button.GetComponent<ShipTab>().animator.SetBool("Pressed", false);
        }
        // Set the selected ship button to selected
        shipTab.GetComponent<ShipTab>().animator.SetBool("Pressed", true);
        // Set the current ship to the selected ship
        actionManager.UpdateCurrentShip(shipTab.GetComponent<ShipTab>().ship);

        // Move the pointer to the selected ship's shipCharacter
        pointer.transform.position = shipTab.GetComponent<ShipTab>().shipCharacter.transform.position;
        // On the shipCharacter, set the anim trigger to "Flare"
        shipTab.GetComponent<ShipTab>().shipCharacter.GetComponent<Animator>().SetTrigger("Flare");
    }

    public void SetupEnemyShipsImages(List<Sprite> enemySprites) {
        for (int i = 0; i < enemySprites.Count; i++) {
            enemyShips[i].SetActive(true);
            enemyShips[i].GetComponent<SpriteRenderer>().sprite = enemySprites[i];
        }
    }

    public void DisplayGameOver() {
        // Fade in canvas group
        while (GameOverPanel.GetComponent<CanvasGroup>().alpha < 1) {
            GameOverPanel.GetComponent<CanvasGroup>().alpha += Time.deltaTime * 1;
            // Turn on block raycast
            GameOverPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }

    public void DisplayCombatWin(List<Ship> shipsDestroyed, int encounterDifficulty, bool isBossFight) {

        // Fade in canvas group
        while (CombatWinPanel.GetComponent<CanvasGroup>().alpha < 1) {
            CombatWinPanel.GetComponent<CanvasGroup>().alpha += Time.deltaTime * 1;
            // Turn on block raycast
            CombatWinPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        // Set the ships destroyed text (The name of the ships destroyed line by line)
        ShipsDestroyedText.text = "";
        foreach (Ship ship in shipsDestroyed) {
            ShipsDestroyedText.text += ship.shipName + "\n";
        }

        // Set the credits earned text (For each level of difficulty, the player earns between 75-125 credits)
        int creditsEarned = Random.Range(75, 125) * encounterDifficulty;
        if (isBossFight) {
            creditsEarned *= 2;
        }
        actionManager.AddCredits(creditsEarned);
        CreditsEarnedText.text = "Credits Earned: " + creditsEarned.ToString() + "c";
    }

    public void DebugOnClick() {
        Debug.Log("Debugging");
    }
}
