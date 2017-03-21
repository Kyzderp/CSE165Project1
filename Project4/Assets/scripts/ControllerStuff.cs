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
    public GameObject gameLight;
    public GameObject gameSpark;
    public GameObject box;
    public GameObject boxPlug;
    public GameObject breakerSearch;
    public GameObject lightswitch;
    public GameObject switchSearch;
    public GameObject switchup;

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
        float distanceThreshold = 1.0f;
        // Right hand grabbing
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {
            if (Vector3.Distance(r.transform.position, gameSpark.transform.position) < distanceThreshold)
                this.grabObject(gameSpark);
            if (Vector3.Distance(r.transform.position, gameLight.transform.position) < distanceThreshold)
                this.grabObject(gameLight);
        }
        // Left hand grabbing
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        {
            if (Vector3.Distance(l.transform.position, gameSpark.transform.position) < distanceThreshold)
                this.grabObject(gameSpark);
            if (Vector3.Distance(l.transform.position, gameLight.transform.position) < distanceThreshold)
                this.grabObject(gameLight);

            if (gameFlow.stage == GameFlow.Stages.Game && sparkplug.activeSelf)
            {
                if (Vector3.Distance(l.transform.position, box.transform.position) < distanceThreshold + 0.5f)
                    this.grabObject(box);
            }
            if (gameFlow.stage == GameFlow.Stages.Game && boxPlug.activeSelf)
            {
                if (Vector3.Distance(l.transform.position, lightswitch.transform.position) < distanceThreshold + 0.5f)
                    this.grabObject(lightswitch);
            }
        }
    }

    void grabObject(GameObject obj)
    {
        Debug.Log(obj.tag);
        if (obj.tag == "flashlight")
        {
            obj.SetActive(false);
            flashlight.SetActive(true);
            //GameObject.FindGameObjectWithTag("flashlightSearch").SetActive(false);
            if (gameFlow.stage == GameFlow.Stages.Pregame)
                gameFlow.elapsedTime = 181;
        }
        else if (gameFlow.stage == GameFlow.Stages.Game && obj.tag == "sparkplug")
        {
            obj.SetActive(false);
            sparkplug.SetActive(true);
            GameObject[] objs = GameObject.FindGameObjectsWithTag("sparkSearch");
            foreach (GameObject search in objs)
                search.SetActive(false);
            breakerSearch.SetActive(true);
            gameFlow.setText("You found a spark plug. Take it to the breaker box.");
        }
        else if (obj.tag == "breakerbox")
        {
            boxPlug.SetActive(true);
            sparkplug.SetActive(false);
            gameFlow.setText("That's how it works... right? Now go flip the switch!");
            breakerSearch.SetActive(false);
            switchSearch.SetActive(true);
        }
        else if (obj.tag == "switch")
        {
            gameFlow.setText("Ding!");
            switchSearch.SetActive(false);
            lightswitch.SetActive(false);
            switchup.SetActive(true);
            gameFlow.stage = GameFlow.Stages.GameWin;
        }
    }
}
