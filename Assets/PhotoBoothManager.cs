using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
            TakePhoto();
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
        yield return new WaitForSeconds(4);
    }

    private void TakePhoto()
    {

    }
}
