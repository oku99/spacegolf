using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    private float timer;
    [SerializeField] private TextMeshProUGUI Score;
    [SerializeField] private TextMeshProUGUI Next_Level;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.UpdateState();
        Score.text = "Current Score: " + GameManager.Instance.Score;
        if (GameManager.Instance.Level == 1){    
            Next_Level.text = "Going to Level 2...";
        }
        else if (GameManager.Instance.Level == 2){
            Next_Level.text = "Going to Level 3...";
        }
        else if (GameManager.Instance.Level == 3){
            Score.text += ("\nPersonal Best: " + GameManager.Instance.GetPersonalBest());
            Next_Level.text = "Congratulations, you beat the game!";
        }
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        //Load scene if counter has reached the nSecond
        if (timer > 3)
        {
            timer = 0;
            if (GameManager.Instance.Level == 1){
                
                SceneManager.LoadScene("Level2");
            }
            else if (GameManager.Instance.Level == 2){
                SceneManager.LoadScene("Level3");
            }
            else if (GameManager.Instance.Level == 3){
                SceneManager.LoadScene("MainMenu");
            }
        }
        
    }
}
