using System.Collections;
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
    public GameObject gameoverScreen;

    public enum Stages { Menu, Pregame, Transition, Game, GameOver, GameWin };
    public Stages stage = Stages.Menu; // Which section of game we're in.

    public enum Difficulty { Easy, Medium, Hard };
    public Difficulty difficulty = Difficulty.Medium; // Determines the hints given

    public List<GameObject> sparkSearch;

    public float enemySpeed = 1.0f;
    public float textCooldown = 0.0f;

    private List<NavMeshAgent> enemies;
    public float elapsedTime = 0.0f;
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

        if (stage == Stages.GameWin)
        {
            gameWinLoop();
        }
    }

    /**
     * Loop for lights on
     * */
    private void pregameLoop()
    {
        // First-time setup
        if (enteringLoop)
        {
            ambient.Play();
            museum.Play();
            enteringLoop = false;

            // Display text at first
            if (difficulty == Difficulty.Easy)
                this.setText("Find the flashlight on a shelf");
            else if (difficulty == Difficulty.Medium)
                this.setText("Explore the museum and find the flashlight");
            else if (difficulty == Difficulty.Hard)
                this.setText("Explore the museum");
        }

        // Text display time
        if (txt.text != "")
        {
            textCooldown += Time.deltaTime;
            if (textCooldown > 7.0f)
            {
                textCooldown = 0;
                txt.text = "";
            }
        }

        if (elapsedTime > 180) // Let's say 2 minutes for now?
        {
            stage = Stages.Transition;
            elapsedTime = 0;


            GameObject[] lights = GameObject.FindGameObjectsWithTag("light");
            foreach (GameObject obj in lights)
            {
                if (obj.GetComponents<LightFlicker>().Length > 0)
                    obj.GetComponent<LightFlicker>().doFlicker = true;
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
        // Text display time
        if (txt.text != "")
        {
            textCooldown += Time.deltaTime;
            if (textCooldown > 7.0f)
            {
                textCooldown = 0;
                txt.text = "";
            }
        }

        // 10 seconds of light flashing seems fine
        if (elapsedTime > 10)
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
                if (obj.GetComponents<LightFlicker>().Length > 0)
                    obj.GetComponent<LightFlicker>().doFlicker = false;
                obj.GetComponent<Light>().enabled = false;
            }

            setText("Uh oh, looks like the lights shorted. Find something to fix the lights.");

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
            //GameObject[] objs = GameObject.FindGameObjectsWithTag("sparkSearch");
            foreach (GameObject search in sparkSearch)
            {
                Debug.Log("Activate " + search.tag + " " + search.name);
                search.SetActive(true);
            }
            enteringLoop = false;
        }

        // Text display time
        if (txt.text != "")
        {
            textCooldown += Time.deltaTime;
            if (textCooldown > 7.0f)
            {
                textCooldown = 0;
                txt.text = "";
            }
        }

        
        // Make enemies follow player
        foreach (NavMeshAgent agent in enemies)
        {
            if (agent.gameObject.GetComponent<EnemyStuff>().canMove())
            {
                agent.destination = goal.position;
                agent.Resume();

                // Only if the monster can move should it be able to kill you
                if (Vector3.Distance(agent.transform.position, goal.position) < 2.0f) // TODO: not sure how much needed
                {
                    stage = Stages.GameOver;
                    elapsedTime = 0;

                    gameoverScreen.SetActive(true);

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

    private void gameWinLoop()
    {
        GameObject[] lights = GameObject.FindGameObjectsWithTag("light");
        foreach (GameObject obj in lights)
        {
            if (obj.GetComponents<LightFlicker>().Length > 0)
            {
                obj.GetComponent<LightFlicker>().doFlicker = false;
                obj.GetComponent<LightFlicker>().alwaysFlicker = false;
            }

            obj.GetComponent<Light>().enabled = true;

            setText("You win ayyyy lmao");
        }

        foreach (NavMeshAgent agent in enemies)
            agent.Stop();
    }

    public void setText(string text)
    {
        txt.text = text;
        textCooldown = 0;
    }
}
