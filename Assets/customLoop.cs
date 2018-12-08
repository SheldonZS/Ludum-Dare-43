using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customLoop : MonoBehaviour {

    private AudioSource BGM;

    public bool loop = false;
    public float loopStart;
    public float loopEnd;

    private void Start()
    {
        BGM = GetComponent<AudioSource>();
    }

    private void LateUpdate () {
	    if (loop)
        {
            if (BGM.time >= loopEnd)
                BGM.time = BGM.time - (loopEnd - loopStart);
        }
	}

    public void playCustomLoop(AudioClip clip, float start, float end)
    {
        BGM.Stop();
        BGM.clip = clip;
        BGM.Play();
        loop = true;
        loopStart = start;
        loopEnd = end;
    }

}
