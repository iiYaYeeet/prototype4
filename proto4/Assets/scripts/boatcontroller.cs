using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class boatcontroller : MonoBehaviour
{
    public GameObject sail;
    public float maxspeed;
    public float rotspeed;
    public Rigidbody RB;
    public float saildepth;
    public bool sailsdown;
    public float helmturnpos;
    public GameObject helm;
    public void Awake()
    {
        storemanager.God.BM = this;
    }

    public void Update()
    {
        if (sailsdown)
        {
            float sails = saildepth * 2;
            RB.AddForce(transform.right * sails);
        }

        if (RB.velocity.magnitude > 10)
        {
            RB.drag = 5;
        }
        else
        {
            RB.drag = 0.75f;
        }
        RB.AddTorque(transform.up * helmturnpos,ForceMode.Force);
        if (storemanager.God.PC.onhelm)
        {
            if (Input.GetKey(KeyCode.D))
            {
                helmturnpos += 0.01f;
            }

            if (Input.GetKey(KeyCode.A))
            {
                helmturnpos -= 0.01f;
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
        if (helmturnpos <=0.3 && helmturnpos >=-0.3 && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
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

        sail.transform.localScale = new Vector3(saildepth/2+0.1f, sail.transform.localScale.y, saildepth/3+0.1f);
    }
}
