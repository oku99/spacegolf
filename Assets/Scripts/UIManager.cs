using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// In-Game Menu
public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numHits;
    [SerializeField] private Canvas escMenu;

    private void Awake()
    {
        // Disables the Esc Menu
        escMenu.GetComponent<Canvas>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        numHits.text = "Hits: " + GameManager.Instance.Hit;
        
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            // check if scene is playable scene/not main menu or current menu
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MainMenu"))
            {
                // Game Menu
                if (escMenu.GetComponent<Canvas>().enabled) { EscMenuDisable(); }
                else { EscMenuEnable(); }
            }
        }

    }

    public void EscMenuEnable()
    {
        escMenu.GetComponent<Canvas>().enabled = true;
    }

    public void EscMenuDisable()
    {
        escMenu.GetComponent<Canvas>().enabled = false;
    }

    public void RestartLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        GameManager.Instance.Restart();
        SceneManager.LoadScene(currentScene);
        //Debug.Log(currentScene + " restarted");
    }

    public void GoToMainMenu()
    {
        GameManager.Instance.Reset();
        SceneManager.LoadScene("MainMenu");
    }
}
