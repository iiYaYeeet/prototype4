using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;
using Debug = System.Diagnostics.Debug;
using GameObject = UnityEngine.GameObject;

public class playercont : MonoBehaviour
{
    //comps
    public Rigidbody RB;
    public CanvasGroup cg;
    public AudioSource AS;
    public AudioClip AC;
    //floats
    public float MouseSensitivity = 2;
    public float WalkSpeed = 10;
    //headbob stuff
    public float transitionSpeed = 10f;
    public float bobSpeed = 4.8f;
    public float bobAmount = 0.05f;
    float timer = Mathf.PI / 2;
    //objs
    public Camera Eyes;
    public Vector3 camPos;
    public Vector3 restPosition;
    public List<GameObject> gameobs;
    public bool moving;
    
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
            //movement
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) ||
                Input.GetKey(KeyCode.D)) //moving
            {
                timer += bobSpeed * Time.deltaTime;
                Vector3 newPosition = new Vector3(Mathf.Cos(timer) * bobAmount,
                    restPosition.y + Mathf.Abs((Mathf.Sin(timer) * bobAmount)),
                    restPosition.z); //abs val of y for a parabolic path
                camPos = newPosition;
            }

            else
            {
                timer = Mathf.PI / 2;

                Vector3 newPosition = new Vector3(
                    Mathf.Lerp(camPos.x, restPosition.x, transitionSpeed * Time.deltaTime),
                    Mathf.Lerp(camPos.y, restPosition.y, transitionSpeed * Time.deltaTime),
                    Mathf.Lerp(camPos.z, restPosition.z,
                        transitionSpeed * Time.deltaTime)); //transition smoothly from walking to stopping.
                camPos = newPosition;
            }

            if (Input.GetKeyDown(KeyCode.W) && moving == false || Input.GetKeyDown(KeyCode.A) && moving == false || Input.GetKeyDown(KeyCode.S) && moving == false ||
                Input.GetKeyDown(KeyCode.D) && moving == false)
            {
                AS.PlayOneShot(AC);
                moving = true;
            }
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) ||
                Input.GetKeyUp(KeyCode.D))
            {
                AS.Stop();
                moving = false;
            }
            Eyes.transform.localPosition = camPos;
            if (timer > Mathf.PI * 2) //completed a full cycle on the unit circle. Reset to 0 to avoid bloated values.
            {
                timer = 0;
                AS.PlayOneShot(AC);
            }
            //get mousexy
            float xRot = Input.GetAxis("Mouse X") * MouseSensitivity;
            float yRot = -Input.GetAxis("Mouse Y") * MouseSensitivity;
            //horrot
            transform.Rotate(0, xRot, 0);
            //get rot
            Vector3 Prot = Eyes.transform.localRotation.eulerAngles;
            //add change to rot
            Prot.x += yRot;
            //if's
            if (Prot.x < -180)
            {
                Prot.x += 360;
            }

            if (Prot.x > 180)
            {
                Prot.x -= 360;
            }

            //clamp minmax
            Prot = new Vector3(Mathf.Clamp(Prot.x, -65, 40), 0, 0);
            //plug back in
            Eyes.transform.localRotation = Quaternion.Euler(Prot);

            if (WalkSpeed > 0)
            {
                //set 0
                Vector3 move = Vector3.zero;
                //fore
                if (Input.GetKey(KeyCode.W))
                    move += transform.forward;
                //aft
                if (Input.GetKey(KeyCode.S))
                    move -= transform.forward;
                //left
                if (Input.GetKey(KeyCode.A))
                    move -= transform.right;
                //right
                if (Input.GetKey(KeyCode.D))
                    move += transform.right;
                //setspeed
                move = move.normalized * WalkSpeed;
                //plug back in
                move = new Vector3(move.x, RB.velocity.y, move.z);
                RB.velocity = move;
            }

            foreach (GameObject G in gameobs)
            {
                if (Vector3.Distance(G.transform.position, transform.position)<25)
                {
                    float e = Mathf.Abs(Vector3.Distance(G.transform.position, transform.position));
                    float s = Mathf.InverseLerp(50, 20, e);
                    cg.alpha = s;
                }
            }

            if (cg.alpha >= 1f)
            {
                SceneManager.LoadScene("Scenes/SampleScene");
            }
    }
}
