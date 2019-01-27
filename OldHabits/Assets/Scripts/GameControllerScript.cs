using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("Game Controller Start");
        GameObject.DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
