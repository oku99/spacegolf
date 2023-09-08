using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// reference : https://gamedevbeginner.com/how-to-convert-the-mouse-position-to-world-space-in-unity-2d-3d/#screen_to_world_3d

public class BallControl : MonoBehaviour
{

    // TO-DO: Clubs should probably have their own class
    // if we intend to add lots more in the future.
    private float MAX_POWER_DRIVER = 100f;
    private float MAX_POWER_PUTTER = 45f;
    private float MAX_SHOOT_LINE_LENGTH = 4f;
    private float BALL_RADIUS = 0.5f;
    private float MAX_UP_DRIVER = 85f;
    private float MAX_DOWN_DRIVER = 30f;
    private float MAX_UP_PUTTER = 30f;
    private float MAX_DOWN_PUTTER = 30f;


    [SerializeField] private LineRenderer shootLine;
    [SerializeField] private float shootPower;
    [SerializeField] private float aimSpeed;

    [SerializeField] AudioSource audioBigHit;
    [SerializeField] AudioSource audioSmallHit;

    private Rigidbody rb;

    private float timePressed = 0f;
    private float timeStill = 0f;
    private float maxPower;
    private float maxRotateUp;
    private float maxRotateDown;
    private Vector3 shootDir;

    // Using a (very primitive) finite state machine to handle input and camera stuff.
    // Good resource on the topic: https://gameprogrammingpatterns.com/state.html
    public enum State {Aiming, Hitting, TravellingSlow, Travelling, Disabled, Spiralling};

    public enum Club {Driver, Putter}
    public State state;
    private Club club;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        shootDir = transform.forward;

        // Initiate state.
        state = State.Aiming;
        club = Club.Driver;
        maxRotateUp = MAX_UP_DRIVER;
        maxRotateDown = MAX_DOWN_DRIVER;
        maxPower = MAX_POWER_DRIVER;
        shootLine.enabled = true;

    }

    void Update()
    {   
        if (state == State.Disabled) {
            
        } else if (state == State.Aiming) {
            // Cycle through the golf clubs.
            if (Input.GetButtonDown("ChangeClub")) {
                if (club == Club.Driver) {
                    club = Club.Putter;
                    maxPower = MAX_POWER_PUTTER;
                    maxRotateUp = MAX_UP_PUTTER;
                    maxRotateDown = MAX_DOWN_PUTTER;
                    shootLine.material.color = Color.blue;
                } else if (club == Club.Putter) {
                    club = Club.Driver;
                    maxPower =  MAX_POWER_DRIVER;
                    maxRotateUp = MAX_UP_DRIVER;
                    maxRotateDown = MAX_DOWN_DRIVER;                   
                    shootLine.material.color = Color.red;
                }
                shootDir = ShootDirToPlane(shootDir); // Resets the aim "tangent" to the planet
            }

            // Cap rotation.
            float rotationToPlane = Vector3.SignedAngle(ShootDirToPlane(shootDir), shootDir, Vector3.Cross(shootDir, transform.up));

            if (rotationToPlane < maxRotateUp) {
                if (Input.GetAxis("Vertical") > 0) {
                    shootDir = Quaternion.AngleAxis(Time.deltaTime * aimSpeed * Input.GetAxis("Vertical"), Vector3.Cross(shootDir, transform.up)) * shootDir;
                }
            }

            if (rotationToPlane > -maxRotateDown) {
                if (Input.GetAxis("Vertical") < 0) {
                    shootDir = Quaternion.AngleAxis(Time.deltaTime * aimSpeed * Input.GetAxis("Vertical"), Vector3.Cross(shootDir, transform.up)) * shootDir;
                }
            }
            // Rotate shoot direction according to user input. We multiply by 'deltaTime' to get a constant
            // rotation regardless of framerate.
            shootDir = Quaternion.AngleAxis(Time.deltaTime * aimSpeed * Input.GetAxis("Horizontal"), transform.up) * shootDir;
            shootLine.SetPositions(DrawVector(shootDir, MAX_SHOOT_LINE_LENGTH));


            if (Input.GetButtonDown("Fire")) {
                timePressed = 0f;
                state = State.Hitting;
                GameManager.Instance.AddHit(1);
            }

        } else if (state == State.Hitting) {

            timePressed += Time.deltaTime;
            float hitPower = Mathf.Lerp(0.0f, 1.0f, (timePressed * 0.5f)) * maxPower;
            shootLine.SetPositions(DrawVector(shootDir, (hitPower / maxPower) * MAX_SHOOT_LINE_LENGTH)); // We normalize hitPower to between [0, MAX_SHOOT_LINE_LENGTH]
            
            // TO-DO: User should be able to "bail out" of hitting the ball to readjust their aim.
            if (Input.GetButtonUp("Fire")) {

                rb.AddForce(shootDir * hitPower, ForceMode.Impulse);
                shootLine.enabled = false;
                timeStill = 0f;
                if (hitPower < 50f) {
                    audioSmallHit.Play();
                    state = State.TravellingSlow;
                } else {
                    audioBigHit.Play();
                    state = State.Travelling;
                }
            }


        } else if (state == State.Travelling || state == State.TravellingSlow) {

            // Go back to aiming after we've stopped moving for some amount of time.
            if (rb.velocity.magnitude < 1.0e-2) {
                timeStill += Time.deltaTime;
                if (timeStill > 1.0f) {
                    shootLine.enabled = true;
                    shootDir = ShootDirToPlane(shootDir); // Reset the aim.
                    state = State.Aiming;
                }
            }
        }

    }

    void FixedUpdate()
    {
        transform.up = UniversalGravity.vectorUp;
    }

    // "Pushes down" rotation vector to on the same plane the ball is on
    Vector3 ShootDirToPlane(Vector3 shootDir) {
        return Quaternion.AngleAxis(90f, transform.up) * Vector3.Cross(shootDir, transform.up).normalized;
    }


    // Returns a line between the centre of this object and the point
    // along vector 'v' some 'length' away.
    private Vector3[] DrawVector(Vector3 v, float length)
    {
        Vector3[] vertices = {
            transform.position + v * BALL_RADIUS,
            transform.position + v * (BALL_RADIUS + length)
        };
        return vertices;

    }

    public void startSpiral(){
        state = State.Spiralling;
    }

    public void stopSpiral(){
        state = State.Travelling;
    }

}
