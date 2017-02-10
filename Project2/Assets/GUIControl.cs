using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIControl : MonoBehaviour {

    public Transform canvas;

    public Transform whiteboardPrefab;
    public Transform lockerPrefab;
    public Transform deskPrefab;
    public Transform chairPrefab;
    public Transform cabinetPrefab;
    public Transform treeDTVPrefab;

    public GameObject l;

    string[] types = { "whiteboard", "locker", "desk", "chair", "cabinet", "3DTV" };
    int currentIndex;
    int previousIndex;
    int scrollCooldown;

    // Use this for initialization
    void Start ()
    {
        currentIndex = 0;
        previousIndex = 0;
        scrollCooldown = 0;
        setSelected(0);
	}

    // Update is called once per frame
    void Update()
    {
        if (scrollCooldown > 0)
            scrollCooldown--;

        // scrolling on the menu
        if (scrollCooldown == 0)
        {
            if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y > 0.1)
            {
                currentIndex--;
                scrollCooldown = 20; 
            }
            else if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y < -0.1)
            {
                currentIndex++;
                scrollCooldown = 20;
            }

            if (currentIndex < 0)
                currentIndex = 5;
            if (currentIndex > 5)
                currentIndex = 0;
        }


        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick))
        {
            spawnObject(types[currentIndex]);
        }

        // update colors
        if (currentIndex != previousIndex)
        {
            setUnselected(previousIndex);
            setSelected(currentIndex);
        }

        previousIndex = currentIndex;
    }

    void spawnObject(string type)
    {
        Vector3 pos = l.transform.position + l.transform.forward * 5.0f;
        if (pos.y < 2)
            pos.y = 2;

        Transform obj = null;

        Quaternion quat = new Quaternion(-0.7071068f, 0, 0, 0.7071068f);

        switch (type)
        {
            case "whiteboard": obj = Instantiate(whiteboardPrefab, pos, quat); break;
            case "locker": obj = Instantiate(lockerPrefab, pos, quat); break;
            case "desk": obj = Instantiate(deskPrefab, pos, quat); break;
            case "chair": obj = Instantiate(chairPrefab, pos, quat); break;
            case "cabinet": obj = Instantiate(cabinetPrefab, pos, quat); break;
            case "3DTV": obj = Instantiate(treeDTVPrefab, pos, quat); break;
            default: break;
        }
    }

    void setUnselected(int index)
    {
        canvas.GetChild(0).GetChild(index).GetComponent<UnityEngine.UI.Image>().color = new Color(1f, 1f, 1f);
    }

    void setSelected(int index)
    {
        canvas.GetChild(0).GetChild(index).GetComponent<UnityEngine.UI.Image>().color = new Color(0.9f, 1f, 0.9f);
    }
}
