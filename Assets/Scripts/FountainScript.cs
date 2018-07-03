using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FountainScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("enter font");
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("stay font");

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trig ent font");

    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("trig stay font");

    }
}
