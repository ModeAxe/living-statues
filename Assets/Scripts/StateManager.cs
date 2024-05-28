using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static string CURRENTSTATE = "Idle";
    private string PREVIOUSSTATE;

    public static Dictionary<string, string> STATES = new Dictionary<string, string>()
    {
        { "Static", "Static" },
        { "Welcome", "Welcome"},
        { "Hero", "Hero"},
        { "FollowLeft", "FollowLeft"},
        { "FollowRight", "FollowRight" },
        { "Mimic", "Mimic"},
        { "Invisible", "Invisible"}
    };

    private List<string> statesList = new List<string> { "Static", "Welcome", "Hero", "FollowLeft", "FollowRight", "Mimic", "Invisible" };

    public List<GameObject> characters = new List<GameObject>();


    //Timers
    public float TimeInverval = 5.0f;
    private float timer;

    private float timeSinceStateChange;

    public float MaxStaticTime = 60f;





    void Start()
    {
        //initialize timers
        timer = Time.deltaTime;
        timeSinceStateChange = Time.deltaTime;
        
    }

    // Update is called once per frame
    void Update()
    {
        //check current conditions


        //update state
        if (timer > TimeInverval)
        {
            var newState = statesList[(int)Random.Range(0, statesList.Count)];
            SetState(newState);
            timer = 0f;
        }


        //update timers
        timer += Time.deltaTime;

    }

    public void SetState(string state)
    {
        try
        {
            PREVIOUSSTATE = CURRENTSTATE;
            CURRENTSTATE = STATES[state];

            Debug.Log("Current State: " +  CURRENTSTATE);

            foreach (GameObject character in characters)
            {
                character.BroadcastMessage("SetState", state);
            }
        }
        catch
        {
            CURRENTSTATE = STATES["Static"];
            Debug.Log("SetState to " + state + " failed. Defaulting to static state");
        }
    }

}
