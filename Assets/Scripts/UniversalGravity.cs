using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This component is used to orient a physics body within a "planetary" system. */
public class UniversalGravity : MonoBehaviour
{
    [Tooltip("The gravitational constant. Used in Newton's law of universal gravitation.")]
    [SerializeField] float G; 
    [Tooltip("The system of bodies we want this object to be affected by.")]
    [SerializeField] GameObject[] planets;
    
    private Rigidbody rb;
    public static Vector3 vectorUp;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        vectorUp = transform.up;
    }

    void FixedUpdate()
    {
        if(BlackHole.Captured){

        }else{
            Vector3 universalGravity = Vector3.zero;
            foreach(GameObject planet in planets){
                Vector3 direction = planet.transform.position - transform.position;
                float distance = direction.magnitude;
                universalGravity += G * ((planet.GetComponent<Rigidbody>().mass * rb.mass) / Mathf.Pow(distance, 2)) * direction.normalized;
            }
            rb.AddForce(universalGravity, ForceMode.Force);
            vectorUp = -universalGravity.normalized; 
        }
        // Debug.Log(universalGravity);
        // the y-axis of local coordinate system of the ball is set to be anti-pararell to the total universal gravitational force.
            
    }

    
}
