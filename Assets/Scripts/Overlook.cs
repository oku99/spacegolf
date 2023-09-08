using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overlook : MonoBehaviour
{
    [SerializeField] GameObject ball;

    private Vector3 localOffset;
    private Vector3 offset;
    private GameObject TPPCamera; 

    void Start()
    {
        // Initial relevent position of the overlook camera to the ball in terms of the local coordinates of the ball.
        TPPCamera = ball.transform.GetChild(0).gameObject;
        localOffset = TPPCamera.transform.localPosition; 
        localOffset *= 3.0f;
    }

    void Update()
    {  
        Vector3 pos = ball.transform.TransformPoint(localOffset);
        transform.position = pos;
        transform.LookAt(ball.transform.position); // The camera alawys look at the ball.
    }
}
