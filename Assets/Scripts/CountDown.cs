using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour {

    public Text text;

	// Use this for initialization
	void Start () {
        int a = 11;
        text.text = a.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
