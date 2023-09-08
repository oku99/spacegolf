using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HoleGoal : MonoBehaviour
{

    [SerializeField] private string nextScene;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Ball") {
            SceneManager.LoadScene(nextScene);
        }
    }
}
