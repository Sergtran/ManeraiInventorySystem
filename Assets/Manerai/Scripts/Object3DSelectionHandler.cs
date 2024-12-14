using System.Collections.Generic;
using UnityEngine;

public interface IInputHandler
{
    public void OnSelect();

    public void OnDrag();

    public void OnRelease();
}

public class Object3DSelectionHandler : MonoBehaviour
{
    [SerializeField] private LayerMask _interactableLayer;
    private Camera _mainCamera;
    private List<IInputHandler> _currentHandlers = new List<IInputHandler>(); // List to store active handlers
    private RaycastHit[] hits = new RaycastHit[3];

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleSelect();
        }
        else if (Input.GetMouseButton(0))
        {
            HandleDrag();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            HandleRelease();
        }
    }

    private void HandleSelect()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        int hitCount = Physics.RaycastNonAlloc(ray, hits, Mathf.Infinity, _interactableLayer);
        if (hitCount > 0)
        {
            RaycastHit closestHit = hits[0];
            for (int i = 1; i < hitCount; i++)
            {
                if (hits[i].distance < closestHit.distance)
                {
                    closestHit = hits[i];
                }
            }

            // Retrieve all IInputHandler components from the closest hit
            var handlers = closestHit.collider.GetComponents<IInputHandler>();
            if (handlers.Length > 0)
            {
                _currentHandlers.AddRange(handlers);
                foreach (var handler in handlers)
                {
                    handler.OnSelect(); 
                }
            }
        }
    }

    private void HandleDrag()
    {
        if (_currentHandlers.Count == 0) return;

        foreach (var handler in _currentHandlers)
        {
            handler.OnDrag(); 
        }
    }

    private void HandleRelease()
    {
        if (_currentHandlers.Count == 0) return;

        foreach (var handler in _currentHandlers)
        {
            handler.OnRelease(); 
        }

        _currentHandlers.Clear(); 
    }

    public void ForceRelease()
    {
        HandleRelease();
    }
}

public enum ObjectInputHandlerState
{
    Select = 0,
    Drag = 1,
    Release = 2,
}