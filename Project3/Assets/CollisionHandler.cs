using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour {

    public bool collected = false;
    public bool isNext = false;

    public Vector3 collidePos;
    public Quaternion collideRot;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "player")
        {
            if(isNext)
            {
                collected = true;

                collideRot = other.transform.rotation;
                collidePos = other.transform.position;
            }
        }
    }
}
