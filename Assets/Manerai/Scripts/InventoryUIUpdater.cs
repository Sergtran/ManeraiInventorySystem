using UnityEngine;
using System.Collections.Generic;
using System;

public class InventoryUIUpdater : MonoBehaviour, IInventoryListener
{
    [SerializeField] private ItemTypeHolder[] _itemTypeHolder;

    private Dictionary<ItemType, ItemDataHolder> _itemTypeToHolder;

    private void Start()
    {
        GameManager.Instance.InventoryItemCollector.AddListener(this);

        _itemTypeToHolder = new Dictionary<ItemType, ItemDataHolder>();

        foreach (var itemType in _itemTypeHolder)
        {
            if (!_itemTypeToHolder.ContainsKey(itemType.itemType))
            {
                _itemTypeToHolder.Add(itemType.itemType, itemType.itemDataHolder);
            }
        }
    }

    public void OnItemAdded(ItemData itemData)
    {
        if (!_itemTypeToHolder.TryGetValue(itemData.ItemDefinition.type, out ItemDataHolder itemDataHolder)) return;
        itemDataHolder.ItemData = itemData;
        itemDataHolder.SetData(true);
    }

    public void OnItemRemoved(ItemData itemData)
    {
        if (!_itemTypeToHolder.TryGetValue(itemData.ItemDefinition.type, out ItemDataHolder itemDataHolder)) return;
        itemDataHolder.ItemData = itemData;
        itemDataHolder.SetData(false);
    }

    private void OnDestroy()
    {
        GameManager.Instance.InventoryItemCollector.RemoveListener(this);
    }
}

[Serializable]
public class ItemTypeHolder
{
    public ItemType itemType; 
    public ItemDataHolder itemDataHolder;
}
