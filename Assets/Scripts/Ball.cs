using UnityEngine;

public class LaunchCharacter : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 direction;
    private Vector3 directionOpposite;
    private float maxPower = 30f; 
    private float minPower = 10f;  
    private float power;
    private bool isDragging = false;
    [SerializeField] private int numberOfRays;
    [SerializeField] private LineRenderer line;

    public LineRenderer dragLine; 

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dragLine.positionCount = 2; 
        dragLine.enabled = false; 
        line.positionCount = 2;
        
    }

    void Update()
    {
        

        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            startPos = transform.position;
            dragLine.enabled = true;
            line.enabled = true;
            dragLine.SetPosition(0, startPos);
            line.SetPosition(0, startPos);
            rb.velocity = Vector3.zero;

        }

        if (isDragging)
        {

            rb.useGravity = false;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z; 

            endPos = Camera.main.ScreenToWorldPoint(mousePos); 
            direction = (startPos - endPos).normalized;
            directionOpposite = (endPos - startPos).normalized;

            float distance = Vector3.Distance(startPos, endPos);
            power = Mathf.Clamp(distance * 2f, minPower, maxPower);

            CastRay(transform.position, -directionOpposite);
            dragLine.SetPosition(1, endPos); 

        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            dragLine.enabled = false;
            line.enabled = false;
            launchCharacter();
        }
        
    }

    private void CastRay(Vector3 rayPos, Vector3 rayDir)
    {

        for (var i = 0; i < numberOfRays; i++)
        {
            var ray = new Ray(rayPos, rayDir);
            bool flag = Physics.Raycast(ray, out var rayHit, 10);
            if (flag && rayHit.collider.CompareTag("bouncy"))
            {
                if (line.positionCount < numberOfRays + 1)
                {
                    line.positionCount++;
                }
                line.SetPosition(i + 1, rayHit.point);
                Debug.DrawLine(rayPos, rayHit.point, Color.blue);
                rayPos = rayHit.point;
                rayDir = Vector3.Reflect(rayPos, rayHit.normal);

            }
            else
            {
                line.SetPosition(i + 1, rayHit.point);
                line.positionCount = 2 + i;
                Debug.DrawRay(rayPos, direction*power, Color.red);
                break;
            }
        }

    }
    void launchCharacter()
    {
        rb.velocity = direction * power;
    }

}

