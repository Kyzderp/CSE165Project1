using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeSystem : MonoBehaviour {

    public Transform cannonballPrefab;
    public GameObject weapon;
    public GameObject brickInstantiator;
    List<GameObject> cannonBalls = new List<GameObject>();

    public const int NONE = 0;
    public const int LASER = 1;
    public const int CANNON = 2;

    const float dwellTime = 1.0f;
    const float cannonInterval = 3.0f;
    
    // Use this for initialization
    void Start () {
		
	}

    float timer = dwellTime;
    float cannonTimer = cannonInterval;
    static GameObject curBrick;
    static GameObject prevBrick;
    static int mode = NONE;
	
	// Update is called once per frame
	void Update () {
        Ray gaze = new Ray(transform.position, transform.forward);
        // cant figure out how to get a lazer to show up...
        //Vector3 v = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z);
        //Debug.DrawRay(transform.position, transform.forward * 10.0f, Color.green);
        RaycastHit rayHit;

        if (Physics.Raycast(gaze, out rayHit, Mathf.Infinity))
        {
            // Debug.LogFormat("You hit {0}!", rayHit.collider.name);

            if (rayHit.transform.gameObject.tag == "brick" && mode == LASER)
            {
                prevBrick = curBrick;
                curBrick = rayHit.transform.gameObject;

                if (curBrick == prevBrick)
                    timer -= Time.deltaTime;
                else
                    timer = dwellTime;

                if (timer < 0f)
                {
                    Debug.Log("destroy brick");
                    Destroy(rayHit.transform.gameObject);
                }
            }
            else if (rayHit.transform.gameObject.tag == "laser")
            {
                timer -= Time.deltaTime;

                if (timer < 0f)
                {
                    Debug.Log("Laser Selected");
                    weapon.GetComponent<TextMesh>().text = "Laser";

                    mode = LASER;
                }
            }
            else if (rayHit.transform.gameObject.tag == "cannon")
            {
                timer -= Time.deltaTime;

                if (timer < 0f)
                {
                    Debug.Log("Cannon Selected");
                    weapon.GetComponent<TextMesh>().text = "Cannon";

                    mode = CANNON;
                }
            }
            else if (rayHit.transform.gameObject.tag == "reset" && mode != NONE)
            {
                timer -= Time.deltaTime;

                if (timer < 0f)
                {
                    Debug.Log("Reset");
                    weapon.GetComponent<TextMesh>().text = "";
                    mode = NONE;

                    BrickInstantiation BIscript = brickInstantiator.GetComponent<BrickInstantiation>();
                    List<GameObject> bricks = BIscript.bricks;
                    
                    foreach (var brick in bricks) {
                        Destroy(brick);
                    }

                    foreach (var ball in cannonBalls)
                    {
                        Destroy(ball);
                    }

                    BIscript.createCylinder();

                }
            }
            else
            {
                timer = dwellTime;
            }
        }

        // Fire cannon periodically if in cannon mode
        if (mode == CANNON)
        {
            cannonTimer -= Time.deltaTime;
            if (cannonTimer < 0f)
            {
                this.fireCannonball();
                cannonTimer = cannonInterval;
            }
        }
    }

    void fireCannonball()
    {
        Transform ball = Instantiate(cannonballPrefab, transform.position, Quaternion.identity);
        ball.GetComponent<Rigidbody>().velocity = transform.forward * 17.0f;
        cannonBalls.Add(ball.gameObject);
    }
}
