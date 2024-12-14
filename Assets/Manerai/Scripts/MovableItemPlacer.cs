using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableItemPlacer : MonoBehaviour, IInventoryListener
{
    [SerializeField] private ItemTypePosition[] _itemTypePositions;
    [SerializeField] private LayerMask _movableLayer;
    [SerializeField] private float _smoothDuration = 1f;
    [SerializeField] private Transform _parentObjects;

    private Dictionary<ItemType, Transform> _itemTypeToPositionMap;

    
    private void Start()
    {
        GameManager.Instance.InventoryItemCollector.AddListener(this);
        InitializePositionMap();
    }

    private void InitializePositionMap()
    {
        _itemTypeToPositionMap = new Dictionary<ItemType, Transform>();

        foreach (var item in _itemTypePositions)
        {
            if (item.targetPosition != null && !_itemTypeToPositionMap.ContainsKey(item.itemType))
            {
                _itemTypeToPositionMap.Add(item.itemType, item.targetPosition);
            }
        }
    }

    public void OnItemAdded(ItemData itemData)
    {
        HandleItemPlacement(itemData, lockObject: true);
    }

    public void OnItemRemoved(ItemData itemData)
    {
        HandleItemPlacement(itemData, lockObject: false);
    }

    private void HandleItemPlacement(ItemData itemData, bool lockObject)
    {
        MovableObject movable = itemData.GetComponent<MovableObject>();

        if (lockObject) PositionMovableObject(itemData, movable);
        else ResetMovableObject(movable);
        
    }

    private void PositionMovableObject(ItemData itemData, MovableObject movable)
    {
        if (!_itemTypeToPositionMap.TryGetValue(itemData.ItemDefinition.type, out Transform target)) return;
        movable.LockObject(true);
        StartCoroutine(SmoothMoveAndRotate(movable, target.position, target.rotation));
        movable.transform.SetParent(target, true);
    }

    private void ResetMovableObject(MovableObject movable)
    {
        movable.LockObject(false);
        StartCoroutine(SmoothMoveAndRotate(movable, movable.InitialPosition, movable.InitialRotation));
        movable.transform.SetParent(_parentObjects, true);
    }

    private IEnumerator SmoothMoveAndRotate(MovableObject movable, Vector3 targetPosition, Quaternion targetRotation)
    {
        Vector3 startPosition = movable.transform.position;
        Quaternion startRotation = movable.transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < _smoothDuration)
        {
            float t = elapsedTime / _smoothDuration;
            movable.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            movable.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        movable.transform.position = targetPosition;
        movable.transform.rotation = targetRotation;
    }

    private void OnDestroy()
    {
        GameManager.Instance.InventoryItemCollector.RemoveListener(this);
    }
}

[Serializable]
public class ItemTypePosition
{
    public ItemType itemType;
    public Transform targetPosition;
}
