using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStuff : MonoBehaviour
{
    public Transform flashlight;
    public GameObject meshRenderer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /**
     * Checks if a navigator is in spotlight and in player's view
     * */
    public bool canMove()
    {
        // If light is on it
        // TODO: need better than just this single ray, maybe better way is an invisible
        // cone object that we check collisions with? + also a raycast in case of behind walls
        Ray ray = new Ray(flashlight.position, flashlight.forward);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
        {
            if (rayHit.transform.gameObject.tag == "enemy")
            {
                // Check that it is within view
                /*if (meshRenderer != null && meshRenderer.GetComponent<SkinnedMeshRenderer>().isVisible)
                {
                    return false;
                }*/
            }
        }
        return true;
    }
}
