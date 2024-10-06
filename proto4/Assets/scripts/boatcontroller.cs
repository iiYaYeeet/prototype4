using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class boatcontroller : MonoBehaviour
{
    public GameObject sail;
    public float maxspeed;
    public float rotspeed;
    public Rigidbody RB;
    public float saildepth;
    public bool sailsdown, anchor;
    public float helmturnpos,anchordepth;
    public GameObject helm;
    public GameObject capstan;
    public float transitionSpeed = 10f;
    public float bobSpeed = 4.8f;
    public float bobAmount = 0.001f;
    float timer = Mathf.PI / 2;
    public GameObject boat;
    
    public void Awake()
    {
        storemanager.God.BM = this;
    }

    public void Update()
    {
        if (RB.velocity.magnitude > 20 && anchor==false)
        {
            RB.drag = 5;
        }
        else if (anchor)
        {
            RB.drag = 30;
        }
        else if (anchor==false)
        {
            RB.drag = 0.75f;
        }

        bobSpeed = RB.velocity.magnitude / 20+0.5f;
        bobAmount = RB.velocity.magnitude / 20+0.1f;
        timer += bobSpeed * Time.deltaTime;
        if (timer > Mathf.PI * 2) //completed a full cycle on the unit circle. Reset to 0 to avoid bloated values.
        {
            timer = 0;
        }

        float bob = Mathf.Sin(timer) * bobAmount;
        RB.AddTorque(new Vector3(0,90,0) * helmturnpos/5,ForceMode.Force);
        RB.AddTorque(boat.transform.right * bob, ForceMode.Force);
        if (storemanager.God.PC.onhelm)
        {
            if (Input.GetKey(KeyCode.D))
            {
                helmturnpos += 0.004f;
            }

            if (Input.GetKey(KeyCode.A))
            {
                helmturnpos -= 0.004f;
            }
        }
        if (storemanager.God.PC.onsail || storemanager.God.PC.onsail2)
        {
            if (Input.GetKey(KeyCode.S))
            {
                saildepth += 0.01f;
            }

            if (Input.GetKey(KeyCode.W))
            {
                saildepth -= 0.005f;
            }
        }
        if (saildepth >= 1.5f)
        {
            saildepth = 1.5f;
        }
        if (saildepth <= 0)
        {
            saildepth = 0;
        }
        if (helmturnpos <=0.05 && helmturnpos >=-0.05 && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            helmturnpos = 0;
        }
        if (helmturnpos >= 2)
        {
            helmturnpos = 2;
        }
        if (helmturnpos <= -2)
        {
            helmturnpos = -2;
        }

        helm.transform.localRotation = Quaternion.Euler(helmturnpos*-180,0,90);

        sail.transform.localScale =
            new Vector3(saildepth / 2 + 0.1f, sail.transform.localScale.y, saildepth / 3 + 0.1f);
        
        if (sailsdown)
        {
            float sails = saildepth * 2;
            RB.AddForce(transform.right * sails);
        }
    }

    public IEnumerator dropanchor()
    {
        while (anchordepth<30)
        {
            capstan.transform.Rotate(0,4,0,Space.Self);
            anchordepth += 0.30f;
            yield return new WaitForFixedUpdate();
        }
        anchor = true;
    }
    public IEnumerator raiseanchor()
    {
        while (anchordepth>0)
        {
            capstan.transform.Rotate(0,-2,0,Space.Self);
            anchordepth -= 0.15f;
            yield return new WaitForFixedUpdate();
        }
        anchor = false;
    }
}
