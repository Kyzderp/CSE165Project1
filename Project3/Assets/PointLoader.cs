﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PointLoader : MonoBehaviour 
{
	public Transform pointPrefab;

	// Use this for initialization
	void Start () {
		loadData("sample.txt");
	}
	
	// Update is called once per frame
	void Update () {
		
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
            float z = float.Parse(tokens[1]) * 0.0254f;

            Transform point = Instantiate(pointPrefab, new Vector3(x, y, z), new Quaternion());
        }
    }
}
