using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireEffect : MonoBehaviour {

    public float duration;
    private float startTime;
	// Use this for initialization
	void Start () {
        startTime = Time.time;
        GameObject.Find("SFX").GetComponent<AudioSource>().PlayOneShot((AudioClip) Resources.Load("SFX/Sacrifice Flame"));
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time - startTime > duration)
            Destroy(this.gameObject);
	}
}
