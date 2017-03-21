using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerEnter : MonoBehaviour
{
    public GameFlow gameFlow;
    public string easyHint = "Hmm... it's not here...";
    public string mediumHint = "This doesn't seem like a likely spot.";
    public string hardHint = "Could it be here?";

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "player")
        {
            if (gameFlow.difficulty == GameFlow.Difficulty.Easy)
                gameFlow.setText(easyHint);
            else if (gameFlow.difficulty == GameFlow.Difficulty.Medium)
                gameFlow.setText(mediumHint);
            else if (gameFlow.difficulty == GameFlow.Difficulty.Hard)
                gameFlow.setText(hardHint);

            gameObject.SetActive(false);
        }
    }
}
