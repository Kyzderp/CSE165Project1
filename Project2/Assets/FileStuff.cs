using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileStuff : MonoBehaviour {

    public Transform whiteboardPrefab;
    public Transform lockerPrefab;
    public Transform deskPrefab;
    public Transform chairPrefab;
    public Transform cabinetPrefab;
    public Transform treeDTVPrefab;

    private StreamWriter file;

	// Use this for initialization
	void Start () {
        file = new StreamWriter("data.txt");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void saveData()
    {
        string[] types = {"whiteboard", "locker", "desk", "chair", "cabinet", "3DTV"};
        foreach (string type in types)
            saveType(type);

        file.Close();
    }

    void saveType(string type)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(type);
        foreach (GameObject obj in objs)
        {
            file.WriteLine(type + "$" + obj.name
                + "$" + obj.transform.position.x
                + "$" + obj.transform.position.y
                + "$" + obj.transform.position.z
                + "$" + obj.transform.rotation.x
                + "$" + obj.transform.rotation.y
                + "$" + obj.transform.rotation.z
                + "$" + obj.transform.rotation.w);
        }
    }

    void loadData()
    {
        string[] lines = File.ReadAllLines("data.txt");
        foreach (string line in lines)
        {
            string[] tokens = line.Split('$');
            spawnObject(tokens);

        }
    }

    void spawnObject(string[] tokens)
    {
        Transform obj = Instantiate(chairPrefab);

        Vector3 pos = new Vector3(float.Parse(tokens[2]), float.Parse(tokens[3]), float.Parse(tokens[4]));
        Quaternion quat = new Quaternion(float.Parse(tokens[5]), float.Parse(tokens[6]), float.Parse(tokens[7]), float.Parse(tokens[8]));

        switch (tokens[0])
        {
            case "whiteboard": obj = Instantiate(whiteboardPrefab, pos, quat); break;
            case "locker": obj = Instantiate(lockerPrefab, pos, quat); break;
            case "desk": obj = Instantiate(deskPrefab, pos, quat); break;
            case "chair": obj = Instantiate(chairPrefab, pos, quat); break;
            case "cabinet": obj = Instantiate(cabinetPrefab, pos, quat); break;
            case "3DTV": obj = Instantiate(treeDTVPrefab, pos, quat); break;
            default: break;
        }

        obj.name = tokens[1];
    }
}
