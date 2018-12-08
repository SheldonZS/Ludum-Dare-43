using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingDisplayer : MonoBehaviour {

    private DataBucket databucket;
    private AudioSource musicPlayer;
    private Image endingImage;
    private Text endingText;

    private Diety currentEnding;
	// Use this for initialization
	void Awake () {
        databucket = GameObject.Find("DataBucket").GetComponent < DataBucket > ();
        musicPlayer = GameObject.Find("BGM 1").GetComponent<AudioSource>();
        endingImage = GameObject.Find("endingImage").GetComponent<Image>();
        endingText = GameObject.Find("endingText").GetComponent<Text>();
	}

    private void Start()
    {
        currentEnding = databucket.ending;

        musicPlayer.Stop();

        switch (currentEnding)
        {
            case Diety.Cthulhu:
                musicPlayer.PlayOneShot(Resources.Load<AudioClip>("BGM/Cthulhu Ending2"));
                musicPlayer.loop = false;
                endingText.text = "So I may have just sacrificed my blood to summon Cthulhu. No worries. It only results in the destruction of the entire universe. It's not my fault. I didn't know I was going to end the world. Well, maybe I had an inkling. ";
                endingImage.overrideSprite = Resources.Load<Sprite>("cthulhu_ending");
                break;
            case Diety.GummyBear:
                musicPlayer.PlayOneShot(Resources.Load<AudioClip>("BGM/Gummy_Victory"));

                musicPlayer.loop = false;
                endingText.text = "I can't think of a justification for why I sacrificed to the Gummy Bear God. I am part of it now. And soon the world will join me, absorbed into the Gummy Mass of Everything Sweet, which is everything.";
                endingImage.overrideSprite = Resources.Load<Sprite>("gummy_ending");
                break;
            case Diety.Baldr:
                musicPlayer.PlayOneShot(Resources.Load<AudioClip>("BGM/BaldrVictory3"));
                musicPlayer.loop = false;

                endingText.text = "I made it out of the temple! Baldr opened the doors for me when no one else would! Praise Baldr! You're my favorite. I'm sure you wanted to free me as soon as you learned about my plight. Sorry if I never return.";
                endingImage.overrideSprite = Resources.Load<Sprite>("baldr_ending");
                break;
            default:
                endingText.text = "Error: ending not found";

                break;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
