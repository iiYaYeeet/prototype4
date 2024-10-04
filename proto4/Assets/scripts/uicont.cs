using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class uicont : MonoBehaviour
{
    public GameObject player;
    public SpriteRenderer SR;
    public GameObject text;
    public void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= 4)
        {
            SR.enabled = true;
            text.SetActive(true);
        }
        else
        {
            SR.enabled = false;
            text.SetActive(false);
        }
    }
}
