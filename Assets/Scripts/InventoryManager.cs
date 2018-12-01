using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

    public Sprite nextArrow;
    public Sprite blank;
    public item[] defaultInventory;

    private InventoryItem[] inventorySlots;
    private List<item> inventory;
    private textBox textBox;

    private int selectedItem;
    private int currentPage;
    private frameControl selectedFrame;

    private void Start()
    {
        textBox = GameObject.Find("TextBox").GetComponent<textBox>();
        inventory = new List<item>();
        inventorySlots = new InventoryItem[10];
        selectedFrame = GetComponentInChildren<frameControl>();
        selectedItem = 0;

        Vector3 localPos = Vector3.zero;
        localPos.y = 64;
        GameObject slotTemplate = Resources.Load("Inventory Item") as GameObject;
        for(int x = 0;x < 10; x++)
        {
            localPos.x = 128 * x -576;
            InventoryItem newSlot = Instantiate(slotTemplate, this.transform).GetComponent<InventoryItem>();
            newSlot.slotNumber = x;
            newSlot.GetComponent<RectTransform>().localPosition = localPos;
            newSlot.image = newSlot.GetComponent<Image>();
            newSlot.im = this;
            inventorySlots[x] = newSlot;
        }

        for (int x = 0; x < defaultInventory.Length; x++)
            addItem(defaultInventory[x]);
    }

    public void itemClicked(int x)
    {
        if (x == 9 && inventory.Count >= 11)
        {
            refreshInventory(currentPage + 1);
        }
        else
        {
            item currentItem = inventory[selectedItem];
            int targetItemSlot = 9 * currentPage + x;
            item targetItem = inventory[targetItemSlot];

            if (currentItem == targetItem)
            {
                textBox.displayText(currentItem.lookDescription[Random.Range(0, currentItem.lookDescription.Length)]);
            }
            //fusion script here
            else
            {
                selectedItem = targetItemSlot;
                selectedFrame.setSlot(x);
            }
        }


    }

    public void addItem(item item)
    {
        inventory.Add(item);
        refreshInventory(inventory.Count / 9);
    }

    public void removeItem(item item)
    {
        item currentItem = inventory[selectedItem];
        if (currentItem == item)
            selectedItem = 0;
        inventory.Remove(item);
        selectedItem = inventory.FindIndex(i => i == item);
        refreshInventory(currentPage);
    }

    public void refreshInventory(int page)
    {
        if (inventory.Count <= 10)
        {
            for (int x = 0; x < 10; x++)
            {
                if (x < inventory.Count)
                    inventorySlots[x].image.sprite = inventory[x].icon;
                else
                    inventorySlots[x].image.sprite = blank;
            }

            currentPage = 0;
            selectedFrame.setSlot(selectedItem);
        }
        else
        {
            selectedFrame.hide();
            if (page > inventory.Count / 9)
                page = 0;
            for (int x = 0; x < 9; x++)
            {
                if (9*page + x < inventory.Count)
                    inventorySlots[x].image.sprite = inventory[9*page + x].icon;
                else
                    inventorySlots[x].image.sprite = blank;

                if (selectedItem == 9 * page + x)
                    selectedFrame.setSlot(x);
            }
            inventorySlots[9].image.sprite = nextArrow;

            currentPage = page;
        }
    }
}