using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameFlow : MonoBehaviour
{
    public Transform goal;
    public Transform flashlight;

    enum Stages { Pregame, Transition, Game, GameOver };
    private Stages stage; // Which section of game we're in.

    private List<NavMeshAgent> enemies;
    private float elapsedTime = 0.0f;

	/**
     * Use this for initialization
     * */
	void Start ()
    {
        enemies = new List<NavMeshAgent>();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject obj in objs)
        {
            enemies.Add(obj.GetComponent<NavMeshAgent>());
        }
        // TODO: spawn the flashlight somewhere random so it's at least more fun for us
	}

    /**
     * Update is called once per frame
     * */
    void Update()
    {
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
        if (elapsedTime > 5) // Let's say 2 minutes for now?
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
                    obj.GetComponent<LightFlicker>().doFlicker = true;
            }

            Debug.Log("Go into transition phase");

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
        if (elapsedTime > 10)
        {
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

            Debug.Log("Go into main loop");

            this.mainGameLoop();
            return;
        }
    }

    /**
     * Loop for lights off
     * */
    private void mainGameLoop()
    {
        // Make enemies follow player
        foreach (NavMeshAgent agent in enemies)
        {
            if (agent.gameObject.GetComponent<EnemyStuff>().canMove())
            {
                agent.destination = goal.position;
                // TODO: and if we want, could do the animation at this time too

                // Only if the monster can move should it be able to kill you
                if (Vector3.Distance(agent.transform.position, goal.position) < 2.0f) // TODO: not sure how much needed
                {
                    stage = Stages.GameOver;
                    elapsedTime = 0;
                    this.gameoverLoop();
                    return;
                }
            }
        }
    }

    /**
     * Checks if a navigator is in spotlight and in player's view
     * */
    /*private bool canMove(NavMeshAgent agent)
    {
        // If light is on it
        // TODO: need better than just this single ray, maybe better way is an invisible
        // cone object that we check collisions with? + also a raycast in case of behind walls
        Ray ray = new Ray(flashlight.position, flashlight.forward);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
        {
            // Check that it is within view
            MeshRenderer rend = agent.gameObject.GetComponent<MeshRenderer>();
            if (rend.isVisible)
            {
                return false;
            }
        }
        return true;
    }*/

    /**
     * Loop for game over
     * */
    private void gameoverLoop()
    {
        // TODO: wat do
    }
}
