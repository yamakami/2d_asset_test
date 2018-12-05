using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeltonRising : MonoBehaviour {
    public GameObject target;


    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            target.SetActive(true);
        }
    }
}
