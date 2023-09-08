using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    private GameObject ball;
    private UniversalGravity ballGravityLogic;
    private BallControl ballControl;
    private static bool hasTeleported = false;
    [SerializeField] GameObject tpToObject;

    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
        ballGravityLogic = ball.GetComponent<UniversalGravity>();
        ballControl = ball.GetComponent<BallControl>();
        Debug.Assert(tpToObject);
    }

    // Update is called once per frame
    void Update(){
        Debug.Log(hasTeleported);
    }

    IEnumerator OnTriggerEnter(){
        if (Teleport.hasTeleported){
            yield return null;
        }
        else {
            ballGravityLogic.enabled = true;
            ballControl.state = BallControl.State.Travelling;
            ball.transform.position = tpToObject.transform.position;
            hasTeleported = true;
            yield return new WaitForSeconds(3.0f);
            hasTeleported = false;
        }
    }
    
}
