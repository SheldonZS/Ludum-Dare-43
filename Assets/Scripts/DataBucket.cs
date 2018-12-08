using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBucket : MonoBehaviour {

    public static DataBucket instance = null;

    public bool CthulhuEnding = false;
    public bool GummyBearEnding = false;
    public bool BaldrEnding = false;

    public bool CuchulainOffering = false;
    public bool AnansiOffering = false;
    public bool AmaterasuOffering = false;
    public bool NephthysOffering = false;
    public bool SekhmetOffering = false;
    public bool MercuryOffering = false;
    public bool MonkeyKingOffering = false;

    public Diety ending;
    public bool loadGame;
    public bool savedGame;

    [Header("New Game Settings")]

    public item[] initialInventory;
    private item[] savedInventory;
    public Diety[] initialWheel;
    private wheelSlot[] savedWheel;
    public roomSO[] initialRooms;
    private room[] savedRooms;
    private int[] savedPlayer;

    void Awake ()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        else
            Destroy(gameObject);
	}

    public item[] getInventory()
    {
        if (loadGame)
            return savedInventory;
        else
            return initialInventory;
    }

    public void saveInventory(List<item> inv)
    {
        savedInventory = new item[inv.Count];

        for (int x = 0; x < inv.Count; x++)
            savedInventory[x] = inv[x];
    }

    public wheelSlot[] getWheel()
    {
        wheelSlot[] temp = new wheelSlot[10];
        for (int x = 0; x < 10; x++)
        {
            if (loadGame)
                temp[x] = new wheelSlot(savedWheel[x]);
            else
                temp[x] = new wheelSlot(initialWheel[x]);

        }
        return temp;
    }

    public void saveWheel(wheelSlot[] wheel)
    {
        savedWheel = new wheelSlot[10];
        for(int x = 0;x < 10;x++)
        {
            savedWheel[x] = new wheelSlot(wheel[x]);
        }
    }

    public room[] getRooms()
    {
        room[] rooms;
        if (loadGame)
        {
            rooms = new room[savedRooms.Length];

            for (int x = 0; x < rooms.Length; x++)
            {
                rooms[x].name = savedRooms[x].name;
                rooms[x].roomImage = savedRooms[x].roomImage;
                rooms[x].playerVisible = savedRooms[x].playerVisible;
                rooms[x].objects = new List<clickable>();

                foreach (clickable o in savedRooms[x].objects)
                    rooms[x].objects.Add(o);
            }
        }
        else
        {
            rooms = new room[initialRooms.Length];

            for (int x = 0; x < rooms.Length; x++)
            {
                rooms[x].name = initialRooms[x].name;
                rooms[x].roomImage = initialRooms[x].roomImage;
                rooms[x].playerVisible = initialRooms[x].playerVisible;
                rooms[x].objects = new List<clickable>();

                foreach (clickable o in initialRooms[x].clickables)
                    rooms[x].objects.Add(o);
            }
        }

        return rooms;
    }

    public void saveRooms(room[] rooms)
    {
        savedRooms = new room[rooms.Length];

        for (int x = 0; x < rooms.Length; x++)
        {
            savedRooms[x].name = rooms[x].name;
            savedRooms[x].roomImage = rooms[x].roomImage;
            savedRooms[x].playerVisible = rooms[x].playerVisible;
            savedRooms[x].objects = new List<clickable>();

            foreach (clickable o in rooms[x].objects)
                savedRooms[x].objects.Add(o);
        }
    }

    public int[] getPlayer()
    {
        if (loadGame)
            return savedPlayer;
        else return new int[] {0, 0, 0, 0};
    }

    public void savePlayer(int candles, int tones, int pleased, bool doorUnlocked)
    {
        savedPlayer = new int[] { candles, tones, pleased, 0 };
        if (doorUnlocked)
            savedPlayer[3] = 1;
    }
    public void saveGame()
    {
        savedGame = true;
        GameObject.Find("Inventory").GetComponent<InventoryManager>().save();
        GameObject.Find("Wheel").GetComponent<altarController>().save();
        GameObject.Find("Background").GetComponent<roomController>().save();
        GameObject.Find("Protag").GetComponent<playerController>().save();
    }
}