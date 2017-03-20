using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public OVRInput.Controller left;
    public OVRInput.Controller right;

    enum Screen { Initial, Start, Options };
    private Screen screen = Screen.Initial;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (screen == Screen.Initial)
        {
            // TODO: if clicked
            screen = Screen.Start;
        }

        if (screen == Screen.Start)
        {
            // TODO: if clicked
            screen = Screen.Options;
        }

        if (screen == Screen.Options)
        {

        }
	}
}
