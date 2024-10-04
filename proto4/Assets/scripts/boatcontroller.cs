using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class boatcontroller : MonoBehaviour
{
    public GameObject helm, helmposition,sail;
    public float maxspeed;
    public float rotspeed;
    public Rigidbody RB;
    public float saildepth;
    public bool sailsdown;
    public float helmturnpos;
    public void Awake()
    {
        storemanager.God.BM = this;
    }

    public void Update()
    {
        if (sailsdown)
        {
            float sails = saildepth * 5;
            RB.AddForce(transform.right * sails);
        }

        if (RB.velocity.magnitude > 10)
        {
            RB.drag = 5;
        }
        else
        {
            RB.drag = 1;
        }
        RB.AddTorque(transform.up * helmturnpos,ForceMode.Force);

        if (Input.GetKey(KeyCode.D))
        {
            helmturnpos += 0.01f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            helmturnpos -= 0.01f;
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

        sail.transform.localScale = new Vector3(saildepth/2+0.1f, sail.transform.localScale.y, saildepth/3+0.1f);
    }
}
