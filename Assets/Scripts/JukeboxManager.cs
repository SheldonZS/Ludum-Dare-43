using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JukeboxManager : MonoBehaviour {

    private Text juketext;
    private AudioSource musicPlayer;

    private DataBucket db;
	// Use this for initialization
	void Start () {

        musicPlayer.Stop();
        musicPlayer.loop = true;

	}

    private void Awake()
    {
        musicPlayer = GameObject.Find("BGM 1").GetComponent<AudioSource>();
        db = GameObject.Find("DataBucket").GetComponent<DataBucket>();
        juketext = GameObject.Find("juketext").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void SelectMusic(string music)
    {
        switch(music)
        {
            case "title":
                musicPlayer.clip = Resources.Load<AudioClip>("BGM/Main Title");
                musicPlayer.Play();
                juketext.text = "Main Title, composed by Jamin Morden \n\tWhen we started brainstorming, I wasn't sure yet what kind of game we were going for, so I just sketched out several extremely short, basic ideas. These included, among other ideas, a song for an island-type theme, something modeled roughly after 1960s-style infomercial soundtracks, and what ended up being the ending theme for one of the gods. Once I was given direction for the tone of the title screen, \"dark and foreboding,\" I grabbed the thematic idea that seemed best suited, played around with it a bit, and made it the main theme of the game.";

                break;
            case "intro":
                musicPlayer.clip = Resources.Load<AudioClip>("BGM/IntroTune");
                musicPlayer.Play();

                juketext.text = "Intro Theme, composed by Jamin Morden \n\tThis is a variation on the bell theme heard in the wandering theme. I wanted to write something light-hearted, but still capture the mystery of wandering around a building full of various gods. The harp provides the melody and mystery, and the strings keep things light, as well as increasing the rhythmic interest.";
                break;
            case "wandering":
                musicPlayer.clip = Resources.Load<AudioClip>("BGM/Wandering-5");
                musicPlayer.Play();

                juketext.text = "Wandering Theme, composed by Jamin Morden" +
                    "\n\tSince the game takes place in a museum-like atmosphere, I wanted to write a tune that was somber, but not terrified, melancholic, or sad, as a minor key would normally imply.The need to write something that would increase in tension led to the second theme of the game: the bell theme, which in this song rises in pitch (and implied tonality) before the other instruments drag us back to the start of the loop."
                + "\n\tMy main tool for writing music is software called Finale, which many folks use due to its wide range of tools for engraving different kinds of sheet music, and almost certainly not a single professional game music composer uses, since the playback can be obstinate and many, many instruments sound terrible.A few instruments that Finale imitates well are harp, piano, clarinet, flute, and most string sounds(unless you want them to have any sort of attack whatsoever), hence the instrument choices for most of the game."
                + "\n\tI am a big fan of Yoshi's Island for the SNES and its soundtrack, so I was pleased when asked to make layering orchestration for this theme. I always enjoy the comparison between \"here's the theme at the beginning of the game\" versus \"here's the theme close to the ending of the game.\"";
                break;
            case "cthulhu":
                if (db.CthulhuEnding == true)
                {
                    musicPlayer.clip = Resources.Load<AudioClip>("BGM/Cthulhu Ending2");
                    musicPlayer.Play();
                    juketext.text = "Cthulhu Ending Theme, composed by Jamin Morden"+
                        "\n\tThe initial draft of this theme was slower, more somber, and more creeping in its terror. Then I learned that the ending \"should be frantic, like the world is ending.\" This was tricky, since the instruments I had already chosen to represent Cthulhu were the bassoon and the contrabassoon, both of which have a lovely, otherworldly sound in the low range(contrabassoon especially), but neither of which struck me as particularly frantic.The string section provides the frantic for us, though, and the bassoons get to rise from the depths to herald Cthulhu destroying the world. ";

                }
                else
                {
                    juketext.text = "Play the game to unlock this ending music!";
                }
                break;
            case "gummy":

                if (db.GummyBearEnding == true)
                {
                    musicPlayer.clip = Resources.Load<AudioClip>("BGM/Gummy_Victory");
                    musicPlayer.Play();
                    juketext.text = "Gummy Ending Theme, composed by Jamin Morden \n\tMeredith asked me to compose something \"insanely happy.\" I took that literally, attempting to compose something simultaneously happy and insane. You might hear the heavy inspiration from the opening theme of The Simpsons, which provided a great deal of harmonic inspiration. The bell theme enters in the piano, which makes sense on its own, but as more instruments come in, the song gets less and less focused until the whole thing collapses. The xylophone represents this god and is featured in this tune. ";

                }
                else
                {
                    juketext.text = "Play the game to unlock this ending music!";
                }
                break;
            case "baldr":
                if (db.BaldrEnding == true)
                {
                    musicPlayer.clip = Resources.Load<AudioClip>("BGM/BaldrVictory3");
                    musicPlayer.Play();
                    juketext.text = "Baldr Ending Theme, composed by Jamin Morden\n\t	The first draft of this was extremely Norse and Skyrim sounding. I was kindly reminded that Baldr is the Norse god of \"light, joy, and purity\" and not, in fact, his father Odin. I had really wanted to do a Nordic hunting-horn style piece for this (mostly for selfish reasons, as I am a horn player), and had to do two more drafts before coming up with something appropriate for the god of joy. \n\tUnfortunately, Finale's stock horn MIDI is awful (you can hear some of it in the Gummy ending). Since I play the horn myself, I was able to record the horn live at a (very kind, very patient) friend's home at a moment's notice. ";

                }
                else
                {
                    juketext.text = "Play the game to unlock this ending music!";
                }
                break;
            default:
                break;
        }
    }
}
