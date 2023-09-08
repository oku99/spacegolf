using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// reference: https://docs.unity3d.com/Manual/MultipleCameras.html
// reference: https://emmaprats.com/p/how-to-rotate-the-camera-around-an-object-in-unity3d/

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject TPPCamera;
    [SerializeField] private GameObject OverlookCamera;
    [SerializeField] private GameObject FreeCamera;
    [SerializeField] private BallControl Ball;

    private Vector3 initMousePos;

    private float distance;

    private BallControl.State lastState;  // stores non-disabled Ball State

    void Start()
    {
        distance = (Ball.transform.position - TPPCamera.transform.position).magnitude;
        TPPCamera.SetActive(true);
        OverlookCamera.SetActive(false);
    }

    void Update()
    {   
        //Debug.Log(Ball.state);
        // while you pressing the P key or the ball is moving, the camera is changed to overlook view.
        if (Input.GetButton("Overview") || Ball.state == BallControl.State.Travelling)
        {
            TPPCamera.SetActive(false);
            OverlookCamera.SetActive(true);
            FreeCamera.SetActive(false);
        }else if (Input.GetMouseButton(1)) {  // right click
            TPPCamera.SetActive(false);
            OverlookCamera.SetActive(false);
            FreeCamera.SetActive(true);
        } else {
            TPPCamera.SetActive(true);
            OverlookCamera.SetActive(false);
            FreeCamera.SetActive(false);
            RotateCamera();
        }

        FreeCamSettings();

    }

    // Click and drag let you to rotate the TPP camera around the ball.
    private void RotateCamera()
    {
        if(Input.GetMouseButtonDown(0)){
            initMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }else if(Input.GetMouseButton(0)){
            Vector3 newMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Vector3 direction = initMousePos - newMousePos;
            
            float rotationY = -direction.x * 180 ;
            float rotationX = direction.y * 180;
            Camera.main.transform.position = Ball.transform.position;

            Camera.main.transform.Rotate(Vector3.right, rotationX);
            Camera.main.transform.Rotate(Vector3.up, rotationY, Space.World);

            Camera.main.transform.Translate(new Vector3(0, 0, -distance));

            initMousePos = newMousePos;
        }
    }

    private void FreeCamSettings()
    {
        // Temporarily disables Ball Control when in FreeCamera mode
        if (Ball.state != BallControl.State.Disabled) { lastState = Ball.state; }
        Ball.state = FreeCamera.activeSelf ? BallControl.State.Disabled : lastState;
    }
    
}
