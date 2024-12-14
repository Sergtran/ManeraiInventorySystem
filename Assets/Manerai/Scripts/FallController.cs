using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallController : MonoBehaviour
{
    [SerializeField] private CollisionController _collisionController;

    private void Start()
    {
        if (_collisionController == null) _collisionController = GetComponent<CollisionController>();
        _collisionController.OnCollisionExit += OnCollision;
        _collisionController.IsActived = true;
    }

    private void OnCollision(Collider collider)
    {
        MovableObject movableObject = collider.GetComponent<MovableObject>();
        if(movableObject == null) return;
        movableObject.ResetPos();
    }
}
