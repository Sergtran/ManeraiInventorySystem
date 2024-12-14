using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class InventoryServerNotifier : MonoBehaviour, IInventoryListener
{
    private const string ServerUrl = "https://wadahub.manerai.com/api/inventory/status";
    private const string AuthorizationKey = "Bearer kPERnYcWAY46xaSy8CEzanosAgsWM84Nx7SKM4QBSqPq6c7StWfGxzhxPfDh8MaP";

    private void Start()
    {
        GameManager.Instance.InventoryItemCollector.AddListener(this);
    }
    public void OnItemAdded(ItemData itemData)
    {
        SendInventoryEvent(itemData.ItemDefinition.identifier, "added");
    }

    public void OnItemRemoved(ItemData itemData)
    {
        SendInventoryEvent(itemData.ItemDefinition.identifier, "removed");
    }

    private void SendInventoryEvent(string itemId, string eventType)
    {
        InventoryEventPayload payload = new InventoryEventPayload
        {
            item_id = itemId,
            event_type = eventType
        };

        string jsonData = JsonUtility.ToJson(payload);
        StartCoroutine(SendPostRequest(ServerUrl, jsonData));
    }

    private IEnumerator SendPostRequest(string url, string jsonData)
    {
        // Create a UnityWebRequest for a POST request
        using UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        // Set request body and headers
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", AuthorizationKey);

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        // Handle the response
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Request successful: {request.downloadHandler.text}");
        }
        else
        {
            Debug.LogError($"Request failed: {request.error}");
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.InventoryItemCollector.RemoveListener(this);
    }
}

[Serializable]
public class InventoryEventPayload
{
    public string item_id;
    public string event_type;
}
