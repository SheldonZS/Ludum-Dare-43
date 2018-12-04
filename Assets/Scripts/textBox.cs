using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class textBox : MonoBehaviour {

    public float showDialogueBoxTime;
    public float delayBetweenLetters;

    private playerController player;
    private RectTransform rt;
    private Text text;
    private float scale;

	// Use this for initialization
	void Start () {
        rt = GetComponent<RectTransform>();
        text = GetComponentInChildren<Text>();
        scale = GameObject.Find("Canvas").GetComponent<Canvas>().scaleFactor;
        player = GameObject.Find("Protag").GetComponent<playerController>();
        text.text = "";
	}
	
    public void displayText(string displayText)
    {
        StartCoroutine(animateText(displayText));
    }

    public IEnumerator animateText(string displayText)
    {
        player.busy = true;
        //Debug.Log("Player Busy");

        float startTime = Time.time;
        Vector3 pos = rt.position;

        text.text = "";

        while (Time.time - startTime < showDialogueBoxTime)
        {
            pos.y = (128 * (Time.time - startTime) / showDialogueBoxTime) * scale;
            rt.position = pos;
            yield return null;
        }

        pos.y = 128 * scale;
        rt.position = pos;

        for(int x = 0; x <= displayText.Length; x++)
        {
            if (Input.GetMouseButtonDown(0))
                x = displayText.Length - 1;
            text.text = displayText.Substring(0, x);
            yield return new WaitForSeconds(delayBetweenLetters);
        }

        do
        {
            yield return null;
        } while (Input.GetMouseButtonDown(0) == false);

                startTime = Time.time;
        while (Time.time - startTime < showDialogueBoxTime)
        {
            pos.y = (128 - 128 * (Time.time - startTime) / showDialogueBoxTime) * scale;
            rt.position = pos;
            yield return null;
        }

        pos.y = 0;
        rt.position = pos;

        player.busy = false;
        //Debug.Log("End Dialogue");
    }
}
