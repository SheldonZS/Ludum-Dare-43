using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

public class roomController : MonoBehaviour {

    public float fadeTime;
    public roomSO[] initialRooms;
    
    private room[] rooms;

    private Image background;
    private Transform objectsLayer;
    private GameObject player;
    private GameObject clickablePrefab;
    private AudioSource BGM;
    private Image Fader;

    private int currentRoom;


	// Use this for initialization
	void Start () {

        rooms = new room[initialRooms.Length];
        background = GetComponent<Image>();
        objectsLayer = GameObject.Find("Objects").transform;
        player = GameObject.Find("Protag");
        clickablePrefab = (GameObject) Resources.Load("Clickable");
        BGM = GameObject.Find("BGM 1").GetComponent<AudioSource>();
        Fader = GameObject.Find("Fader").GetComponent<Image>();

        for (int x = 0; x < rooms.Length; x++)
        {
            rooms[x].name = initialRooms[x].name;
            rooms[x].roomImage = initialRooms[x].roomImage;
            rooms[x].playerVisible = initialRooms[x].playerVisible;
            rooms[x].objects = new List<clickable>();

            foreach (clickable o in initialRooms[x].clickables)
               rooms[x].objects.Add(o);
        }

        //load initial room
        currentRoom = 0;
        background.overrideSprite = rooms[0].roomImage;
        foreach (clickable o in rooms[0].objects)
            spawnObject(o);

        //play main wandering theme
        BGM.loop = true;
        BGM.clip = Resources.Load("BGM/Wandering") as AudioClip;
        BGM.Play();

	}
	
	// Update is called once per frame
    private void spawnObject (clickable newObject)
    {
        GameObject newClickable = Instantiate(clickablePrefab, objectsLayer);
        newClickable.GetComponent<clickableController>().loadClickable(newObject);
    }

    public void addToRoom (clickable newObject)
    {
        rooms[currentRoom].objects.Add(newObject);
        spawnObject(newObject);
    }

    public void removeFromRoom (GameObject removeMe)
    {
        rooms[currentRoom].objects.Remove(removeMe.GetComponent<clickableController>().clickable);
        Destroy(removeMe);

        Debug.Log("Items in room: " + rooms[currentRoom].objects.Count);
    }

    public void moveToRoom (string newRoom)
    {
        int roomNum = -1;
        for(int x = 0; x < initialRooms.Length; x++)
        {
            if (rooms[x].name == newRoom)
                roomNum = x;
        }

        if (roomNum == -1)
            Debug.Log("Room Transition Failed");
        else
            StartCoroutine(moveToRoom(roomNum));
    }

    private IEnumerator moveToRoom(int roomNum)
    {
        Debug.Log("Beging Room Transition");

        player.GetComponent<playerController>().busy = true;

        float startTime = Time.time;
        Color fadeColor = Color.black;
        Fader.enabled = true;

        while((Time.time - startTime) < fadeTime)
        {
            fadeColor.a = (Time.time - startTime) / fadeTime;
            Fader.color = fadeColor;
            yield return null;
        }

        Fader.color = Color.black;

        while (objectsLayer.childCount > 0)
        {
            DestroyImmediate(objectsLayer.GetChild(0).gameObject);
        }

        background.overrideSprite = rooms[roomNum].roomImage;
        if (rooms[roomNum].playerVisible == false)
            player.GetComponent<SkeletonGraphic>().enabled = false;
        else
        {
            player.GetComponent<SkeletonGraphic>().enabled = true;
            //position player in room
        }

        foreach (clickable o in rooms[roomNum].objects)
            spawnObject(o);

        startTime = Time.time;
        fadeColor = Color.black;

        while ((Time.time - startTime) < fadeTime)
        {
            fadeColor.a = 1 - (Time.time - startTime) / fadeTime;
            Fader.color = fadeColor;
            yield return null;
        }

        Fader.enabled = false;
        player.GetComponent<playerController>().busy = false;
    }
}

public struct room
{
    public string name;
    public Sprite roomImage;
    public bool playerVisible;
    public List<clickable> objects;
}