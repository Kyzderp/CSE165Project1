using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeSystem : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    int i = 0;
    float timer = 2.0f;
    static GameObject curBrick;
    static GameObject prevBrick;
	
	// Update is called once per frame
	void Update () {
        Ray gaze = new Ray(transform.position, transform.forward);
        // cant figure out how to get a lazer to show up...
       // Debug.DrawRay(transform.position, transform.forward * 10.0f, Color.green, 5);
        RaycastHit rayHit;

        if(Physics.Raycast(gaze, out rayHit, Mathf.Infinity))
        {
           // Debug.LogFormat("You hit {0}!", rayHit.collider.name);
            if(rayHit.transform.gameObject.tag == "brick")
            {
                prevBrick = curBrick;
                curBrick = rayHit.transform.gameObject;
                if(curBrick == prevBrick)
                {
                    timer -= Time.deltaTime;
                } else
                {
                    Debug.Log("Resetting Timer");
                    timer = 2.0f;
                }

                if(timer < 0f)
                {
                    Destroy(rayHit.transform.gameObject);
                }
                //Debug.LogFormat("You hit a brick!");
            }
        }
		
	}
}
