using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour {

    //public bool instructionsOpen = false;
    //public Image blackbg;
    //public Image whiteborder;
    //public Text actualinstructions;
    //private AudioSource buttonSound;
    private AudioSource musicPlayer;
    private AudioSource customLooper;
    private AudioSource sfxPlayer;
    private DataBucket databucket;

	// Use this for initialization
	void Awake(){
        //buttonSound = GameObject.Find ("ButtonSound").GetComponent<AudioSource> ();
        databucket = GameObject.Find("DataBucket").GetComponent<DataBucket>();
        musicPlayer = GameObject.Find("BGM 1").GetComponent <AudioSource>();
        customLooper = GameObject.Find("BGM 2").GetComponent <AudioSource>();
        sfxPlayer = GameObject.Find("SFX").GetComponent<AudioSource>();

    }

    private void Start()
    {
        if( SceneManager.GetActiveScene().name == "title")
        {
            if (databucket.savedGame == true)
            {
                Image button = GameObject.Find("ContinueButton").GetComponent<Image>();
                button.enabled = true;
                button.GetComponentInChildren<Text>().enabled = true;
            }

        }
    }


    // Update is called once per frame
    void Update () {
	
	}

    public void Continue()
    {
        databucket.loadGame = true;
        LoadLevel();
    }

	public void LoadLevel()
    {
        SceneManager.LoadScene ("Sample_Scene");
        sfxPlayer.Stop();
        customLooper.Stop();
        musicPlayer.clip = Resources.Load<AudioClip>("BGM/wandering");
        musicPlayer.loop = true;
        musicPlayer.Play ();
	}

    public void LoadIntro()
    {
        sfxPlayer.Stop();
        customLooper.Stop();
        SceneManager.LoadScene("intro");
        customLooper.GetComponent<customLoop>().playCustomLoop(musicPlayer.clip = Resources.Load<AudioClip>("BGM/IntroTune"), 5.902f, 11.892f);
    }

		
	public void LoadCredits()
    {
		//buttonSound.Play ();
		SceneManager.LoadScene("credits");

		if (SceneManager.GetActiveScene().name == "ending" || SceneManager.GetActiveScene().name == "Sample_Scene")
		{
            sfxPlayer.Stop();
            customLooper.Stop();
            customLooper.GetComponent<customLoop>().playCustomLoop(musicPlayer.clip = Resources.Load<AudioClip>("BGM/Main Title"),53.666f,78.951f);
        }
	}

    public void LoadEndings()
    {
        SceneManager.LoadScene("EndingViewer");
        if (SceneManager.GetActiveScene().name == "ending" || SceneManager.GetActiveScene().name == "Sample_Scene")
        {
            sfxPlayer.Stop();
            customLooper.Stop();
            customLooper.GetComponent<customLoop>().playCustomLoop(musicPlayer.clip = Resources.Load<AudioClip>("BGM/Main Title"), 53.666f, 78.951f);

        }
    }

    public void LoadJukebox()
    {
        SceneManager.LoadScene("jukebox");
        if (SceneManager.GetActiveScene().name == "ending" || SceneManager.GetActiveScene().name == "Sample_Scene")
        {
            sfxPlayer.Stop();
            customLooper.Stop();
            customLooper.GetComponent<customLoop>().playCustomLoop(musicPlayer.clip = Resources.Load<AudioClip>("BGM/Main Title"), 53.666f, 78.951f);

        }

    }

    public void LoadTitle()
    {

        //buttonSound.Play ();
        SceneManager.LoadScene("title");
        //stop in-game music


        if (SceneManager.GetActiveScene().name == "Sample_Scene" || SceneManager.GetActiveScene().name == "ending")
        {
            sfxPlayer.Stop();
            customLooper.Stop();
            customLooper.GetComponent<customLoop>().playCustomLoop(musicPlayer.clip = Resources.Load<AudioClip>("BGM/Main Title"), 53.666f, 78.951f);
        }

        if (musicPlayer.isPlaying == false && customLooper.isPlaying == false)
        {
            sfxPlayer.Stop();
            customLooper.Stop();
            customLooper.GetComponent<customLoop>().playCustomLoop(musicPlayer.clip = Resources.Load<AudioClip>("BGM/Main Title"), 53.666f, 78.951f);
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /*public void Instructions(){
		//buttonSound.Play ();

		if (instructionsOpen)
		{
			instructionsOpen = false;
			blackbg.enabled = false;
			whiteborder.enabled = false;
			actualinstructions.enabled = false;
		}
		else
		{
			blackbg.enabled = true;
			whiteborder.enabled = true;
			actualinstructions.enabled = true;
			instructionsOpen = true;

		}
	}*/
}

