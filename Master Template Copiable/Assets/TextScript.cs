using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextScript : MonoBehaviour {



    public void ChangeLetter(char i ) {

        GetComponent<TextMesh>().text = Convert.ToString(i);
        Debug.Log("Display is now " + i);

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
