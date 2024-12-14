using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get; private set;
    }

    [SerializeField] private InventoryItemCollector _inventoryItemCollector;
    public InventoryItemCollector InventoryItemCollector
    {
        get => _inventoryItemCollector;
    }

    [SerializeField] private Object3DSelectionHandler _selectionHandler;
    public Object3DSelectionHandler SelectionHandler
    {
        get => _selectionHandler;
    }

    [SerializeField] private TextMessageHandler _errorHandler;
    public TextMessageHandler ErrorHandler
    {
        get => _errorHandler;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}

