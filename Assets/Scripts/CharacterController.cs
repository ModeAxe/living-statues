using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject characterMirror;

    private GameObject anchor;

    public bool RotateStatic = true;
    public float RotationSpeed = 2.0f;

    private string state;
    private Animator animator;
    private AnimatorOverrideController overrideController;
    public AnimationClip[] wildCardClips;

    public static Dictionary<string, string> stateTrigger = new Dictionary<string, string>()
    {
        { "Static", "TrAnyToStatic" },
        { "Welcome", "TrAnyToWelcome"},
        { "Hero", "TrAnyToHero"},
        { "FollowLeft", "TrAnyToFollowLeft"},
        { "FollowRight", "TrAnyToFollowRight" },
        { "Mimic", "TrAnyToMimic"},
        { "Invisible", "TrAnyToInvisible"},
        { "Wildcard", "TrAnyToWildcard" },
        { "Loaded", "TrAnyToLoaded" }
    };

    public GameObject objectToRecord;

    void Start()
    {
        //state = "Static";    
        animator = GetComponent<Animator>();
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;

        anchor = GameObject.Find("Anchor");
    }

    // Update is called once per frame
    void Update()
    {
        if (state != StateManager.CURRENTSTATE) { SetState(StateManager.CURRENTSTATE); }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            state = "Wildcard";
            StateManager.CURRENTSTATE = state;  
            overrideController["Wildcard"] = wildCardClips[(int)Random.Range(0, wildCardClips.Length)];
            Debug.Log(overrideController["Wildcard"]);
            SetState("Wildcard");
        }

        if (state == "Static")
        {
            this.gameObject.transform.Rotate(new Vector3(0, RotationSpeed * 10 * Time.deltaTime, 0));
        }
        else
        {
            gameObject.transform.SetPositionAndRotation(anchor.transform.position, Quaternion.identity);
        }
        if (state == "Mimic")
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = false;
            }

            Renderer[] renderersMirror = characterMirror.GetComponentsInChildren<Renderer>();
            characterMirror.transform.SetPositionAndRotation(anchor.transform.position, Quaternion.identity);
            foreach (Renderer renderer in renderersMirror)
            {
                renderer.enabled = true;
            }
        }
        else
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = true;
            }
            Renderer[] renderersMirror = characterMirror.GetComponentsInChildren<Renderer>();
            characterMirror.transform.SetPositionAndRotation(anchor.transform.position, Quaternion.identity);
            foreach (Renderer renderer in renderersMirror)
            {
                renderer.enabled = false;
            }
        }
    }

    public void SetState(string state)
    {

        //do some logic to determine what trigger to call and then call the trigger
        if (state == "Mimic")
        {
            this.state = state;
            SetMimic();
        }
        else
        {
            this.state = state;
            animator.SetTrigger(stateTrigger[state]);
        }
    }

    private void SetMimic()
    {
        animator.SetTrigger(stateTrigger["Mimic"]);
        this.state = "Mimic";
    }

    public GameObject GetObjectToRecord()
    {
        return objectToRecord;
    }
}
