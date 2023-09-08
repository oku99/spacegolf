using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Handling Game Data
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int Hit { get; private set; }
    public int Score { get; private set; }

    public int Level { get; private set; }
    private int _personalBest = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Instance.Level = 0;
        
        DontDestroyOnLoad(this.gameObject);
    }
    
    public void Reset()
    {
        Restart(); 
        Score = 0; }

    public void Restart() { Hit = 0; }

    public void AddHit(int add) { Hit += add; }

    // Resets Hit count when leaving the current Map but adds it to Total Score
    public void UpdateState()
    {
        Score += Hit;
        Hit = 0;
        Level += 1;
    }

    public int GetPersonalBest()
    {
        if (_personalBest == 0) { _personalBest = Score; }

        if (Score < _personalBest)
        {
            _personalBest = Score;
        }

        return _personalBest;
    }
}


/*
 * CODE ADAPTED FROM
 *      - "Implement data persistence between scenes" from Unity Learn
 *          - https://learn.unity.com/tutorial/implement-data-persistence-between-scenes#60b7415aedbc2a5532d1331c
 *
 * MUSIC
 *      - "How to play background music in Unity" by Rusben Guzman
 *          - https://rusbenguzman.medium.com/how-to-play-background-music-in-unity-82a6e0e87ff1
 *      - "Space - Golf With Your Friends Soundtrack" by sahmyol
 *          - https://www.youtube.com/watch?v=3cxTAPDB1DE
*/