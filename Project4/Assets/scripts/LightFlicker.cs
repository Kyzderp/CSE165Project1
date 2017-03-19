using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour {
    float minFlickerSpeed = 0.001f;
    float maxFlickerSpeed = 0.12f;

    int minFlickerAgain = 2;
    int maxFlickerAgain = 9;


    float minDownTime = 2.0f;
    float maxDownTime = 6.0f;

    private Light thisLight;
    public bool doFlicker = false;

    // Use this for initialization
    void Start() {
        thisLight = GetComponent<Light>();
        StartCoroutine(flicker());
    }


    void Update()
    {
        
    }

    // can only wait inside a coroutine
    IEnumerator flicker()
    {
        while (true)
        {
            //Debug.Log(this.name + " doFlicker: " + doFlicker);

            if (!doFlicker)
                yield return new WaitForSeconds(0.5f);

            if (doFlicker)
            {
                //thisLight.enabled = false;

                yield return new WaitForSeconds(Random.Range(minDownTime, maxDownTime));

                int flickerNum = Random.Range(minFlickerAgain, maxFlickerAgain);

                for (int i = 0; i < flickerNum; i++)
                {
                    thisLight.enabled = true;
                    yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));
                    thisLight.enabled = false;
                    yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));
                }
            }
        }
    }
}
