using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameFlow gameFlow;
    public Transform player;
    //public OVRInput.Controller left;
    //public OVRInput.Controller right;

    public Transform l;
    public Transform r;

    public GameObject startButton;
    public GameObject openingMenu;
    public GameObject optionsMenu;
    public GameObject difficultyMenu;
    public Slider slider;

    enum Screen { Initial, Start, Options };
    private Screen screen = Screen.Initial;

    private float elapsedTime = 0.0f;
    private int flashCd = 0;
    private float textAlpha = 1000;
    private Vector3 initialHandle;
    private bool sliding = false;

    // Use this for initialization
    void Start() {
        slider.minValue = 1;
        slider.maxValue = 2;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (screen == Screen.Initial)
        {
            this.initialLoop();
        }

        if (screen == Screen.Start)
        {
            this.startLoop();
        }

        if (screen == Screen.Options)
        {
            this.optionLoop();
        }
    }

    /**
     * Initial loop
     * */
    private void initialLoop()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            Ray ray = new Ray(r.transform.position, r.transform.forward);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
            {
                GameObject obj = rayHit.transform.gameObject;
                if (obj.tag == "sorryClosed")
                {
                    obj.GetComponent<Rigidbody>().useGravity = true;
                    screen = Screen.Start;
                    elapsedTime = 0.0f;
                    Debug.Log("Enter menu start loop");
                }
            }
        }
    }

    /**
     * Start loop
     * */
    private void startLoop()
    {
        if (elapsedTime > 3.0f)
        {
            startButton.SetActive(true);
            if (flashCd == 0)
                startButton.transform.GetChild(1).gameObject.SetActive(!startButton.transform.GetChild(1).gameObject.activeSelf);
            flashCd++;
            if (flashCd > 100)
                flashCd = 0;

            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            {
                Ray ray = new Ray(r.transform.position, r.transform.forward);
                RaycastHit rayHit;

                if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
                {
                    GameObject obj = rayHit.transform.gameObject;
                    if (obj.tag == "startButton")
                    {
                        openingMenu.SetActive(false);
                        optionsMenu.SetActive(true);
                        screen = Screen.Options;
                        elapsedTime = 0.0f;
                        Debug.Log("Enter menu option");
                    }
                }
            }
        }

    }

    /**
     * Option loop
     * */
    private void optionLoop()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            Ray ray = new Ray(r.transform.position, r.transform.forward);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
            {
                GameObject obj = rayHit.transform.gameObject;
                if (obj.tag == "difficultyDisplay")
                {
                    obj.SetActive(false);
                    difficultyMenu.transform.GetChild(1).gameObject.SetActive(true);
                    difficultyMenu.transform.GetChild(2).gameObject.SetActive(true);
                    difficultyMenu.transform.GetChild(3).gameObject.SetActive(true);
                }
                if (obj.tag == "difficultyChoice")
                {
                    difficultyMenu.transform.GetChild(0).gameObject.SetActive(true);
                    difficultyMenu.transform.GetChild(0).gameObject.GetComponent<Text>().text = obj.GetComponent<Text>().text;
                    difficultyMenu.transform.GetChild(1).gameObject.SetActive(false);
                    difficultyMenu.transform.GetChild(2).gameObject.SetActive(false);
                    difficultyMenu.transform.GetChild(3).gameObject.SetActive(false);
                }
                if (obj.tag == "startButton")
                {
                    player.position = new Vector3(194, 1, 329);
                    gameFlow.enemySpeed = slider.value;
                    if (difficultyMenu.transform.GetChild(0).gameObject.GetComponent<Text>().text == "Easy")
                        gameFlow.difficulty = GameFlow.Difficulty.Easy;
                    else if (difficultyMenu.transform.GetChild(0).gameObject.GetComponent<Text>().text == "Medium")
                        gameFlow.difficulty = GameFlow.Difficulty.Medium;
                    else if (difficultyMenu.transform.GetChild(0).gameObject.GetComponent<Text>().text == "Hard")
                        gameFlow.difficulty = GameFlow.Difficulty.Hard;
                    gameFlow.stage = GameFlow.Stages.Pregame;
                }
                if (obj.tag == "sliderHandle")
                {
                    // TODO: manipulation
                    initialHandle = rayHit.point;
                    sliding = true;
                    Debug.Log("Grab handle");
                }
            }
        }

        if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.0f && sliding)
        {
            Ray ray = new Ray(r.transform.position, r.transform.forward);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
            {
                GameObject obj = rayHit.transform.gameObject;
                if (obj.tag == "sliderHandle" || obj.tag == "sliderBackboard")
                {
                    // TODO: manipulation
                    Vector3 difference = rayHit.point - initialHandle;
                    initialHandle = rayHit.point;
                    slider.value += -difference.z * 2.2f;
                }
            }
        }
        else
            sliding = false;

        slider.transform.GetChild(0).GetComponent<Text>().text = "" + slider.value;
    }
}
