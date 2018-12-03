using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class playerController : MonoBehaviour {

    public float walkSpeed = 5f;

    [HideInInspector]
    public bool busy = false;

    private DataBucket db;
    private InventoryManager im;
    private SkeletonGraphic spine;
    private textBox textBox;
    private RectTransform rt;
    private roomController roomController;
    private AudioSource SFX;

    private GameObject clickable;
    private Transform objectsLayer;

    private string currentAnimation = "idle";

	// Use this for initialization
	void Start () {
        db = GameObject.Find("DataBucket").GetComponent<DataBucket>();
        im = GameObject.Find("Inventory").GetComponent<InventoryManager>();
        rt = GetComponent<RectTransform>();
        textBox = GameObject.Find("TextBox").GetComponent<textBox>();
        spine = GetComponent<SkeletonGraphic>();
        roomController = GetComponentInParent<roomController>();
        SFX = GameObject.Find("SFX").GetComponent<AudioSource>();

        clickable = Resources.Load("Clickable") as GameObject;
        objectsLayer = GameObject.Find("Objects").transform;
	}
	
    public void clickedObject(clickableController target)
    {
        if(!busy)
            StartCoroutine(playerAction(target, im.SelectedItem()));
    }

    private IEnumerator playerAction(clickableController targetController, item item)
    {
        busy = true;

        clickable target = targetController.clickable;

        if (spine.enabled == true)
        {

            Vector3 pos = rt.localPosition;

            //move to target
            Vector3 moveDirection = (target.moveTo - pos).normalized;

            currentAnimation = "idle";

            while (Vector3.Distance(target.moveTo, pos) > walkSpeed)
            {
                if (currentAnimation == "idle")
                {
                    spine.AnimationState.SetAnimation(0, "walk", true);
                    currentAnimation = "walk";
                    spine.Skeleton.FlipX = moveDirection.x > 0;
                }

                pos += moveDirection * walkSpeed;
                rt.localPosition = pos;
                yield return null;
            }

            rt.localPosition = target.moveTo;
            spine.Skeleton.FlipX = target.facingRight;

        }
        characterAnimation animation;
        clickableAction[] actions;
        string text;

        if (item.name == "Hand")
        {
            animation = target.handAnimation;
            actions = target.handActions;
            text = target.handText;
        }
        else if (item == target.correctItem)
        {
            if (target.consumeItem)
                im.removeItem(item);
            animation = target.correctAnimation;
            actions = target.correctActions;
            text = target.correctText;
        }
        else
        {
            animation = target.wrongAnimation;
            actions = target.wrongActions;
            if (target.overrideWrongText)
                text = target.wrongOverrideText;
            else text = item.useWrongText[Random.Range(0, item.useWrongText.Length)];
        }

        if (spine.enabled == true)
        {
            string nextAnimation;

            switch (animation)
            {
                case characterAnimation.reachHigh: nextAnimation = "interacthighTWEEN"; break;
                case characterAnimation.reachMid: nextAnimation = "interactmidTWEEN"; break;
                case characterAnimation.reachLow: nextAnimation = "interactlowTWEEN"; break;
                default: nextAnimation = "idle"; break;
            }

            if (nextAnimation != "idle")
            {
                spine.AnimationState.SetAnimation(0, nextAnimation, false);
                spine.AnimationState.AddAnimation(0, "idle", true, .5f);
                yield return new WaitForSeconds(.5f);
            }
            else if (currentAnimation != "idle")
                spine.AnimationState.AddAnimation(0, "idle", true, 0f);


            currentAnimation = "idle";
        }

        busy = false;

        if (text != "")
            textBox.displayText(text);

        foreach (clickableAction a in actions)
        {
            switch(a.action)
            {
                case action.removeThis: roomController.removeFromRoom(targetController.gameObject); break;
                case action.addClickable:
                    roomController.addToRoom(a.clickable);
                    break;
                case action.addItemToInventory:
                    im.addItem(a.item);
                    if (item.name == "Sandal")
                        SFX.PlayOneShot((AudioClip) Resources.Load("SFX/flying"));
                    else SFX.PlayOneShot((AudioClip)Resources.Load("SFX/pickuptune"));
                    break;
                case action.codeAction:
                    SpecialAction(target, item);
                    break;
                default: break;
            }
        }
    }

    private void SpecialAction(clickable target, item item)
    {
        Debug.Log(target.name);
        if (target.name == "Door Desk")
            roomController.moveToRoom("Room Desk Closeup");

        if (target.name == "Door Back Arrow")
            roomController.moveToRoom("Room Main Temple");
    }
}
