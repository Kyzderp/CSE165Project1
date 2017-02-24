using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PointLoader : MonoBehaviour 
{
	public Transform pointPrefab;
    public Transform ship;
    public Transform pointerPlane;
    public Transform cam;
    public TextMesh timerText;
    public TextMesh messageText;
    float messageDisplay;

    public TextMesh five;
    public TextMesh four;
    public TextMesh three;
    public TextMesh two;
    public TextMesh one;

    public Transform audioObject;
    SoundEffectPlayer sounds;

    List<Transform> points; // list of points
    List<GameObject> paths;
    int next = 0; // next point you need to collect
    int numPoints;
    public bool inGame = false; // True if the game is ongoing, false otherwise
    float timer = 0.0f;

    float countdown = 7.2f;
    int currCount = 6;
    TextMesh currText;

	// Use this for initialization
	void Start () {
        points = new List<Transform>();
        paths = new List<GameObject>();
        sounds = audioObject.gameObject.GetComponent<SoundEffectPlayer>();
        sounds.playCountDown();

        //loadData("test.txt");
        loadData("sample.txt");

        inGame = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (messageDisplay > 0)
        {
            messageDisplay -= Time.deltaTime;
            if (messageDisplay <= 0)
                messageText.text = "";
        }

        if(inGame)
        {
            timer += Time.deltaTime;
            timerText.text = timer.ToString("#.00");

            if (points[next].GetComponent<CollisionHandler>().collected == false)
            {
                // player has not collected next point

                // keep waiting for collision
            } else
            {
                // else player collected the next point
                handleCollected(points[next]);

                if(next == numPoints - 1)
                {
                    // you won
                    sounds.playWin();
                    inGame = false;
                } else
                {
                    sounds.stop();
                    sounds.playBurst();
                    sounds.playEngine();
                    next++;

                    setNext(points[next]);
                    if(next > 1)
                    popAndUpdatePath();
                }
            }

            if (inGame) pointerPlane.LookAt(points[next]); 
            else
            {
                timerText.color = Color.red;
                timerText.fontSize = 40;
                Destroy(paths[0]);
                Destroy(pointerPlane.gameObject);
            }
        }
        else
        {
            if (currCount < 0)
                return;

            // countdown
            countdown -= Time.deltaTime;
            if (countdown < currCount)
            {
                // New number
                currCount--;
                if (currCount == 5)
                {
                    one.gameObject.SetActive(false);
                    five.gameObject.SetActive(true);
                    currText = five;
                }
                else if (currCount == 4)
                {
                    five.gameObject.SetActive(false);
                    four.gameObject.SetActive(true);
                    currText = four;
                }
                else if (currCount == 3)
                {
                    four.gameObject.SetActive(false);
                    three.gameObject.SetActive(true);
                    currText = three;
                }
                else if (currCount == 2)
                {
                    three.gameObject.SetActive(false);
                    two.gameObject.SetActive(true);
                    currText = two;
                }
                else if (currCount == 1)
                {
                    two.gameObject.SetActive(false);
                    one.gameObject.SetActive(true);
                    currText = one;
                }

                currText.transform.localPosition = new Vector3(0.0f, 0.0f, 1.4f);
            }

            if (currText != null)
                currText.transform.position -= currText.transform.forward * 0.01f;

            if (currCount == 0)
            {
                inGame = true;
                sounds.playEngine();
                one.gameObject.SetActive(false);
                currCount--;
            }
        }
    }


    void setNext(Transform point)
    {
        point.GetComponent<CollisionHandler>().isNext = true;
        point.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
    }

    void handleCollected(Transform point)
    {
        point.gameObject.SetActive(false);
    }

    public void loadData(string filename = "sample.txt")
    {
        string[] lines = File.ReadAllLines(filename);
        foreach (string line in lines)
        {
            string[] tokens = line.Split(' ');
            if (tokens.Length != 3)
                tokens = line.Split('\t');
            float x = float.Parse(tokens[0]) * 0.0254f;
            float y = float.Parse(tokens[1]) * 0.0254f;
            float z = float.Parse(tokens[2]) * 0.0254f;

            Transform point = Instantiate(pointPrefab, new Vector3(x, y, z), new Quaternion());

            points.Add(point);

            if (points.Count > 1)
            {
                GameObject myLine = new GameObject();
                myLine.name = "line";
                myLine.AddComponent<LineRenderer>();
                LineRenderer lr = myLine.GetComponent<LineRenderer>();
                lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
                Color color = new Color(0.4f, 0.4f, 0.4f);
                lr.startWidth = 1f;
                lr.endWidth = 1f;

                drawLine(points[points.Count - 2].position, point.position, myLine, color);
                paths.Add(myLine);
            }
        }

        numPoints = points.Count;
        setNext(points[0]);

        ship.transform.position = points[0].position;
        ship.LookAt(points[1]);
        ship.forward = new Vector3(ship.forward.x, 0, ship.forward.z);

        cam.forward = ship.forward;
        Vector3 camPos;
        camPos = ship.position - ship.forward * 0.75f;
        camPos.y += 0.25f;
        cam.position = camPos;

        cam.position = ship.position;
        

        highlightFirstPath();
    }

    void drawLine(Vector3 start, Vector3 end, GameObject line, Color color)
    {
        line.transform.position = start;
        LineRenderer lr = line.GetComponent<LineRenderer>();
        lr.startColor = color;
        lr.endColor = color;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    void popAndUpdatePath()
    {
        Destroy(paths[0]);
        paths.RemoveAt(0);
        highlightFirstPath();
        displayText("Checkpoint reached!");
    }

    void highlightFirstPath()
    {
        paths[0].GetComponent<LineRenderer>().startColor = new Color(1f, 1f, 0f);
        paths[0].GetComponent<LineRenderer>().endColor = new Color(0f, 1f, 0f);
    }

    public Transform restartCountdown()
    {
        displayText("You crashed!");
        countdown = 6.0f;
        currCount = 6;
        inGame = false;
        sounds.stop();

        return points[next - 1];
    }

    void displayText(string text)
    {
        messageText.text = text;
        messageDisplay = 2.0f;
    }
}
