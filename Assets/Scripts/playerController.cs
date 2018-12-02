using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class playerController : MonoBehaviour {

    public float walkSpeed;
    public bool busy = false;

    private DataBucket db;
    private InventoryManager im;
    private SkeletonGraphic spine;

	// Use this for initialization
	void Start () {
        db = GameObject.Find("DataBucket").GetComponent<DataBucket>();
        im = GameObject.Find("Inventory").GetComponent<InventoryManager>();
        spine = GetComponent<SkeletonGraphic>();
	}
	
    public void clickedObject(clickable target)
    {
        if(!busy)
            StartCoroutine(playerAction(target, im.SelectedItem()));
    }

    private IEnumerator playerAction(clickable target, item item)
    {
        busy = true;
        yield return null;
        busy = false;
    }
}
