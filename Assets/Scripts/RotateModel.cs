using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Custom Script to rotate any object along any axis
public class RotateModel : MonoBehaviour
{
    public int min_x;
    public int max_x;
    public int min_y;
    public int max_y;
    public int min_z;
    public int max_z;
    private int x;
    private int y;
    private int z;
    // Update is called once per frame
    private void Start()
    {
        x = Random.Range(min_x, max_x);
        y = Random.Range(min_y, max_y);
        z = Random.Range(min_z, max_z);
    }
    void Update()
    {
        this.gameObject.transform.Rotate(new Vector3(x * 10 * Time.deltaTime, y * 10 * Time.deltaTime, z * 10 * Time.deltaTime));
        
    }
}
