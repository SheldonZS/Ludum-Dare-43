using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class roomController : MonoBehaviour {

    public roomSO[] initialRooms;

    private room[] rooms;

    private Image background;
    private Transform objectsLayer;
    private GameObject player;
    private GameObject clickablePrefab;
    private AudioSource BGM;

    private int currentRoom;


	// Use this for initialization
	void Start () {

        rooms = new room[initialRooms.Length];
        background = GetComponent<Image>();
        objectsLayer = GameObject.Find("Objects").transform;
        player = GameObject.Find("Protag");
        clickablePrefab = (GameObject) Resources.Load("Clickable");
        BGM = GameObject.Find("BGM 1").GetComponent<AudioSource>();

        for (int x = 0; x < rooms.Length; x++)
        {
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
}

public struct room
{
    public Sprite roomImage;
    public bool playerVisible;
    public List<clickable> objects;
}