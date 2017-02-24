using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PointLoader : MonoBehaviour 
{
	public Transform pointPrefab;
    List<Transform> points; // list of points
    int next = 0; // next point you need to collect
    int numPoints;
    bool inGame = false; // True if the game is ongoing, false otherwise

	// Use this for initialization
	void Start () {
        points = new List<Transform>();

		loadData("sample.txt");

        inGame = true;
	}
	
	// Update is called once per frame
	void Update () {
        while(inGame)
        {
            if (points[next].GetComponent<CollisionHandler>().collected == false)
            {
                // player has not collected next point
                continue; // keep waiting for collision
            } else
            {
                // else player collected the next point
                next++;

                setNext(points[next]);
            }
        }
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
        }

        numPoints = points.Count;
        setNext(points[0]);
    }

    void setNext(Transform point)
    {
        point.GetComponent<CollisionHandler>().isNext = true;
        point.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
    }
}
