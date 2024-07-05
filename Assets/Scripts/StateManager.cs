using com.rfilkov.kinect;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static com.rfilkov.kinect.KinectInterop;

public class StateManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static string CURRENTSTATE = "Static";
    private string PREVIOUSSTATE;

    public bool ManualOverride = false;
    public int OverrideStateIndex = 0;

    public static Dictionary<string, string> STATES = new Dictionary<string, string>()
    {
        { "Static", "Static" },
        { "Welcome", "Welcome"},
        { "Hero", "Hero"},
        { "FollowLeft", "FollowLeft"},
        { "FollowRight", "FollowRight" },
        { "Mimic", "Mimic"},
        { "Invisible", "Invisible"},
        { "Wildcard", "Wildcard"},
        { "Loaded", "Loaded"}
    };

    private List<string> statesList = new List<string> { "Static", "Welcome", "Hero", "FollowLeft", "FollowRight", "Mimic", "Invisible", "Wilcard", "Loaded" };
    //private List<string> statesList = new List<string> { "Static", "Welcome", "Mimic" };

    public List<GameObject> characters = new List<GameObject>();

    public GameObject WelcomeProp; 


    //Timers
    public float TimeInverval = 5.0f;
    private float timer = 0;

    private float timeSinceStateChange;

    [Header("Sta, Wel, Hero, FLeft, FRight, Mim, Inv, Wild, Load")]
    public int[] MaxTimes = new int[9];
    public int[] MinTimes = new int[9];

    // primary kinect sensor data structure
    private KinectInterop.SensorData sensorData = null;
    private KinectManager kinectManager = null;
    public int sensorIndex = 0;

    private bool isRecording = false;
    private float recordingTimer = 0;
    private GameObject currentRecordingCharacter;

    void Start()
    {
        //initialize timers
        timer = Time.deltaTime;
        timeSinceStateChange = Time.deltaTime;

        SetState(statesList[0]);


        //Get Sensor Data
        try
        {
            // get sensor data
            kinectManager = KinectManager.Instance;
            if (kinectManager && kinectManager.IsInitialized())
            {
                sensorData = kinectManager.GetSensorData(sensorIndex);
            }

            if (sensorData == null || sensorData.sensorInterface == null)
            {
                throw new Exception("KinectManager is missing or not initialized.");
            }
        }
        catch (DllNotFoundException ex)
        {
            Debug.LogError(ex.ToString());
            //if (debugText != null)
            //    debugText.text = "Please check the SDK installations.";
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            //if (debugText != null)
            //    debugText.text = ex.Message;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //check current conditions
        //if (CURRENTSTATE == "Mimic" && kinectManager.NumberOfTrackedUsers() == 0)
        //{
        //    int WelcomeIndex = 1;
        //    TimeInverval = UnityEngine.Random.Range(MinTimes[WelcomeIndex], MaxTimes[WelcomeIndex]);
        //    SetState(STATES["Welcome"]);
        //}

        //update state
        if (timer > TimeInverval)
        {
            int stateIndex = ManualOverride ? OverrideStateIndex : (int)UnityEngine.Random.Range(0, statesList.Count);
            var newState = statesList[stateIndex];
            timer = 0f;
            TimeInverval = UnityEngine.Random.Range(MinTimes[stateIndex], MaxTimes[stateIndex]);
            SetState(newState);
        }

        if (CURRENTSTATE == "Mimic" && timer > 10 && timer < 30 && !isRecording)
        {
            //Get Active Object
            //currentRecordingCharacter = SwitchModel.GetActiveCharacter().transform.GetChild(0).gameObject;
            //Start Recording
            //currentRecordingCharacter.GetComponent<CharacterController>().GetObjectToRecord().GetComponent<UnityAnimationRecorder>().StartRecording();
            //Start Recording Timer
            isRecording = true;
            //Debug.Log("Recording Started");
        }

        if (isRecording && recordingTimer > 20)
        {
            //Stop Recording
            //currentRecordingCharacter.GetComponent<CharacterController>().GetObjectToRecord().GetComponent<UnityAnimationRecorder>().StopRecording();
            //Reset recording timer
            recordingTimer = 0;
            isRecording = false;
        }

        //Debug.Log(recordingTimer);

        if (CURRENTSTATE == "Welcome")
        {
            WelcomeProp.SetActive(true);
        }
        else
        {
            WelcomeProp.SetActive(false);
        }

        //update timers
        timer += Time.deltaTime;

        if (isRecording)
        {
            recordingTimer += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            gameObject.GetComponent<InvisibleStateManager>().CreateInvisible(SwitchModel.GetActiveCharacter());
        }

    }

    public void SetRandomState()
    {
        int stateIndex = ManualOverride ? OverrideStateIndex : (int)UnityEngine.Random.Range(0, statesList.Count);
        var newState = statesList[stateIndex];
        timer = 0f;
        TimeInverval = UnityEngine.Random.Range(MinTimes[stateIndex], MaxTimes[stateIndex]);
        SetState(newState);
    }


    public void SetState(string state)
    {
        try
        {
            PREVIOUSSTATE = CURRENTSTATE;
            CURRENTSTATE = STATES[state];

            Debug.Log("Current State: " +  CURRENTSTATE);

            if (state == STATES["Invisible"])
            {
                gameObject.GetComponent<InvisibleStateManager>().CreateInvisible(SwitchModel.GetActiveCharacter());
            }

            foreach (GameObject character in characters)
            {
                if(character.activeInHierarchy)
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
