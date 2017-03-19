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

    Light light;

    // Use this for initialization
    void Start() {
        light = GetComponent<Light>();
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
            light.enabled = false;
            yield return new WaitForSeconds(Random.Range(minDownTime, maxDownTime));

            int flickerNum = Random.Range(minFlickerAgain, maxFlickerAgain);

            for(int i = 0; i < flickerNum; i++)
            {
                light.enabled = true;
                yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));
                light.enabled = false;
                yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));
            }
        }
    }
}
