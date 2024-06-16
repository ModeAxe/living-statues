using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SwitchModel : MonoBehaviour
{
    // Start is called before the first frame update

    public bool ManualOverride = false;
    public int OverrideModelIndex = 0;

    public bool AutoSwitch = false;
    public int MaxTime = 60;
    public int MinTime = 30;
    private float timer;
    private float switchTimeInterval;


    public List<GameObject> options = new List<GameObject>();
    private static GameObject currentActiveCharacter;

    void Start()
    {
        switchTimeInterval = Random.Range(MinTime, MaxTime);
        Switch();        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (AutoSwitch)
        {
            if (timer > switchTimeInterval)
            {
                Switch();
                timer = 0f;
                switchTimeInterval = Random.Range(MinTime, MaxTime);
            }
            timer += Time.deltaTime;
            //Debug.Log(timer);
            //Debug.Log(switchTimeInterval);
        }

    }

    public void Switch()
    {
        int randomIndex = ManualOverride ? OverrideModelIndex : Random.Range(0, (options.Count));
        
        foreach (var model in options)
        {
            model.SetActive(false);
        }

        options[randomIndex].SetActive(true);
        currentActiveCharacter = options[randomIndex];
        Debug.Log(options[randomIndex].name);
    }

    public static GameObject GetActiveCharacter()
    {
        return currentActiveCharacter;
    }
}
