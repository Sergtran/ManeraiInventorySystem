using UnityEngine;
using UnityEngine.EventSystems;

public class BackpackHandler : MonoBehaviour, IInputHandler
{
    [SerializeField] private GameObject _backpackUI;

    private GameObject _currentItemDataHolder;

    private void Start()
    {
        SetBackpackVisibility(false);
    }

    public void OnSelect()
    {
        SetBackpackVisibility(true);
    }

    public void OnDrag()
    {
        // Optional: Implement drag functionality if needed in the future.
    }

    public void OnRelease()
    {
        DetectReleaseTarget();
        SetBackpackVisibility(false);

        if (_currentItemDataHolder == null) return;

        if (_currentItemDataHolder.TryGetComponent(out ItemDataHolder itemData))
        {
            GameManager.Instance.InventoryItemCollector.RemoveItemFromInventory(itemData.ItemData);
        }
    }

    private void DetectReleaseTarget()
    {
        var pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var raycastResults = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        _currentItemDataHolder = raycastResults.Count > 0 ? raycastResults[0].gameObject : null;
    }

    private void SetBackpackVisibility(bool isVisible)
    {
        _backpackUI?.SetActive(isVisible);
    }
}
