using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Objects/item")]
public class item : ScriptableObject {

    public string itemName = "new item";
    public Sprite icon;

    public string[] lookDescription;
    public string[] useWrongText;
}
