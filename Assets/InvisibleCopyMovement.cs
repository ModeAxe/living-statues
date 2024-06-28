using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleCopyMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject activeCharacter;
    public GameObject parent;
    Transform bone = null;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (parent != null)
        {
            transform.position = parent.transform.position;
            transform.rotation = parent.transform.rotation;
        }

    }

    private Transform GetBone(string name, Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                bone = child;
            }
            else
            {
                if (child.childCount > 0)
                {
                    if (bone == null)
                    {
                        GetBone(name, child);
                    }
                }
            }
        }

        return bone;
    }
}
