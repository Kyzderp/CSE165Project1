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
    GameObject lLine;
    bool selecting = false;
    List<GameObject> selection;
    float dist;

    List<GameObject> anchor;

    // note whiteboard not included
    string[] selectableTypes = { "locker", "desk", "chair", "cabinet", "3DTV", "whiteboard" };

    // Use this for initialization
    void Start () {
        selection = new List<GameObject>();
        anchor = new List<GameObject>();

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

        lLine = new GameObject();
        lLine.name = "lline";
        lLine.AddComponent<LineRenderer>();
        LineRenderer llr = lLine.GetComponent<LineRenderer>();
        llr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        llr.startColor = color;
        llr.endColor = color;
        llr.startWidth = 0.01f;
        llr.endWidth = 0.01f;
    }
	
	// Update is called once per frame
	void Update () {
        drawLine(r.transform.position, r.transform.position + r.transform.forward * 10f, myLine); // This is for drawing it ingame
        drawLine(l.transform.position, l.transform.position + l.transform.forward * 10f, lLine); // This is for drawing it ingame

        handleTeleport();
        handleSelection();
        handleManipulation();
        handleGroupSelect();
	}

    void drawLine(Vector3 start, Vector3 end, GameObject line)
    {
        line.transform.position = start;
        LineRenderer lr = line.GetComponent<LineRenderer>();
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
                    if (!selection.Contains(obj))
                    {
                        deselect();
                        select(obj); // Normal single selection
                    }
                    else
                    {
                        // Selecting an object that's in a group
                        // Put it on the front
                        selection.Remove(obj);
                        selection.Insert(0, obj);
                        // Keep track of all current position and rotation

                        anchor.Clear();

                        foreach (GameObject elem in selection)
                        {
                            GameObject newobj = new GameObject();
                            newobj.transform.position = new Vector3(elem.transform.position.x, elem.transform.position.y, elem.transform.position.z);
                            newobj.transform.rotation = new Quaternion(elem.transform.rotation.x,
                                elem.transform.rotation.y,
                                elem.transform.rotation.z,
                                elem.transform.rotation.w);
                            anchor.Add(newobj);
                        }
                        selecting = true;
                        Debug.Log("Grabbed item in group");
                    }
                }
            } 
        } else if(OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            deselect();
        }
    }

    void handleManipulation()
    {
        if (!selecting)
            return;

        if (selection.Count > 0 
            && selection[0].tag != "whiteboard")
        {
            // rotations
            Vector3 yrot = Vector3.up * OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x;
            Vector3 xrot = Vector3.left * OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y;
            //singleSelected.transform.Rotate(xrot, Space.World);
            selection[0].transform.Rotate(yrot, Space.World);

            // movement
            dist = (OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y * 0.1f) + dist;
            selection[0].transform.position = r.transform.position + (dist * r.transform.forward);

            doGroupManipulation();
        } 
        else if(selection.Count > 0 && selection[0].tag == "whiteboard")
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
                    selection[0].transform.position = wallPos;
                    selection[0].transform.forward = wallN;
                    selection[0].transform.Rotate(Vector3.right * -90);
                }
            }

            doGroupManipulation();
        }
    }

    void doGroupManipulation()
    {
        if (selection.Count <= 1)
            return;

        Vector3 posDiff = selection[0].transform.position - anchor[0].transform.position;
        float angle = selection[0].transform.eulerAngles.y - anchor[0].transform.eulerAngles.y;

        for (int i = 1; i < selection.Count; i++)
        {
            selection[i].transform.position = anchor[i].transform.position + posDiff;
            GameObject fake = new GameObject();
            fake.transform.rotation = anchor[i].transform.rotation;
            fake.transform.position = selection[i].transform.position;
            fake.transform.RotateAround(selection[0].transform.position, Vector3.up, angle);
            selection[i].transform.rotation = fake.transform.rotation;
            selection[i].transform.position = fake.transform.position;
        }
    }

    void deselect()
    {
        if (selection != null && selection.Count > 0)
        {
            foreach (GameObject obj in selection)
            {
                if (obj.tag != "whiteboard")
                {
                    obj.GetComponent<Rigidbody>().useGravity = true;
                    obj.GetComponent<Rigidbody>().isKinematic = false;
                }
            }
            selection.Clear();
        }

        selecting = false;
    }

    void select(GameObject obj)
    {
        selecting = true;
        selection.Clear();
        selection.Add(obj);

        if (selection[0].tag != "whiteboard")
        {
            selection[0].GetComponent<Rigidbody>().useGravity = false;
            selection[0].GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void clearSelected()
    {
        selection.Clear();
    }

    void handleGroupSelect()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            if (selecting)
                deselect();
            selecting = false;
            Ray ray = new Ray(l.transform.position, l.transform.forward);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
            {
                GameObject obj = rayHit.transform.gameObject;
                if (System.Array.IndexOf(selectableTypes, obj.tag) >= 0
                    && !selection.Contains(obj))
                {
                    if (selection.Count > 0)
                    {
                        // Make sure whiteboards can't be grouped with others, deselect if so
                        if (selection[0].tag == "whiteboard" && obj.tag != "whiteboard")
                            deselect();
                        else if (selection[0].tag != "whiteboard" && obj.tag == "whiteboard")
                            deselect();
                    }

                    selection.Add(obj);
                    if (obj.tag != "whiteboard")
                    {
                        obj.GetComponent<Rigidbody>().useGravity = false;
                        obj.GetComponent<Rigidbody>().isKinematic = true;
                    }
                    Debug.Log("Added item. Count is now " + selection.Count);
                }
            }
            else
            {
                deselect();
                Debug.Log("deselected group");
            }
        }
    }
}
