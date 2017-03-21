using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStuff : MonoBehaviour
{
    public GameFlow gameflow;
    public Transform flashlight;
    public GameObject meshRenderer;
    public Transform character;

    private bool hasSeen = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Check if player is in line of sight to activate it
        if (gameflow.stage == GameFlow.Stages.Game && !this.hasSeen)
        {
            RaycastHit hit;
            Vector3 dir = character.position - (transform.position + transform.forward * 1.5f);
            if (Physics.Raycast(transform.position + transform.forward * 1.5f, dir, out hit))
            {
                if (hit.collider.tag == "Player")
                {
                    Debug.Log(name + " saw the player");
                    this.hasSeen = true;
                }
            }
        }
    }

    /**
     * Checks if a navigator is in spotlight and in player's view
     * */
    public bool canMove()
    {
        if (!this.hasSeen)
            return false;

        // Check that it is within view
        if (meshRenderer != null && meshRenderer.GetComponent<SkinnedMeshRenderer>().isVisible)
        {
            RaycastHit hit;
            Vector3 dir = character.position - (transform.position + transform.forward * 1.5f);
            if (Physics.Raycast(transform.position + transform.forward * 1.5f, dir, out hit))
            {
                if (hit.collider.tag == "Player")
                {
                    return false;
                }
            }
        }

        // If light is on it
        // TODO: need better than just this single ray, maybe better way is an invisible
        // cone object that we check collisions with? + also a raycast in case of behind walls
        /*Ray ray = new Ray(flashlight.position, -flashlight.right);
        Debug.DrawRay(flashlight.position, -flashlight.right * 20.0f);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
        {
            Debug.Log(rayHit.transform.gameObject.tag + " " + rayHit.transform.gameObject.name);
            if (rayHit.transform.gameObject.tag == "enemy" && rayHit.transform.gameObject == this.gameObject)
            {
                return false;
                
            }
        }*/
        return true;
    }
}
