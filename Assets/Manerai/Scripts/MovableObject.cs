using UnityEngine;

public class MovableObject : MonoBehaviour, IInputHandler
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private LayerMask _wallLayer;   
    [SerializeField] private LayerMask _groundLayer;   
    [SerializeField] private float _moveHeight = 0.6f;

    private RaycastHit[] hits = new RaycastHit[3];
    private bool _isLocked;

    private Vector3 _initialPosition;
    private Quaternion _initialRotation;

    public Vector3 InitialPosition { get => _initialPosition; }
    public Quaternion InitialRotation { get => _initialRotation; }

    private void Start()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody>();

        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
    }

    public void OnSelect()
    {
        if (_isLocked) return;

        _rb.useGravity = false;
        _rb.velocity = Vector3.zero;
    }

    public void OnDrag()
    {
        if (_isLocked) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int hitCount = Physics.RaycastNonAlloc(ray, hits, Mathf.Infinity);

        if (hitCount > 0)
        {
            float minDistance = float.MaxValue;
            Vector3 closestPoint = Vector3.zero;

            for (int i = 0; i < hitCount; i++)
            {
                if (hits[i].collider != null)
                {
                    float distance = Vector3.Distance(ray.origin, hits[i].point);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestPoint = hits[i].point;
                    }
                }
            }

            closestPoint.y = _moveHeight;
            transform.position = closestPoint;
        }
    }

    public void OnRelease()
    {
        _rb.useGravity = true;
    }

    public void LockObject(bool state)
    {
        _isLocked = state;
        _rb.useGravity = !state;
        _rb.isKinematic = state;
    }

    public void ResetPos()
    {
        transform.position = InitialPosition; 
        transform.rotation = InitialRotation;
    }
}
