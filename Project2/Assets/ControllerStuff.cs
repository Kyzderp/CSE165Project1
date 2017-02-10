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
    bool selecting = false;
    GameObject singleSelected;
    float dist;
    

    // note whiteboard not included
    string[] selectableTypes = { "locker", "desk", "chair", "cabinet", "3DTV", "whiteboard" };

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
        handleManipulation();
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
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && !selecting)
        {
            Ray ray = new Ray(r.transform.position, r.transform.forward);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
            {
                GameObject obj = rayHit.transform.gameObject;
                dist = Vector3.Distance(r.transform.position, obj.transform.position);
                if (System.Array.IndexOf(selectableTypes, obj.tag) >= 0)
                {
                    select(obj);
                }
            } 
        } else if(OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            deselect();
        }
    }

    void handleManipulation()
    {
        if(singleSelected != null && singleSelected.tag != "whiteboard")
        {
            // rotations
            Vector3 yrot = Vector3.up * OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x;
            Vector3 xrot = Vector3.left * OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y;
            //singleSelected.transform.Rotate(xrot, Space.World);
            singleSelected.transform.Rotate(yrot, Space.World);

            // movement
            dist = (OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y * 0.1f) + dist;
            singleSelected.transform.position = r.transform.position + (dist * r.transform.forward);
        } 
        else if(singleSelected != null && singleSelected.tag == "whiteboard")
        {
            Ray ray = new Ray(r.transform.position, r.transform.forward);

            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray, Mathf.Infinity);

            foreach(var rayHit in hits)
            {
                if (rayHit.transform.gameObject.tag == "wall")
                {
                    Vector3 wallN = rayHit.normal;
                    Vector3 wallPos = rayHit.point;
                    singleSelected.transform.position = wallPos;
                    singleSelected.transform.forward = wallN;
                    singleSelected.transform.Rotate(Vector3.right * -90);
                }
            }
        }
    }

    void deselect()
    {
        if (singleSelected != null)
        {
            if(singleSelected.tag != "whiteboard")
            {
                singleSelected.GetComponent<Rigidbody>().useGravity = true;
                singleSelected.GetComponent<Rigidbody>().isKinematic = false;
            }
            singleSelected = null;
        }

        selecting = false;
    }

    void select(GameObject obj)
    {
        selecting = true;
        singleSelected = obj;

        if (singleSelected.tag != "whiteboard")
        {
            singleSelected.GetComponent<Rigidbody>().useGravity = false;
            singleSelected.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
