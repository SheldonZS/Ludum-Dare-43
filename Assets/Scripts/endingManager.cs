using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class endingManager : MonoBehaviour {

    private DataBucket db;
    private AudioSource SFX;

    public Sprite BaldrEnding;
    public Sprite CthulhuEnding;
    public Sprite GummyBearEnding;
    public Sprite NephthysOff;
    public Sprite NephthysOn;
    public Sprite AnansiOff;
    public Sprite AnansiOn;
    public Sprite CuchulainOff;
    public Sprite CuchulainOn;
    public Sprite AmaterasuOff;
    public Sprite AmaterasuOn;
    public Sprite MercuryOff;
    public Sprite MercuryOn;
    public Sprite SekhmetOff;
    public Sprite SekhmetOn;
    public Sprite MonkeyKingOff;
    public Sprite MonkeyKingOn;
    // Use this for initialization
    void Start () {
        db = GameObject.Find("DataBucket").GetComponent<DataBucket>();
        SFX = GameObject.Find("SFX").GetComponent<AudioSource>();

        if(db.BaldrEnding == true)
        {
            GameObject.Find("BaldrEnding").GetComponent<Image>().overrideSprite = BaldrEnding;
            GameObject.Find("BaldrText").GetComponent<Text>().text = "Baldr Ending";
        }

        if (db.CthulhuEnding == true)
        {
            GameObject.Find("CthulhuEnding").GetComponent<Image>().overrideSprite = CthulhuEnding;
            GameObject.Find("CthulhuText").GetComponent<Text>().text = "Cthulhu Ending";
        }

        if (db.GummyBearEnding == true)
        {
            GameObject.Find("GummyBearEnding").GetComponent<Image>().overrideSprite = GummyBearEnding;
            GameObject.Find("GummyBearText").GetComponent<Text>().text = "Gummy Bear Ending";
        }

        if (db.NephthysOffering == true)
            GameObject.Find("Nephthys").GetComponent<Image>().overrideSprite = NephthysOn;
        else GameObject.Find("Nephthys").GetComponent<Image>().overrideSprite = NephthysOff;

        if (db.AnansiOffering == true)
            GameObject.Find("Anansi").GetComponent<Image>().overrideSprite = AnansiOn;
        else GameObject.Find("Anansi").GetComponent<Image>().overrideSprite = AnansiOff;

        if (db.CuchulainOffering == true)
            GameObject.Find("Cuchulain").GetComponent<Image>().overrideSprite = CuchulainOn;
        else GameObject.Find("Cuchulain").GetComponent<Image>().overrideSprite = CuchulainOff;

        if (db.AmaterasuOffering == true)
            GameObject.Find("Amaterasu").GetComponent<Image>().overrideSprite = AmaterasuOn;
        else GameObject.Find("Amaterasu").GetComponent<Image>().overrideSprite = AmaterasuOff;

        if (db.MercuryOffering == true)
            GameObject.Find("Mercury").GetComponent<Image>().overrideSprite = MercuryOn;
        else GameObject.Find("Mercury").GetComponent<Image>().overrideSprite = MercuryOff;

        if (db.SekhmetOffering == true)
            GameObject.Find("Sekhmet").GetComponent<Image>().overrideSprite = SekhmetOn;
        else GameObject.Find("Sekhmet").GetComponent<Image>().overrideSprite = SekhmetOff;

        if (db.MonkeyKingOffering == true)
            GameObject.Find("Monkey King").GetComponent<Image>().overrideSprite = MonkeyKingOn;
        else GameObject.Find("Monkey King").GetComponent<Image>().overrideSprite = MonkeyKingOff;
    }
	
    public void cthuluEnding()
    {
        if (db.CthulhuEnding)
        {
            db.ending = Diety.Cthulhu;
            SceneManager.LoadScene("ending");
        }
        else SFX.PlayOneShot(Resources.Load<AudioClip>("SFX/errorbeep"));
    }

    public void baldrEnding()
    {
        if (db.BaldrEnding)
        { 
            db.ending = Diety.Baldr;
            SceneManager.LoadScene("ending");
        }
        else SFX.PlayOneShot(Resources.Load<AudioClip>("SFX/errorbeep"));
    }

    public void gummyEnding()
    {
        if (db.GummyBearEnding)
        { 
            db.ending = Diety.GummyBear;
            SceneManager.LoadScene("ending");
        }
        else SFX.PlayOneShot(Resources.Load<AudioClip>("SFX/errorbeep"));
    }

    public void returnToTitle()
    {
        SceneManager.LoadScene("Title");
    }

	// Update is called once per frame
}
