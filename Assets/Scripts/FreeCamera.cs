using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Free Camera during Gameplay
public class FreeCamera : MonoBehaviour
{
    private int movementScale = 10;
    private float mouseSensitivity = 2.0f;

    [SerializeField] private Camera defaultCamera;
    [SerializeField] private Camera selfCamera;
    
    void Update()
    {
        // Keyboard Movement
        // Up
        if (Input.GetKey(KeyCode.Space)) { transform.position += transform.up * Time.deltaTime * movementScale; }
        
        // Left
        if (Input.GetKey(KeyCode.A)) { transform.position += -transform.right * Time.deltaTime * movementScale; }

        // Down
        if (Input.GetKey(KeyCode.LeftShift)) { transform.position += -transform.up * Time.deltaTime * movementScale; }
        
        // Right
        if (Input.GetKey(KeyCode.D)) { transform.position += transform.right * Time.deltaTime * movementScale; }

        // Forward
        if (Input.GetKey(KeyCode.W)) { transform.position += transform.forward * Time.deltaTime * movementScale; }

        // Right
        if (Input.GetKey(KeyCode.S)) { transform.position += -transform.forward * Time.deltaTime * movementScale; }

        // Mouse Movement
        float newRotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
        float newRotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * mouseSensitivity;
        if (newRotationY is < 271 and > 260) { newRotationY = 271; }
        if (newRotationY is > 89 and < 100) { newRotationY = 89; }
        transform.localRotation = Quaternion.Euler(new Vector3(newRotationY, newRotationX, 0f));

    }

    private void OnEnable()
    {
        // Gets Position of Default Camera
        selfCamera.CopyFrom(defaultCamera);
        
        // Locks Cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}


/*
 * CODE ADAPTED FROM
 *      - "Unity FreeCam" by ashleydavis
 *          - https://gist.github.com/ashleydavis/f025c03a9221bc840a2b
 *      - "Input.GetAxis" from Unity Documentation
 *          - https://docs.unity3d.com/ScriptReference/Input.GetAxis.html
 *      - "Cursor.lockState" from Unity Documentation
 *          - https://docs.unity3d.com/ScriptReference/Cursor-lockState.html
 */
