using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "StartScene") { 
            LoadMainMenuScene(); 
        }
        
    }

    public void LoadTestScene()
    {
        GameManager.Instance.Reset();
        SceneManager.LoadScene("Level1");
    }

    public void LoadCustomisationScene()
    {
        SceneManager.LoadScene("CustomisationScene");
    }

    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
    

    public void LoadInstructionsScene(){
        SceneManager.LoadScene("InstructionsScene");
    }
    

}
