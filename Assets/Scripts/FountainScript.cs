using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FountainScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // На какой стороне юнит?
        //isMinion = gameControl.GetComponent<GameControl>().IsMinion(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter" + other.name);

    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit");

    }
}
