using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuActions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MapScene");
    }

    public void LoadGame()
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene("LoadGame");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
