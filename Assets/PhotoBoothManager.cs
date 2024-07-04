using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class PhotoBoothManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Intro; 
    public GameObject Explainer;
    public GameObject Countdown;
    public GameObject Thanks;

    [SerializeField] private TextMeshProUGUI countdownText;
    private bool countingDown = false;
    private float currentTime = 6;
    void Start()
    {
        countdownText = GameObject.Find("CountdownText").GetComponent<TextMeshProUGUI>();
        currentTime = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Intro.activeSelf)
        {
            StartPhotobooth();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (countingDown)
        {
            currentTime -= 1 * Time.deltaTime;
            countdownText.text = ((int)currentTime).ToString();
        }

        if (currentTime <= 0)
        {
            countingDown = false;
            currentTime = 5;
            Countdown.SetActive(false);
            StartCoroutine(TakePhotoPrep());
        }
    }

    private void StartPhotobooth()
    {

        //SHOW EXPLAINER
        StartCoroutine(ShowExplainer());

        //RUN SWITCH MODEL SEQUENCE / TEXTURE ASSINGING SEQUENCE


        //START COUNTDOWN


        //SHOW THANKS

        //RESET

    }

    IEnumerator ShowExplainer()
    {
        Intro.SetActive(false);
        GetComponent<SwitchModel>().Switch();
        Explainer.SetActive(true);
        yield return new WaitForSeconds(5);
        Explainer.SetActive(false);
        StartCountdown();
    }

    public void StartCountdown()
    {
        Countdown.SetActive(true);
        countdownText = GameObject.Find("CountdownText").GetComponent<TextMeshProUGUI>();
        countingDown = true;
    }

    IEnumerator ShowThanks()
    {
        yield return new WaitForSeconds(2);
        Thanks.SetActive(true);
        yield return new WaitForSeconds(10);
        Reset();
    }

    IEnumerator TakePhotoPrep()
    {
        //Material newMaterial = new Material(Shader.Find("HDRP/Lit"));
        //newMaterial.mainTexture = GetTexture();
        //Renderer renderer = SwitchModel.GetActiveCharacter().GetComponent<Transform>().GetChild(0).GetComponent<Transform>().gameObject.GetComponentInChildren<Renderer>();
        //renderer.material = newMaterial;
        //Material mat = SwitchModel.GetActiveCharacter().GetComponent<Transform>().GetChild(0).GetComponent<Transform>().gameObject.GetComponentInChildren<Renderer>();
        //mat.mainTexture = GetTexture();
        yield return new WaitForSeconds(2);
        StartCoroutine(TakePhoto());
    }

    IEnumerator TakePhoto()
    {
        yield return new WaitForEndOfFrame();
        ScreenCapture.CaptureScreenshot("testcapture", 1);

        Texture2D image = ScreenCapture.CaptureScreenshotAsTexture();

        byte[] imageByte = image.EncodeToJPG();

        string sDirName = Application.persistentDataPath + "/Portraits";
        if (!Directory.Exists(sDirName))
            Directory.CreateDirectory(sDirName);

        string sFileName = sDirName + "/" + string.Format("{0:F0}", Time.realtimeSinceStartup * 10f) + ".jpg";
        File.WriteAllBytes(sFileName, imageByte);

        Debug.Log("Photo saved to: " + sFileName);

        StartCoroutine(ShowThanks());
    }

    private void Reset()
    {
        Countdown.SetActive(false);
        Thanks.SetActive(false);
        Intro.SetActive(true);
        Explainer.SetActive(false);
        Countdown.SetActive(false);
        Thanks.SetActive(false);
}

    private Texture2D GetTexture()
    {
        //load image[s]
        string imagePath = null;
        if (Directory.Exists(Path.Combine(Application.persistentDataPath, "Photos")))
        {
            string photosDirectory = Path.Combine(Application.persistentDataPath, "Photos");
            //Get List of Images
            List<string> imageNames = new List<string>();
            DirectoryInfo directoryInfo = new DirectoryInfo(photosDirectory);
            foreach (FileInfo imageName in directoryInfo.GetFiles("*.jpg"))
            {
                imageNames.Add(imageName.Name);
            }
            //Select an image
            string imageFileName = imageNames[(int)Random.Range(0, imageNames.Count)];


            imagePath = Path.Combine(photosDirectory, imageFileName);
        }
        if (imagePath == null)
        {
            Debug.Log("No Image Found");
        }
        byte[] bytes = File.ReadAllBytes(imagePath);
        Texture2D texture = new Texture2D(900, 900, TextureFormat.RGB24, false);
        texture.LoadImage(bytes);

        //get interesting part
        //Rect cropArea = FindMostInterestingRectByVariance(texture, 64, 64);
        //Rect cropArea = FindMostInterestingRectByEdgeDetection(texture, 64, 64);

        //Rect cropArea = GetRandomPartOfTexture(texture, 300, 300);
        ////return texture
        //texture = CropTexture(texture, cropArea);
        return texture;
    }
}
