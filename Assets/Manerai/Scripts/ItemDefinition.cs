using UnityEngine;
using System;

public enum ItemType
{
    Weapon,
    KitchenUtensil,
    Tool
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/ItemDefinition")]
public class ItemDefinition : ScriptableObject
{
    [Header("Item Details")]
    [Tooltip("Name of the item")]
    public string itemName;

    [Tooltip("Unique identifier for the item")]
    public string identifier = Guid.NewGuid().ToString();

    [Tooltip("Weight of the item")]
    public float weight;

    [Tooltip("Type of the item (Weapon, Kitchen Utensil, Tool)")]
    public ItemType type;

    [Tooltip("Sprite image of the item")]
    public Sprite itemSprite;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(identifier))
        {
            identifier = Guid.NewGuid().ToString();
        }
    }
}
