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

    public Transform ship;
    public Transform cam;

    LeapProvider provider;

    bool thirdPerson = true;
    float speed = 0.25f;

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
        Hand Lhand, Rhand;

        if (left.gameObject.activeSelf && right.gameObject.activeSelf)
        {
            if (frame.Hands[0].IsLeft)
            {
                Lhand = frame.Hands[0];
                Rhand = frame.Hands[1];
            } else
            {
                Lhand = frame.Hands[1];
                Rhand = frame.Hands[0];
            }

            float hand_diff_y = Lhand.PalmPosition.ToVector3().y - Rhand.PalmPosition.ToVector3().y;
            hand_diff_y *= -350.0f; // this gives a range of about 5.0 (left higher) to -5.0 (right higher)
            //Debug.Log("lPalm.y: " + lPalm.localPosition.y + " rPalm.y: " + rPalm.localPosition.y);

            //float hand_diff_y = lPalm.localPosition.y - (rPalm.localPosition.y - 0.024f);
            //hand_diff_y *= -1000.0f;
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
        }


        ship.transform.position += ship.transform.forward * speed;
        cam.forward = ship.forward;

        Vector3 camPos;
        if (thirdPerson)
        {
            camPos = ship.position - ship.forward * 0.75f;
            camPos.y += 0.25f;
        } else
        {
            Quaternion cam_rotation = Quaternion.LookRotation(ship.forward, ship.up);
            cam.rotation = cam_rotation;

            camPos = ship.position - ship.forward * 5.0f;
            //camPos = ship.position - new Vector3(ship.forward.x, ship.forward.y - 1.0f, ship.forward.z) * 5.0f;
            //camPos.y += 5.0f;
        }

        cam.position = camPos;

    }
}
