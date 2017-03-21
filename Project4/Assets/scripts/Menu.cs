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

    enum Screen { Initial, Start, Options };
    private Screen screen = Screen.Initial;

    private float elapsedTime = 0.0f;
    private int flashCd = 0;
    private float textAlpha = 1000;

    // Use this for initialization
    void Start() {

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
                if (obj.tag == "sliderHandle")
                {
                    // TODO: manipulation
                }
                if (obj.tag == "startButton")
                {
                    player.position = new Vector3(194, 1, 329);
                    gameFlow.stage = GameFlow.Stages.Pregame;
                }
            }
        }
    }
}
