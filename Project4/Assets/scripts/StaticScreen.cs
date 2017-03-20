using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticScreen : MonoBehaviour
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        float rndXoffset = Random.value;
        float rndYoffset = Random.value;

        this.gameObject.GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(rndXoffset, rndYoffset);
    }
}
