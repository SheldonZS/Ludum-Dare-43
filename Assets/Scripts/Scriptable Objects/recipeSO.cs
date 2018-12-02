using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "recipe book", menuName = "Scriptable Objects/recipe book")]
public class recipeSO : ScriptableObject {

    public recipe[] recipes;
}

[System.Serializable]
public struct recipe
{
    public item item1;
    public item item2;
    public item result;
}