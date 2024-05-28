using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    // Start is called before the first frame update

    private string state;
    private Animator animator;

    public static Dictionary<string, string> stateTrigger = new Dictionary<string, string>()
    {
        { "Static", "TrAnyToStatic" },
        { "Welcome", "TrAnyToWelcome"},
        { "Hero", "TrAnyToHero"},
        { "FollowLeft", "TrAnyToFollowLeft"},
        { "FollowRight", "TrAnyToFollowRight" },
        { "Mimic", "TrAnyToMimic"},
        { "Invisible", "TrAnyToInvisible"}
    };


    void Start()
    {
        //state = STATE;    
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
  
    }

    public void SetState(string state)
    {
        this.state = state;

        //do some logic to determine what trigger to call and then call the trigger
        animator.SetTrigger(stateTrigger[state]);
    }
}
