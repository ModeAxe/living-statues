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


    public List<GameObject> options = new List<GameObject>();

    void Start()
    {
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
            if (timer > MaxTime)
            {
                Switch();
                timer = 0f;
            }
            timer += Time.deltaTime;
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
        Debug.Log(options[randomIndex].name);
    }
}
