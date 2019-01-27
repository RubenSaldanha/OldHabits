using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthStationScript : MonoBehaviour {

    OldManScript oldMan;
    ParticleSystem emitter;

	// Use this for initialization
	void Start () {
        oldMan = FindObjectOfType<OldManScript>();
        emitter = GameObject.Find("healthEmitter").GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {


        if ((oldMan.transform.position - transform.position).magnitude < oldMan.healRange)
        {
            if(Random.value < 0.2f)
                emitter.Emit(1);;
        }
    }
}
