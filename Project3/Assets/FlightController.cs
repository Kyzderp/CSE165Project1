using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;


public class FlightController : MonoBehaviour
{

    public Transform left;
    public Transform right;
    public Transform lPalm;
    public Transform rPalm;

    public Transform main;
    public Transform ship;
    public Transform cam;

    LeapProvider provider;

    float speed = 0.5f;

    // Use this for initialization
    void Start()
    {
        provider = FindObjectOfType<LeapProvider>() as LeapProvider;
        //ship.transform.forward = 
    }

    // Update is called once per frame
    void Update()
    {
        Frame frame = provider.CurrentFrame;

        if (left.gameObject.activeSelf && right.gameObject.activeSelf)
        {
            Hand Lhand = frame.Hands[0];
            Hand Rhand = frame.Hands[1];

            //Debug.Log("body y:" + gameObject.transform.position.y + " Lhand y: " + left.position.y);
            //Debug.Log("body y:" + gameObject.transform.position.y + " Lpalm y: " + lPalm.position.y);

            float hand_diff_y = Lhand.PalmPosition.ToVector3().y - Rhand.PalmPosition.ToVector3().y;
            hand_diff_y *= -275.0f; // this gives a range of about 5.0 (left higher) to -5.0 (right higher)
            //Debug.Log(hand_diff_y);

            float hand_avg_height = (lPalm.position.y + rPalm.position.y) / 2.0f;
            float relative_height = gameObject.transform.position.y - hand_avg_height -0.2f;
            relative_height = (relative_height * -200.0f) - 1.0f; // subtracting -1 to make the range more balanced
            //Debug.Log(relative_height); // range about -3.0 (highest) to 3.0 (lowest)
            // the relative_height range is how high/low your hands are relative to your body. Beware that moving
            // your gaze has about a +/- 0.3 effect on this

            // manipulate ship

            ship.Rotate(Vector3.forward * Time.deltaTime * hand_diff_y); // roll
          
            ship.Rotate(Vector3.right * Time.deltaTime * relative_height); // tilt
            //cam.Rotate(Vector3.right * Time.deltaTime * relative_height);
        }


        ship.transform.position += ship.transform.forward * speed;
        cam.forward = ship.forward;
        Vector3 camPos = ship.position - ship.forward * 50.0f;
        camPos.y += 15.0f;
        cam.position = camPos;

    }
}
