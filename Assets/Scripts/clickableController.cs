using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clickableController : MonoBehaviour {

    public clickable clickable;

    private playerController player;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Protag").GetComponent<playerController>();

        loadClickable(clickable);
	}

    public void onClick()
    {
        if(!player.busy)
            player.clickedObject(this);
    }

    public void loadClickable(clickable c)
    {
        if (c == null)
            return;

        clickable = c;

        RectTransform rt = GetComponent<RectTransform>();

        if (c.sprite == null)
            GetComponent<Image>().color = Color.clear;
        else GetComponent<Image>().overrideSprite = c.sprite;
        rt.localPosition = c.position;
        rt.sizeDelta = c.size;
    }
}
