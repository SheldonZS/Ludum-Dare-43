using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Room", menuName = "Scriptable Objects/room")]
public class roomSO : ScriptableObject {

    public Sprite roomImage;
    public bool playerVisible;
    public clickable[] clickables;

}
