using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    //TODO: Make a damage system
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {        
            Debug.Log($"Hit enemy {collision.transform.name}");
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"You hit {collision.transform.name}");
            Destroy(gameObject);
        }
    }
}
