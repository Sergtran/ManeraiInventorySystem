using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TextMessageHandler : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private float _displayDuration = 3f;
    private Coroutine _currentCoroutine;

    private void Start()
    {
        _panel.SetActive(false);
    }
    public void ShowMessage(string message)
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        _currentCoroutine = StartCoroutine(DisplayMessageCoroutine(message));
    }

    private IEnumerator DisplayMessageCoroutine(string message)
    {
        _messageText.text = message;
        _panel.SetActive(true);
        yield return new WaitForSeconds(_displayDuration);
        _panel.SetActive(false);
        _messageText.text = string.Empty;
        _currentCoroutine = null; 
    }
}
