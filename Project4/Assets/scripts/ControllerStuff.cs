using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerStuff : MonoBehaviour
{
    public GameFlow gameFlow;

    public OVRInput.Controller left;
    public OVRInput.Controller right;
    public GameObject l;
    public GameObject r;
    public GameObject character;
    public GameObject flashlight; // this is the one in the right hand
    public GameObject sparkplug; // this is the one in the left hand

    private GameObject myLine;

    // Use this for initialization
    void Start()
    {
        myLine = new GameObject();
        myLine.name = "line";
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        Color color = new Color(0f, 1f, 0f);
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameFlow.stage == GameFlow.Stages.Menu)
            drawLine(r.transform.position, r.transform.position + r.transform.forward * 10f, myLine); // This is for drawing it ingame

        handleSelection();
        handleManipulation();
    }

    void drawLine(Vector3 start, Vector3 end, GameObject line)
    {
        line.transform.position = start;
        LineRenderer lr = line.GetComponent<LineRenderer>();
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }



    void handleSelection()
    {
        // Right hand grabbing
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {
            Vector3 pos = r.transform.position - r.transform.forward * 0.3f;
            Ray ray = new Ray(pos, r.transform.forward);
            RaycastHit rayHit;

            float grabRadius = 0.2f;

            if (Physics.SphereCast(ray, grabRadius, out rayHit, 0.3f))
            {
                GameObject obj = rayHit.transform.gameObject;
                this.grabObject(obj);
            }
        }

        // Left hand grabbing
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        {
            Vector3 pos = l.transform.position - l.transform.forward * 0.3f;
            Ray ray = new Ray(pos, l.transform.forward);
            RaycastHit rayHit;

            float grabRadius = 0.2f;

            if (Physics.SphereCast(ray, grabRadius, out rayHit, 0.3f))
            {
                GameObject obj = rayHit.transform.gameObject;
                this.grabObject(obj);
            }
        }
    }

    void grabObject(GameObject obj)
    {
        if (obj.tag == "flashlight")
        {
            obj.SetActive(false);
            flashlight.SetActive(true);
        }
        else if (obj.tag == "sparkplug")
        {
            obj.SetActive(false);
            sparkplug.SetActive(true);
        }
    }

    void handleManipulation()
    {
    }
}
