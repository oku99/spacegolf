using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackHole : MonoBehaviour
{
    // [SerializeField] private BallControl Ball;
    [SerializeField] private float alpha; 
    [SerializeField] private float speed;
    [SerializeField] private float radius; 
    
    public static bool Captured = false;

    private Rigidbody rb;
    private float startTime = 0f;
    private float t = 1.0f;
    private float r  = 0f;
    private float height = 0f;
    private GameObject ball;


    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
        rb = ball.GetComponent<Rigidbody>();
        Captured = false;
        t = 1.0f;
        r = 0f; 
        height = 0f; 
    }

    void Update()
    {
        Vector3 position = rb.transform.position;
        position = transform.InverseTransformPoint(position);
        
        if(!Captured){
            IsInEllipsoid(position);
        }

        if(Captured){  
            if(t > 0.05f){
                t = t - (Time.time - startTime) * speed;
                float rad = alpha * t;
                        
                Vector3 newPos = new Vector3(
                                    t*t*t*r*Mathf.Cos(rad),
                                    Mathf.Sqrt(t)*height,
                                    t*t*t*r*Mathf.Sin(rad));  
                newPos = transform.TransformPoint(newPos);

                rb.transform.position = newPos;
                rb.transform.localScale = new Vector3(t, t, t);
            } else {
                Captured = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void IsInEllipsoid(Vector3 localPos){
        float s = Mathf.Pow(localPos.x,2)/(radius*radius) + Mathf.Pow(localPos.y,2)/(radius*radius)*8 + Mathf.Pow(localPos.z,2)/(radius*radius);
        if(s <= 1){
            Captured = true;
            height = 500f; 
            r = 800f; 
            startTime = Time.time;
        }
    }

}

//"Blackhole" (https://skfb.ly/owKUR) by shikoooooooo is licensed under Creative Commons Attribution (http://creativecommons.org/licenses/by/4.0/).
