using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideFrom : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject hideThis;
    public GameObject fromThis;

    // Update is called once per frame
    void Update()
    {
        if (fromThis.active)
        {
           hideThis.SetActive(false);
        }
        else
        {
            hideThis.SetActive(true);
        }
    }
}
