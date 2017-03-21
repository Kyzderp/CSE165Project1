using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    public Transform character;

    float MOVE_SPEED = 7.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        handleMovement();
	}

    void handleMovement()
    {
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0.0f)
        {
            Vector3 dir = new Vector3(transform.forward.x, 0, transform.forward.z);
            character.position += dir * Time.deltaTime * MOVE_SPEED;
        }
    }
}
