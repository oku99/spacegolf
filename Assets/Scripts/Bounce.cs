using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{   
    private Rigidbody rb;
    [SerializeField] float bounceK = 1.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        // GameObject collidedEntity = collisionInfo.gameObject;
        Debug.Log(Vector3.Reflect(rb.velocity.normalized, 
                                       collisionInfo.GetContact(0).normal) * rb.velocity.magnitude * bounceK);
        rb.AddForce(Vector3.Reflect(rb.velocity.normalized, 
                                       collisionInfo.GetContact(0).normal) * rb.velocity.magnitude * bounceK, ForceMode.Impulse);
    }
}
