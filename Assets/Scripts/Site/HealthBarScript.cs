using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour {


    OldManScript oldMan;

	// Use this for initialization
	void Start () {
        oldMan = FindObjectOfType<OldManScript>();
	}
	
	// Update is called once per frame
	void Update () {
        RectTransform transf = GetComponent<RectTransform>();
        transf.localScale = new Vector3(oldMan.health * 80 , 80, 1f);
	}
}
