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
    private AudioSource sfxPlayer;
    private DataBucket databucket;

	// Use this for initialization
	void Awake(){
        //buttonSound = GameObject.Find ("ButtonSound").GetComponent<AudioSource> ();
        databucket = GameObject.Find("DataBucket").GetComponent<DataBucket>();
        musicPlayer = GameObject.Find("BGM 1").GetComponent <AudioSource>();
        sfxPlayer = GameObject.Find("SFX").GetComponent<AudioSource>();

    }


    // Update is called once per frame
    void Update () {
	
	}

	public void LoadLevel()
    {
        SceneManager.LoadScene ("Sample_Scene");
        sfxPlayer.Stop();
        musicPlayer.clip = Resources.Load<AudioClip>("BGM/wandering");
        musicPlayer.loop = true;
        musicPlayer.Play ();
	}

    public void LoadIntro()
    {
        SceneManager.LoadScene("intro");
        musicPlayer.clip = Resources.Load<AudioClip>("BGM/IntroTune");
        musicPlayer.loop = true;
        musicPlayer.Play();
    }

		
	public void LoadCredits()
    {
		//buttonSound.Play ();
		SceneManager.LoadScene("credits");

		if (SceneManager.GetActiveScene().name == "ending" || SceneManager.GetActiveScene().name == "Sample_Scene")
		{
            musicPlayer.clip = Resources.Load<AudioClip>("BGM/Main Title");
            musicPlayer.loop = true;
            musicPlayer.Play();
        }
	}

    public void LoadEndings()
    {
        SceneManager.LoadScene("EndingViewer");
        if (SceneManager.GetActiveScene().name == "ending" || SceneManager.GetActiveScene().name == "Sample_Scene")
        {
            musicPlayer.clip = Resources.Load<AudioClip>("BGM/Main Title");
            musicPlayer.loop = true;
            musicPlayer.Play();
        }
    }

    public void LoadJukebox()
    {
        SceneManager.LoadScene("jukebox");
        if (SceneManager.GetActiveScene().name == "ending" || SceneManager.GetActiveScene().name == "Sample_Scene")
        {
            musicPlayer.clip = Resources.Load<AudioClip>("BGM/Main Title");
            musicPlayer.loop = true;
            musicPlayer.Play();
        }

    }

    public void LoadTitle()
    {

        //buttonSound.Play ();
        SceneManager.LoadScene("title");
        //stop in-game music


        if (SceneManager.GetActiveScene().name == "Sample_Scene" || SceneManager.GetActiveScene().name == "ending")
        {
            musicPlayer.clip = Resources.Load<AudioClip>("BGM/Main Title");
            musicPlayer.loop = true;
            musicPlayer.Play();
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

