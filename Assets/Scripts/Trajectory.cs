using System;
using Unity.VisualScripting;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] LayerMask nonPlayer;
    private LineRenderer _lr;
    public Vector3 tempVec;
    bool flag;  
    public int bounceCount;
    public bool CharacterAim;

    Color c1 = Color.white;
    Color c2 = Color.blue;
    Color c3 = Color.red;

    void Start()
    {
        _lr = GetComponent<LineRenderer>();
        _lr.numCapVertices = 40;
        _lr.startWidth = 0.25f;
        _lr.endWidth = 0.02f;
    }

    public Vector3[] Plot(Vector3 pos, Vector3 force, int steps)
    {
        bounceCount = 0;
        Vector3[] results = new Vector3[steps];
        float timeStep = Time.fixedDeltaTime;

        Vector3 moveStep = force * timeStep;

        for (int i = 0; i < steps; i++)
        {
            RaycastHit hit;

            if (Physics.Raycast(pos, moveStep, out hit, moveStep.magnitude, nonPlayer))
            {
                // Debug.Log($"Raycast hit: {hit.collider.gameObject.name} with tag {hit.collider.gameObject.tag}");

                if (hit.collider.CompareTag("bouncy"))
                {
                    CharacterAim = false;
                    moveStep = Vector3.Reflect(moveStep, hit.normal);
                    Debug.DrawLine(hit.point, force, Color.red);
                    bounceCount++;
                }
                else if (hit.collider.gameObject.CompareTag("enemy"))
                {
                    CharacterAim = true;
                    Debug.Log("Enemy Detected!");
                    _lr.startColor = c3; 
                    tempVec = pos;
                    return results;
                    
                }
                else if (hit.collider.gameObject.CompareTag("ground"))
                {
                    CharacterAim = false;
                    _lr.startColor = c1; 
                    Debug.Log("Platform Detected!");
                    tempVec = pos;
                    return results;
                }
            }
            pos += moveStep;
            results[i] = pos;
        }

        if (bounceCount > 0)
        {
            _lr.startColor = c2;
        }
        else
        {
            _lr.startColor = c1;
        }

        tempVec = pos;

        return results;
    }

    public void RenderTrajectory(Vector3[] trajectory)
    {
        int length = 0;
        for (int i = 0; i < trajectory.Length; i++)
        {
            if (trajectory[i] != Vector3.zero)
            {
                length++;
            }
        }

        Vector3[] temp = new Vector3[length];
        int tempindex = 0;
        for (int i = 0; i < trajectory.Length; i++)
        {
            if (trajectory[i] != Vector3.zero)
            {
                temp[tempindex++] = trajectory[i];
            }
        }

        _lr.positionCount = length;
        _lr.SetPositions(temp);
    }

    public void EndLine02()
    {
        _lr.positionCount = 0;
    }
    
}
