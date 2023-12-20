using UnityEngine;

public class Movement : MonoBehaviour
{
    public Camera mainCamera;
    public LineRenderer _lineRenderer;
    public DistanceJoint2D _Distancejoint;
    public Rigidbody2D rb;
    public float force;
    private Vector3 MouseDir;
    public Transform LinePosition;
    public bool isGrappling;
    public Transform lookToHook;
    void Start()
    {
        isGrappling = true;
        _Distancejoint.autoConfigureDistance = true;
        _Distancejoint.enabled = false;
        _lineRenderer.enabled = false;
    }

    
    void Update()
    {
        MouseDir = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (isGrappling)
        {
            _lineRenderer.SetPosition(0, LinePosition.position);
            _lineRenderer.SetPosition(1, transform.position);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null)
                {
                    Vector2 mousepos = hit.point;
                    _Distancejoint.connectedAnchor = mousepos;
                    _Distancejoint.enabled = true;
                    LinePosition.position = mousepos;
                }
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                _lineRenderer.SetPosition(1, transform.position);
                _lineRenderer.enabled = true;
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                _Distancejoint.enabled = false;
                _lineRenderer.enabled = false;
            }

            if (_Distancejoint.enabled)
            {
                _lineRenderer.SetPosition(1, transform.position);
            }

            if (Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.Mouse0))
            {
                Vector3 Direction = LinePosition.position - transform.position;
                rb.AddForce(new Vector2(force * Time.deltaTime, 0), ForceMode2D.Force);
                _Distancejoint.enabled = true;
            }

            if (Input.GetKeyUp(KeyCode.E) && Input.GetKey(KeyCode.Mouse0))
            {
                _Distancejoint.enabled = true;
            }
        }
    }
}