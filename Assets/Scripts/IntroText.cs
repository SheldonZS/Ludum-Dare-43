using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class IntroText : MonoBehaviour {
    private AudioSource sfxPlayer;

    private float fadeInTime = 0.025F;
    private string str;

    private Text text1, text2, text3, text4;
    private Text completeText;
    private Text currentText;

    private int timesClicked;

    public Toggle loopToggle;

    bool printingText = true;

    private void Awake()
    {
        sfxPlayer = GameObject.Find("SFX").GetComponent<AudioSource>();

        text1 = GameObject.Find("text1").GetComponent<Text>();
        text2 = GameObject.Find("text2").GetComponent<Text>();
        text3 = GameObject.Find("text3").GetComponent<Text>();
        text4 = GameObject.Find("text4").GetComponent<Text>();

        text2.enabled = false;
        text3.enabled = false;
        text4.enabled = false;

    }

    private void Start()
    {
        printingText = true;
        timesClicked = 0;
        StartCoroutine(animate(text1.text, text1));

        sfxPlayer.clip = Resources.Load<AudioClip>("SFX/sfx_writing");
        sfxPlayer.loop = true;
        sfxPlayer.Play();

    }


    void Update()
    { 
        /*
        if (Input.GetButtonDown("Fire1"))
        {
            //Debug.Log ("clicked");
            timesClicked++;
            if (printingText)
            {
                printingText = false;

                AnimateNext();

            }

        }//if click
        */
    }

    public void AnimateNext()
    {

        if (!printingText)
        {
            Debug.Log("animating next");

            switch (timesClicked)
            {
                case 1:
                    printingText = false;
                    printingText = true;
                    StartCoroutine(animate(text2.text, text2));
                    break;
                case 2:
                    printingText = false;
                    printingText = true;
                    StartCoroutine(animate(text3.text, text3));
                    break;
                case 3:
                    printingText = false;
                    printingText = true;
                    StartCoroutine(animate(text4.text, text4));
                    break;
                default:
                    sfxPlayer.Stop();
                    sfxPlayer.loop = false;
                    Debug.Log("loop ending");
                    break;
            }
        }
        
    }

    public IEnumerator animate(string strComplete, Text currentText)
    {
        currentText.enabled = false;
        int i = 0;
        currentText.text = "";
        currentText.enabled = true;
        while (i < strComplete.Length && printingText)
        {
            currentText.text += strComplete[i++];
            yield return new WaitForSeconds(fadeInTime);
        }
        currentText.text = strComplete;
        printingText = false;
        timesClicked++;
        Debug.Log("time increased to: " + timesClicked);
        AnimateNext();
    }
}
