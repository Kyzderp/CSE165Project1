using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickInstantiation : MonoBehaviour {

    public Transform brickPrefab;
    MeshRenderer thisRenderer;

    public List<GameObject> bricks = new List<GameObject>();

    int numberOfObjects = 32;
    float radius = 4f;
    int cylinderHeight = 17;

    float brickOffset = 0.75f; // pretty random value... trying to offset the next layer by half a brick
                               // but this is basically spinning the circle by this many radians

    public void createCylinder()
    {
        for (int yi = 0; yi < cylinderHeight; yi++)
        {
            for (int i = 0; i < numberOfObjects; i++)
            {
                float angle = (i * Mathf.PI * 2 / numberOfObjects) + brickOffset;
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;
                float y = 0.15f + (0.3f * yi);

                Vector3 pos = new Vector3(x, y, z);
                Transform brick = Instantiate(brickPrefab, pos, Quaternion.identity);

                brick.Rotate(new Vector3(0, -1 * Mathf.Rad2Deg * angle, 0));
                brick.GetComponent<MeshRenderer>().material.SetColor("_Color", GetBrickColor());

                bricks.Add(brick.gameObject);
            }

            brickOffset = -1 * brickOffset;
        }
    }

    void Start()
    {
        createCylinder();
    }

    Color GetBrickColor()
    {
        switch(Random.Range(1, 4))
        {
            case 1:
                return new Color(0.5955882f, 0.1886035f, 0.09196584f) + new Color(0.2f, 0.2f, 0.2f);
            case 2:
                return new Color(0.6176471f, 0.3093938f, 0.1725779f) + new Color(0.2f, 0.2f, 0.2f);
            case 3:
                return new Color(0.625f, 0.4684497f, 0.3860294f) + new Color(0.2f, 0.2f, 0.2f);
            default:
                return new Color(0.5955882f, 0.1886035f, 0.09196584f);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
