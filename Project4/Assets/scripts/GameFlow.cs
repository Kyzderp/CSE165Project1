﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GameFlow : MonoBehaviour
{
    public Transform goal;
    public Transform flashlight;
    public GameObject textobj;
    Text txt;
    public AudioSource ambient;
    public AudioSource piano;
    public AudioSource sparks;
    public AudioSource museum;


    public enum Stages { Menu, Pregame, Transition, Game, GameOver };
    public Stages stage = Stages.Menu; // Which section of game we're in.

    public enum Difficulty { Easy, Medium, Hard };
    public Difficulty difficulty = Difficulty.Medium; // Determines the hints given

    public float enemySpeed = 1.0f;

    private List<NavMeshAgent> enemies;
    private float elapsedTime = 0.0f;
    private bool enteringLoop = true;

	/**
     * Use this for initialization
     * */
	void Start ()
    {
        txt = textobj.GetComponent<Text>();
        enemies = new List<NavMeshAgent>();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject obj in objs)
        {
            enemies.Add(obj.GetComponent<NavMeshAgent>());
            obj.GetComponent<NavMeshAgent>().speed *= enemySpeed;
        }
        // TODO: spawn the flashlight somewhere random so it's at least more fun for us
	}

    /**
     * Update is called once per frame
     * */
    void Update()
    {
        if (stage == Stages.Menu)
            return;

        elapsedTime += Time.deltaTime;

        // Lights on
        if (stage == Stages.Pregame)
        {
            pregameLoop();
        }

        // Transition
        if (stage == Stages.Transition)
        {
            transitionLoop();
        }

        // Lights off
        if (stage == Stages.Game)
        {
            mainGameLoop();
        }

        // Game over
        if (stage == Stages.GameOver)
        {
            gameoverLoop();
        }
    }

    /**
     * Loop for lights on
     * */
    private void pregameLoop()
    {
        if (enteringLoop)
        {
            ambient.Play();
            museum.Play();
            enteringLoop = false;
        }
        if (elapsedTime < 7)
        {
            txt.text = "Explore the museum";
        } else
        {
            txt.text = "";
        }
        if (elapsedTime > 1200) // Let's say 2 minutes for now?
        {
            stage = Stages.Transition;
            elapsedTime = 0;


            GameObject[] lights = GameObject.FindGameObjectsWithTag("light");
            foreach (GameObject obj in lights)
            {
                /*Light light = obj.GetComponent<Light>();
                if (Random.value > 0.98f)
                {
                    // toggle light with 0.1 chance?
                    light.enabled = !light.enabled;
                }*/
                if (obj.GetComponents<LightFlicker>().Length > 0)
                {
                    obj.GetComponent<LightFlicker>().doFlicker = true;
                }
            }

            Debug.Log("Go into transition phase");

            enteringLoop = true;
            this.transitionLoop();
            return;
        }

        // Stuff like...? if found flashlight, do something?
    }

    /**
     * Loop for transition phase
     * */
    private void transitionLoop()
    {

        // 10 seconds of light flashing seems fine
        if (elapsedTime > 3)
        {
            if (enteringLoop)
            {
                sparks.Play();
                enteringLoop = false;
            }

            stage = Stages.Game;
            elapsedTime = 0;

            // Stop the flicker
            GameObject[] lights = GameObject.FindGameObjectsWithTag("light");
            foreach (GameObject obj in lights)
            {
                /*Light light = obj.GetComponent<Light>();
                if (Random.value > 0.98f)
                {
                    // toggle light with 0.1 chance?
                    light.enabled = !light.enabled;
                }*/
                if (obj.GetComponents<LightFlicker>().Length > 0)
                    obj.GetComponent<LightFlicker>().doFlicker = false;
                obj.GetComponent<Light>().enabled = false;
            }

            // TODO: spawn the weapons?
            GameObject[] weapons = GameObject.FindGameObjectsWithTag("gun");
            foreach (GameObject obj in weapons)
            {
                obj.SetActive(true);
            }

            Debug.Log("Go into main loop");

            enteringLoop = true;
            this.mainGameLoop();
            return;
        }
    }

    /**
     * Loop for lights off
     * */
    private void mainGameLoop()
    {
        if (enteringLoop)
        {
            piano.Play();
            enteringLoop = false;
        }
        // Make enemies follow player
        foreach (NavMeshAgent agent in enemies)
        {
            if (agent.gameObject.GetComponent<EnemyStuff>().canMove())
            {
                agent.destination = goal.position;
                agent.Resume();
                // TODO: and if we want, could do the animation at this time too

                // Only if the monster can move should it be able to kill you
                if (Vector3.Distance(agent.transform.position, goal.position) < 2.0f) // TODO: not sure how much needed
                {
                    stage = Stages.GameOver;
                    elapsedTime = 0;

                    // TODO: Teleport them to the gameover box

                    enteringLoop = true;
                    this.gameoverLoop();
                    return;
                }
            }
            else
            {
                agent.Stop();
            }
        }
    }

    /**
     * Loop for game over
     * */
    private void gameoverLoop()
    {
        // TODO: wat do
    }
}
