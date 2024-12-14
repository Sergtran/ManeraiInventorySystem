using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IInventoryListener
{
    public void OnItemAdded(ItemData itemData);
    public void OnItemRemoved(ItemData itemData);
}

public class InventoryItemCollector : MonoBehaviour
{
    [SerializeField] private CollisionController _collisionController;
    public Action<String> OnErrorMessage; 
    public UnityEvent<ItemData> _onItemAdded = new UnityEvent<ItemData>();
    public UnityEvent<ItemData> _onItemRemoved = new UnityEvent<ItemData>();
    private List<IInventoryListener> _listeners = new List<IInventoryListener>(); 
    private List<ItemData> _inventory = new List<ItemData>();
        
    public void SetCollisionBehaviour(bool state)
    {
        _collisionController.IsActived=state;
    }

    public void AddItemToInventory(ItemData itemData)
    {
        if (ObjectOnInventary(itemData))
        {
            GameManager.Instance.ErrorHandler.ShowMessage("You don't have space for " + itemData.ItemDefinition.type.ToString() + " object"); 
            return;
        }

        _inventory.Add(itemData);
        NotifyItemAdded(itemData);
        _onItemAdded.Invoke(itemData);
        GameManager.Instance.ErrorHandler.ShowMessage(itemData.ItemDefinition.name + " addeed");
    }

    public void RemoveItemFromInventory(ItemData itemData)
    {
        if (_inventory.Contains(itemData))
        {
            _inventory.Remove(itemData);
            NotifyItemRemoved(itemData);
            _onItemAdded.Invoke(itemData);
            GameManager.Instance.ErrorHandler.ShowMessage(itemData.ItemDefinition.name + " removed");
        }
    }

    public bool ObjectOnInventary(ItemData itemData)
    {
        if (_inventory.Exists(item => item.ItemDefinition.type == itemData.ItemDefinition.type))
        {
            return true;
        }
        return false;
    }

    public void AddListener(IInventoryListener listener)
    {
        if (!_listeners.Contains(listener))
        {
            _listeners.Add(listener);
        }
    }

    public void RemoveListener(IInventoryListener listener)
    {
        if (_listeners.Contains(listener))
        {
            _listeners.Remove(listener);
        }
    }

    private void NotifyItemAdded(ItemData itemData)
    {
        foreach (var listener in _listeners)
        {
            listener.OnItemAdded(itemData);
        }
    }

    private void NotifyItemRemoved(ItemData itemData)
    {
        foreach (var listener in _listeners)
        {
            listener.OnItemRemoved(itemData);
        }
    }
}

