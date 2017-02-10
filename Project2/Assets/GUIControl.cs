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

    public FileStuff filestuff;

    string[] types = { "whiteboard", "locker", "desk", "chair", "cabinet", "3DTV" };
    int currentIndex;
    int previousIndex;
    int currentIndex2;
    int previousIndex2;
    int scrollCooldown;

    int screen;

    // Use this for initialization
    void Start ()
    {
        currentIndex = 0;
        previousIndex = 0;
        currentIndex2 = 0;
        previousIndex2 = 0;
        scrollCooldown = 0;
        screen = 0;
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
            if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x > 0.3)
            {
                if (screen == 0)
                {
                    screen = 1;
                    canvas.GetChild(0).gameObject.SetActive(false);
                    canvas.GetChild(1).gameObject.SetActive(true);
                    setSelected(currentIndex2);
                }
            }
            else if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x < -0.3)
            {
                if (screen == 1)
                {
                    screen = 0;
                    canvas.GetChild(1).gameObject.SetActive(false);
                    canvas.GetChild(0).gameObject.SetActive(true);
                }
            }
            else if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y > 0.3)
            {
                if (screen == 0)
                    currentIndex--;
                else
                    currentIndex2--;
                scrollCooldown = 20; 
            }
            else if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y < -0.3)
            {
                if (screen == 0)
                    currentIndex++;
                else
                    currentIndex2++;
                scrollCooldown = 20;
            }

            if (currentIndex < 0 && screen == 0)
                currentIndex = 5;
            if (currentIndex > 5 && screen == 0)
                currentIndex = 0;

            if (currentIndex2 < 0 && screen == 1)
                currentIndex2 = 3;
            if (currentIndex2 > 3 && screen == 1)
                currentIndex2 = 0;
        }


        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick) && screen == 0)
        {
            spawnObject(types[currentIndex]);
        }
        else if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick) && screen == 1)
        {
            if (currentIndex2 == 0)
                filestuff.saveData();
            else if (currentIndex2 == 1)
                filestuff.loadData();
            else if (currentIndex2 == 2)
                filestuff.deleteAll();
            else
                filestuff.loadData("default.txt");
        }

        // update colors
        if (currentIndex != previousIndex && screen == 0)
        {
            setUnselected(previousIndex);
            setSelected(currentIndex);
        }
        else if (currentIndex2 != previousIndex2 && screen == 1)
        {
            setUnselected(previousIndex2);
            setSelected(currentIndex2);
        }

        previousIndex = currentIndex;
        previousIndex2 = currentIndex2;
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
        canvas.GetChild(screen).GetChild(index).GetComponent<UnityEngine.UI.Image>().color = new Color(1f, 1f, 1f);
    }

    void setSelected(int index)
    {
        canvas.GetChild(screen).GetChild(index).GetComponent<UnityEngine.UI.Image>().color = new Color(0.9f, 1f, 0.9f);
    }
}
