using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SwitchModel : MonoBehaviour
{
    // Start is called before the first frame update
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
    }

    public void Switch()
    {
        int randomIndex = Random.Range(0, (options.Count));
        
        foreach (var model in options)
        {
            model.SetActive(false);
        }

        options[randomIndex].SetActive(true);
        Debug.Log(options[randomIndex].name);
    }
}
