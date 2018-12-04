using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

    public Sprite nextArrow;
    public Sprite blank;

    [Header("Initial Inventory")]
    public item[] defaultInventory;

    [Header("Recipe Book")]
    public recipeSO recipeBook;

    [Header("Full Screen Displays")]
    public float timeToDisplayBook;
    public Sprite journalImage;
    public Sprite brochureImage;

    [Header("Special items")]
    public item dimmedOrb;

    private InventoryItem[] inventorySlots;
    private List<item> inventory;
    private textBox textBox;
    private AudioSource SFX;
    private playerController player;
    private Image fullScreenImage;

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

        SFX = GameObject.Find("SFX").GetComponent<AudioSource>();
        player = GameObject.Find("Protag").GetComponent<playerController>();
        fullScreenImage = GameObject.Find("Fullscreen Image").GetComponent<Image>();

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

    public item SelectedItem()
    {
        return inventory[selectedItem];
    }

    public void itemClicked(int x)
    {
        if (player.busy)
            return;

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
                if(currentItem.name == "Journal" || currentItem.name == "Brochure")
                {
                    SFX.PlayOneShot((AudioClip)Resources.Load("SFX/page turn"));
                    StartCoroutine(showFullscreen(currentItem.name));
                }
                else
                    textBox.displayText(currentItem.lookDescription[Random.Range(0, currentItem.lookDescription.Length)]);
            }

            bool combine = false;

            foreach (recipe r in recipeBook.recipes)
            {
                if (currentItem == r.item1 && targetItem == r.item2 || targetItem == r.item1 && currentItem == r.item2)
                {
                    Debug.Log(r.result.name);
                    if (r.result.name == "Bloody Knife")
                    {
                        if (currentItem.name == "Knife" && targetItem.name == "Hand")
                        {
                            textBox.displayText("Ow! Damnit! I cut myself!");
                            inventory[selectedItem] = r.result;
                            refreshInventory(currentPage);
                            combine = true;
                        }            
                    }
                    else
                    {
                        inventory.Remove(currentItem);
                        inventory.Remove(targetItem);
                        addItem(r.result);
                        currentItem = r.result;
                        targetItem = null;
                        selectedItem = inventory.FindIndex(i => i == r.result);
                        combine = true;

                        if (selectedItem >= 9 && inventory.Count > 10)
                            selectedFrame.setSlot(selectedItem % 9);
                        else selectedFrame.setSlot(selectedItem);
                    }
                }
            }

            if (combine == true)
            {
                SFX.PlayOneShot((AudioClip) Resources.Load("SFX/combinetune"));
            }
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

    IEnumerator showFullscreen(string item)
    {
        player.busy = true;
        float scale = GameObject.Find("Canvas").GetComponent<Canvas>().scaleFactor;

        if (item == "Journal")
            fullScreenImage.overrideSprite = journalImage;
        if (item == "Brochure")
            fullScreenImage.overrideSprite = brochureImage;

        float startTime = Time.time;
        RectTransform rt = fullScreenImage.GetComponent<RectTransform>();

        Vector3 pos = rt.position;

        while (Time.time - startTime < timeToDisplayBook && Input.GetMouseButtonDown(0) == false)
        {
            pos.y = (-544 + 672 * (Time.time - startTime) / timeToDisplayBook) * scale;
            rt.position = pos;
            yield return null;
        }

        pos.y = 128 * scale;
        rt.position = pos;
        yield return null;

        while (Input.GetMouseButtonDown(0) == false)
        {
            yield return null;
        }

        startTime = Time.time;
        yield return null;

        while (Time.time - startTime < timeToDisplayBook && Input.GetMouseButtonDown(0) == false)
        {
            pos.y = (128 - 672 * (Time.time - startTime) / timeToDisplayBook) * scale;
            rt.position = pos;
            yield return null;
        }

        pos.y = -544 * scale;
        rt.position = pos;

        player.busy = false;
    }

    public bool haveDimmedOrb()
    {
        return inventory.Contains(dimmedOrb);
    }
}