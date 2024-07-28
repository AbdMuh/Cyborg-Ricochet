
using UnityEngine;
using UnityEngine.Serialization;


public class DragAndShoot : MonoBehaviour
{
    public Rigidbody rb;
    [SerializeField] public float power = 5f;

    private int bounceHit;
    private int bounceCount;

    public Vector2 minPower;
    public Vector2 maxPower;
    public GameObject spritePrefab;
    public GameObject instanstiatedSprite;
    float distToGround;

    private Camera _camera;
    private Vector3 _force;
    private Vector3 _startPoint;
    private Vector3 _endPoint;
    private LineTrail _trail;
    private Trajectory _trajectory;
    [SerializeField] private int steps;

    private Vector3 _tempVec;


    private bool dragforce;

    public bool isGrounded;
    private bool _buttonDown;
    private RaycastHit _hit;

    private void Start()
    {
        bounceHit = 0;
        _camera = Camera.main;
        _trail = GetComponent<LineTrail>();
        _trajectory = GetComponentInChildren<Trajectory>();
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }


    private void Update()
    {

        // instanstiatedSprite = Instantiate(spritePrefab, transform.position + (0.35f * new Vector3(0,3.3f,0)), Quaternion.identity);

        Vector3 boxCenter = transform.position + (0.35f * new Vector3(0, 2.27f, 0));
        Vector3 halfExtents = new Vector3(0.1f, 0.7f, 0.1f);
        float maxDistance = 0.2f;
        Quaternion orientation = Quaternion.identity;

        isGrounded = Physics.BoxCast(boxCenter, halfExtents, Vector3.down, out _hit, orientation, maxDistance);
        if (!isGrounded)
        {
            _trail.EndLine();
            _trajectory.EndLine02();
            Debug.Log("Not grounded");

            return;
        }

        if (isGrounded && rb.velocity == Vector3.zero)
        {

            if (Input.GetMouseButtonDown(0))
            {
                _buttonDown = true;
                _startPoint = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 3f));
            }

            if (_buttonDown)
            {


                if (Input.GetMouseButton(0))
                {
                    Vector3 currentPoint = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 3f));
                    _trail.RenderLine(currentPoint);


                    _force = new Vector3(Mathf.Clamp(_startPoint.x - currentPoint.x, minPower.x, maxPower.x),
                        Mathf.Clamp(_startPoint.y - currentPoint.y, minPower.y, maxPower.y), 0);


                    Vector3[] trajectory = _trajectory.Plot(transform.position, _force * power, steps);
                    _trajectory.RenderTrajectory(trajectory);
                }


                if (Input.GetMouseButtonUp(0))
                {
                    _buttonDown = false;
                    _tempVec = _trajectory.tempVec;
                    bounceCount = _trajectory.bounceCount;

                    _trail.EndLine();
                    _trajectory.EndLine02();
                    _endPoint = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 3f));

                    _force = new Vector3(Mathf.Clamp(_startPoint.x - _endPoint.x, minPower.x, maxPower.x),
                        Mathf.Clamp(_startPoint.y - _endPoint.y, minPower.y, maxPower.y), 0);
                    // rb.AddForce(_force * power, ForceMode.Impulse);
                    rb.velocity = _force * power;
                    rb.useGravity = false;
                    if (bounceCount == 0)
                    {
                        Debug.Log("1");
                        instanstiatedSprite = Instantiate(spritePrefab, _tempVec + _force.normalized * 0.2f, Quaternion.identity);

                    }

                }
            }

        }
        //Debug.Log("Bounce count: " + bounceCount);
        //Debug.Log("Bounce hit: "+bounceHit);
    }

    void FixedUpdate()
    {
        if (dragforce)
        {
            ApplyDrag();
        }
    }

    void ApplyDrag()
    {
        Vector3 dragForce = -1f * rb.velocity.sqrMagnitude * rb.velocity.normalized;

        rb.AddForce(dragForce);
    }

    private void resetDrag()
    {
        dragforce = false;
    }


    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "ground")
        {
            isGrounded = true;
            //Debug.Log("Grounded");
        }
        else
        {
            isGrounded = false;
            //Debug.Log("Not Grounded!");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("gravity_trigger"))
        {
            rb.useGravity = true;
            Destroy(instanstiatedSprite);
            dragforce = true;
            Invoke("resetDrag", 0.5f);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("bouncy"))
        {
            rb.useGravity = true;
            Destroy(instanstiatedSprite);
            resetDrag();
        }
        else if (other.gameObject.CompareTag("bouncy"))
        {
            bounceHit++;
            if ((bounceCount != 0 && bounceHit >= bounceCount))
            {
                Debug.Log("2");
                instanstiatedSprite = Instantiate(spritePrefab, _tempVec + _force.normalized * 0.2f, Quaternion.identity);
                bounceHit = 0;
                bounceCount = 0;
            }
        }
        if (other.gameObject.CompareTag("test_enemy"))
        {
            Destroy(other.gameObject);
        }
    }


}
