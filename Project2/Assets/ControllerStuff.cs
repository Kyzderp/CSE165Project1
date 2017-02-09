using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerStuff : MonoBehaviour {

    public OVRInput.Controller left;
    public OVRInput.Controller right;
    public GameObject l;
    public GameObject r;
    public GameObject character;
    GameObject myLine;
    Vector3 rightOffset;

    string[] selectableTypes = { "locker", "desk", "whiteboard", "chair", "cabinet", "3DTV" };

    // Use this for initialization
    void Start () {
        myLine = new GameObject();
        myLine.name = "line";
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));

        rightOffset = new Vector3(0, 0, 0);

        Color color = new Color(0f, 1f, 0f);
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 rightPos = r.transform.position + rightOffset;
        drawLine(rightPos, rightPos + r.transform.forward * 10f); // This is for drawing it ingame

        Debug.DrawRay(r.transform.position, r.transform.forward);

        handleTeleport();
        handleSelection();
	}

    void drawLine(Vector3 start, Vector3 end)
    {
        myLine.transform.position = start;
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    void handleTeleport()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            Ray ray = new Ray(r.transform.position, r.transform.forward);
            RaycastHit rayHit;

            if(Physics.Raycast(ray, out rayHit, Mathf.Infinity))
            {
                if(rayHit.transform.gameObject.tag == "floor")
                {
                    character.transform.position = new Vector3(rayHit.point.x, character.transform.position.y, rayHit.point.z);
                }
            }
        }
    }

    void handleSelection()
    {
        //Debug.Log(OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger));
        if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.9f)
        {
            Ray ray = new Ray(r.transform.position, r.transform.forward);
            RaycastHit rayHit;
            //Debug.Log("casting ray..");

            if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
            {
                //Debug.Log(rayHit.transform.gameObject.tag);
                if (System.Array.IndexOf(selectableTypes, rayHit.transform.gameObject.tag) >= 0)
                {
                    Debug.Log("u hit a selektabl objek!!1");
                }
            }
        }
    }
}
