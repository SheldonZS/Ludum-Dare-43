using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Clickable", menuName = "Scriptable Objects/Clickable")]
public class clickable : ScriptableObject {

    public Sprite sprite;
    public Vector3 position;

    [Header("On Click")]
    public Vector2 moveTo;
    public bool facingRight;
    
    [Header("Hand Action")]
    public characterAnimation handAnimation;
    public string handText;
    public clickableAction[] handActions;

    [Header("Use Item")]
    public item[] item;
    public bool consumeItem;
    public characterAnimation correctAnimation;
    public clickableAction[] correctActions;

    [Header("Wrong Item")]
    public characterAnimation wrongAnimation;
    public string wrongOverrideText;

}

public enum characterAnimation { none, reachHigh, reachMid, reachLow }
public enum action { none, removeThis, addClickable, addItemToInventory, codeAction}

[System.Serializable]
public class clickableAction
{
    action action;
    item item;
    clickable clickable;
}
