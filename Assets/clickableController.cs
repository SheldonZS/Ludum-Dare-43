using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickableController : MonoBehaviour {

    public clickable clickable;

    private playerController player;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Protag").GetComponent<playerController>();
	}

    public void onClick()
    {
        if(!player.busy)
            player.clickedObject(clickable);
    }
}
