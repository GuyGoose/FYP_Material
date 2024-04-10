using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    public Pilot pilot;
    public Image pilotImage;
    public Sprite pilotSprite;
    private CharacterSelectController characterSelectController;

    // Start is called before the first frame update
    void Start()
    {
        // Get gameobject with CharacterSelectController script
        characterSelectController = GameObject.FindObjectOfType<CharacterSelectController>();
        // Set the pilot image to the pilot's sprite
        pilotImage.sprite = pilotSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick() {
        // Call the DisplayPilotInfo method from the CharacterSelectController script
        characterSelectController.DisplayPilotInfo(pilot);
    }
}
