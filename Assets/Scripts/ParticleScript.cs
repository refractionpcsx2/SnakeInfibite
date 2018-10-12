using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour {
    float timeToDestroy = 5;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        timeToDestroy -= Time.deltaTime;

        if(timeToDestroy <= 0f)
        {
            Destroy(gameObject);
        }
	}
}
