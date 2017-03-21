using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    public Transform character;

    float MOVE_SPEED = 7.0f;
    Quaternion upright;

	// Use this for initialization
	void Start () {
        upright = character.rotation;
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

        //character.rotation.SetEulerAngles(0, 3.14f, 0);
        character.rotation = upright;
        character.transform.position.Set(character.transform.position.x, 0.5f, character.transform.position.z);
    }
}
