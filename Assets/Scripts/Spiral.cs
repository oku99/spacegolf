using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiral : MonoBehaviour
{
    [SerializeField] private GameObject ballChild;
    
    [SerializeField] private GameObject[] teleporters;

    [SerializeField] private float distanceLimit = 0;
    [SerializeField] private float rotateSpeed;

    private float radius = 1.5f;

    private GameObject activeTeleporter;

    private Renderer childRenderer;
    private Renderer parentRenderer;
    private Rigidbody ballRB;

    private BallControl ballControl;
    private UniversalGravity ballGravityLogic;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(distanceLimit != 0);
        ballChild = this.gameObject;
        childRenderer = GetComponent<MeshRenderer>();
        parentRenderer = transform.parent.GetComponentInParent<MeshRenderer>();
        ballRB = transform.parent.GetComponentInParent<Rigidbody>();
        ballControl = transform.parent.GetComponentInParent<BallControl>();
        ballGravityLogic = transform.parent.GetComponentInParent<UniversalGravity>();

    }

    // Update is called once per frame
    void Update()
    {   
        if (activeTeleporter == null){

            // make child invisible 
            Debug.Assert(childRenderer != null);
            childRenderer.enabled = false;

            float maxDistance = float.MaxValue;
            foreach (GameObject teleporter in teleporters){
                float distance = (teleporter.transform.position - transform.parent.position).magnitude;
                if (distance < maxDistance && distance < distanceLimit){
                    maxDistance = distance;
                    activeTeleporter = teleporter;
                }
            }

            // assigned active Teleporter, change ball move towards teleporter
            if (activeTeleporter != null){
                Vector3 direction = (activeTeleporter.transform.position - transform.parent.position).normalized;
                ballGravityLogic.enabled = false;
                // ballRB.velocity = direction * ballRB.velocity.magnitude;
                ballRB.velocity = direction * 12;
                ballControl.state = BallControl.State.Spiralling;
                // Time.timeScale = 0.5f;
                // camera.StartSpiral();
            }
        }
        
        if (activeTeleporter != null){
            
            // make child visible and parent invisible
            childRenderer.enabled = true;
            parentRenderer.enabled = false;
            transform.position = radius * (transform.position - transform.parent.position).normalized + transform.parent.position; 
            if (radius >= 0){
                radius -= 0.002f;
                Vector3 direction = activeTeleporter.transform.position - transform.parent.position;
                transform.RotateAround(transform.parent.position, direction, rotateSpeed);
            }
            if (ballRB.velocity.magnitude <= 2){
                ballGravityLogic.enabled = true;
                ballControl.state = BallControl.State.Travelling;
                childRenderer.enabled = false;
                parentRenderer.enabled = true;
                activeTeleporter = null;
                radius = 2.0f;
            }
            
            // transform.localPosition -= new Vector3(0.05f, 0, 0);
        }


        
    }

        
}

