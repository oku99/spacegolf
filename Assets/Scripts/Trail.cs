using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Handles the Trail being available across Scenes
public class Trail : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        //if more than one trail exists, copy its data over and delete old one || or use player save thingy
        ParticleSystem[] trails = FindObjectsOfType<ParticleSystem>();

        if (trails.Length > 1)
        {
            // Destroy
            Destroy(trails[0].gameObject);
        }

        AttachToBall();
        
        
    }

    private void AttachToBall()
    {
        // Find the ball in the scene (should be 1 per scene)
        foreach (GameObject currBall in GameObject.FindGameObjectsWithTag("Ball"))
        {
            this.gameObject.transform.position = currBall.transform.position;
        }
    }
}
