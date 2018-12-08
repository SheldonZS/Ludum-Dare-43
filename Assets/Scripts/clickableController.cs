using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clickableController : MonoBehaviour {

    public clickable clickable;
    RectTransform rt;

    private playerController player;
	// Use this for initialization
	void Awake () {
        player = GameObject.Find("Protag").GetComponent<playerController>();
        rt = GetComponent<RectTransform>();

        loadClickable(clickable);
	}

    private void Update()
    {
        rt.localPosition = clickable.position;
        rt.sizeDelta = clickable.size;
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

        if (c.sprite == null)
            GetComponent<Image>().color = Color.clear;
        else GetComponent<Image>().overrideSprite = c.sprite;
        rt.localPosition = c.position;
        rt.sizeDelta = c.size;
    }
}
