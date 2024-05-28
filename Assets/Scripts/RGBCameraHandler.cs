using com.rfilkov.kinect;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static com.rfilkov.kinect.KinectInterop;
using System.IO;
using com.rfilkov.components;
using UnityEngine.UIElements;

public class RGBCameraHandler : MonoBehaviour
{

    [Tooltip("Depth sensor index - 0 is the 1st one, 1 - the 2nd one, etc.")]
    public int sensorIndex = 0;

    public UnityEngine.UI.Image rgbCameraImage;

    public bool PreviewCamera = true;



    // primary sensor data structure
    private KinectInterop.SensorData sensorData = null;
    private KinectManager kinectManager = null;

    //[Header("Save Photos")]
    public bool SavePhotos = false;
    public float TimeInverval = 20.0f;

    private float timer;

    void Start()
    {
        timer = Time.deltaTime;
        
        //Extracted from BackgroundRemovalManger.cs
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
        if (PreviewCamera)
        {
            Texture image = kinectManager.GetColorImageTex(sensorIndex);
            rgbCameraImage.sprite = Sprite.Create((Texture2D)image, new Rect(0.0f, 0.0f, image.width, image.height), new Vector2(0.5f, 0.5f), 100.0f);
        }

        if(SavePhotos)
        {
            timer += Time.deltaTime;

            if (timer >= TimeInverval)
            {
                timer = 0f;
                TakePicture();
            }

        }
    }

    public void TakePicture()
    {
        Texture2D image = (Texture2D)kinectManager.GetColorImageTex(sensorIndex);

        byte[] imageByte = image.EncodeToJPG();

        string sDirName = Application.persistentDataPath + "/Photos";
        if (!Directory.Exists(sDirName))
            Directory.CreateDirectory(sDirName);

        string sFileName = sDirName + "/" + string.Format("{0:F0}", Time.realtimeSinceStartup * 10f) + ".jpg";
        File.WriteAllBytes(sFileName, imageByte);

        Debug.Log("Photo saved to: " + sFileName);
    }
}
