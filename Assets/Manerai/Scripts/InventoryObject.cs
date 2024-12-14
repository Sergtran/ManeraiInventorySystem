using UnityEngine;

public class InventoryObject : MonoBehaviour, IInputHandler
{   
    [SerializeField] private ItemData _itemData;
    [SerializeField] private CollisionController _collisionController;
    private ObjectInputHandlerState _objectInputHandlerState;
    private InventoryItemCollector _inventoryItemCollector;
    private void Start()
    {
        if (_itemData == null) _itemData = GetComponent<ItemData>();
        if (_collisionController == null) _collisionController = GetComponent<CollisionController>();
        _collisionController.OnCollisionStay += CollisionStay;
        _inventoryItemCollector = GameManager.Instance.InventoryItemCollector;
    }

    public void OnSelect()
    {
        _collisionController.IsActived = true;
        _objectInputHandlerState = ObjectInputHandlerState.Select;
    }

    public void OnDrag()
    {
        _objectInputHandlerState = ObjectInputHandlerState.Drag;
    }

    public void OnRelease()
    {
        _objectInputHandlerState = ObjectInputHandlerState.Release;
    }

    public void CollisionStay(Collider collider)
    {
        BackpackHandler backpack = collider.GetComponent<BackpackHandler>();
        if (backpack == null || _objectInputHandlerState != ObjectInputHandlerState.Release) return;

        _inventoryItemCollector.AddItemToInventory(_itemData);
        _collisionController.IsActived = false;
    }
}


