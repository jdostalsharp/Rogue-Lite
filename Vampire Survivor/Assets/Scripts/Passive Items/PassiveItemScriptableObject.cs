using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItemScriptableObject", menuName = "ScriptableObjects/Passive Item")]
public class PassiveItemScriptableObject : ScriptableObject
{
    [SerializeField]
    float multiplier;
    public float Multiplier { get => multiplier; private set => multiplier = value; }

    [SerializeField]
    int level;
    public int Level { get => level; private set => level = value; }

    [SerializeField]
    GameObject nextLevelPrefab;  // The prefab of the objects next level.
    public GameObject NextLevelPrefab { get => nextLevelPrefab; private set => nextLevelPrefab = value; }

    [SerializeField]
    new string name;

    public string Name { get => name; private set => name = value; }

    [SerializeField]
    string description; // What is the dcscription of this item? [if this item is an upgrade, place the description of the upgrade]
    public string Description { get => description; private set => description = value; }

    [SerializeField]
    Sprite icon;    // Not meant to be modified in game
    public Sprite Icon { get => icon; private set => icon = value; }
}
