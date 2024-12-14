using System;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    [SerializeField] LayerMask _collisionMask;
    private List<Collider> _currFrameCollisions = new List<Collider>();
    private List<Collider> _lastFrameCollisions = new List<Collider>();
    private Collider[] _collisionsDetected = new Collider[20];
    private Collider _mainCollider;
    private bool _foundOnCurr = false;
    private Collider _currCollision;
    private bool _isActived= false;

    public Action<Collider> OnCollisionEnter;
    public Action<Collider> OnCollisionStay;
    public Action<Collider> OnCollisionExit;

    public bool IsActived { get => _isActived; set => _isActived = value; }

    public void Start()
    {
        if (_mainCollider==null) TryGetComponent(out _mainCollider);
        _lastFrameCollisions.Clear();
        _currFrameCollisions.Clear();
    }

    public void Update()
    {
        if (!_isActived) return;
        _lastFrameCollisions = _currFrameCollisions.GetRange(0, _currFrameCollisions.Count);
        _currFrameCollisions.Clear();
        int objsHittedAmount = 0;
    
        if (_mainCollider is BoxCollider boxCollider)
        {
            Vector3 worldHalfExtents = Vector3.Scale(boxCollider.size / 2, boxCollider.transform.lossyScale);
            objsHittedAmount = Physics.OverlapBoxNonAlloc(boxCollider.transform.position, worldHalfExtents, _collisionsDetected, boxCollider.transform.rotation, _collisionMask);
        }
        else if (_mainCollider is SphereCollider sphereCollider)
        {
            var pos = sphereCollider.transform.position;
            var rad = sphereCollider.radius;

            objsHittedAmount = Physics.OverlapSphereNonAlloc(pos, rad, _collisionsDetected, _collisionMask);
        }

        for (int i = 0; i < objsHittedAmount; i++)
        {
            var collidedObj = _collisionsDetected[i];
            if (collidedObj == _mainCollider) continue;
            _currFrameCollisions.Add(collidedObj);
            var wasAlreadyColliding = _lastFrameCollisions.Contains(collidedObj);
            if (!wasAlreadyColliding)
            {
                OnCollisionEnter?.Invoke(collidedObj);
            }
        }

        for (int i = 0; i < _lastFrameCollisions.Count; i++)
        {
            var lastCollision = _lastFrameCollisions[i];
            _foundOnCurr = false;    
            for (int j = 0; j < _currFrameCollisions.Count; j++)
            {
                _currCollision = _currFrameCollisions[j];
                if (_currCollision == lastCollision)
                {
                    _foundOnCurr = true;                    
                    break;
                }
            }
            if (_foundOnCurr)
            {
                OnCollisionStay?.Invoke(_currCollision);
            }
            else
            {
                OnCollisionExit?.Invoke(lastCollision);
            }
        }
    }
    
    public void GetCollisionsOfType<T>(List<T> results)
    {
        results.Clear();
        for (int i = 0; i < _currFrameCollisions.Count; i++)
        {
            if (_currFrameCollisions[i].TryGetComponent<T>(out T TypeObject))
            {
                results.Add(TypeObject);
            }
        }
    }
}

