using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToCharacterSelect() {
        // Load the character select scene
        SceneManager.LoadScene("CharacterSelectScene");
    }

    public void GoToMap() {
        // Load the map scene
        SceneManager.LoadScene("MapScene");
    }

}