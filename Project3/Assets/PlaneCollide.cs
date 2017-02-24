using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCollide : MonoBehaviour {

    public Transform ship;
    public Transform cam;
    public Transform prop;

    public PointLoader pointLoader;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        prop.Rotate(Vector3.right * Time.deltaTime * 1000.0f);
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "building")
        {
            Debug.Log("collide");

            Transform point = pointLoader.restartCountdown();
            ship.transform.position = point.GetComponent<CollisionHandler>().collidePos;
            ship.transform.rotation = point.GetComponent<CollisionHandler>().collideRot;

            cam.forward = ship.forward;

            Vector3 camPos;

            camPos = ship.position - ship.forward * 0.75f;
            camPos.y += 0.25f;

            cam.position = camPos;
        }
    }
}
