using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour {

    public int slotNumber = 0;
    public InventoryManager im = null;
    public Image image;

    public void itemClicked()
    {
        im.itemClicked(slotNumber);
    }
}
