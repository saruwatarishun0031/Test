﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Playeruepon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("a");
            IInterface iif = other.gameObject.GetComponent<IInterface>();
            if (iif != null)
            {
                Debug.Log("tta");
                iif.ReceiveDamage(5);
            }
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        Debug.Log("a");
    //        IInterface iif = collision.gameObject.GetComponent<IInterface>();
    //        if (iif != null)
    //        {
    //            Debug.Log("tta");
    //            iif.ReceiveDamage(3);
    //        }
    //    }
    //}
}
