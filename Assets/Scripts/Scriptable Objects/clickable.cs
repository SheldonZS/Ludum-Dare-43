using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "clickable", menuName = "Scriptable Objects/clickable")]
public class clickable : ScriptableObject {

    public Sprite sprite;
    public Vector3 position;
    public Vector2 size;

    [Header("On Click")]
    public Vector3 moveTo;
    public bool facingRight;
    
    [Header("Hand Action")]
    public characterAnimation handAnimation;
    public string handText;
    public clickableAction[] handActions;

    [Header("Use Item")]
    public item correctItem;
    public bool consumeItem;
    public characterAnimation correctAnimation;
    public string correctText;
    public clickableAction[] correctActions;

    [Header("Wrong Item")]
    public characterAnimation wrongAnimation;
    public bool overrideWrongText;
    public string wrongOverrideText;
    public clickableAction[] wrongActions;

}

public enum characterAnimation { none, reachHigh, reachMid, reachLow }
public enum action { none, removeThis, addClickable, addItemToInventory, codeAction}

[System.Serializable]
public class clickableAction
{
    public action action;
    public item item;
    public clickable clickable;
}
