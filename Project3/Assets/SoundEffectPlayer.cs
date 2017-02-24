using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour {

    public AudioClip clapping;
    public AudioClip cheering;
    public AudioClip engine;
    public AudioClip lucio;
    public AudioClip burst;

    public AudioSource source;

	// Use this for initialization
	void Start () {
    }

    // Update is called once per frame
    void Update () {

	}

    public void playEngine()
    {
        source.PlayOneShot(engine, 0.1f);
    }

    public void playWin()
    {
        source.clip = clapping;
        source.volume = 2.0f;
        source.PlayDelayed(2.4f);
        source.PlayOneShot(burst, 0.5f);
        source.PlayOneShot(cheering, 0.6f);
        source.PlayOneShot(clapping, 2.0f);
    }

    public void playCountDown()
    {
        source.PlayOneShot(lucio, 1.0f);
    }

    public void playBurst()
    {
        source.PlayOneShot(burst, 0.5f);
    }

    public void stop()
    {
        source.Stop();
    }
}
