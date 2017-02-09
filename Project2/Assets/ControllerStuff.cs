using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerStuff : MonoBehaviour {

    public OVRInput.Controller left;
    public OVRInput.Controller right;
    GameObject myLine;

    // Use this for initialization
    void Start () {
        GameObject myLine = new GameObject();
        myLine.name = "line";
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));

        Color color = new Color(0f, 1f, 0f);
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 rightPos = OVRInput.GetLocalControllerPosition(right);
        Quaternion rightRot = OVRInput.GetLocalControllerRotation(right);

        Vector3 rightDir = rightRot * Vector3.forward;
        drawLine(rightPos, rightPos + rightDir * 10f); // This is for drawing it ingame
        Debug.DrawRay(rightPos, rightDir);

        Ray point = new Ray(rightPos, rightDir);
	}

    void drawLine(Vector3 start, Vector3 end)
    {
        myLine.transform.position = start;
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
}
