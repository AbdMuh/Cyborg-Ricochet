using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    private Trajectory _trajectory;

    private void Start()
    {
        _trajectory = GetComponentInChildren<Trajectory>();
    }

    private void Update()
    {
        if (_trajectory.characterAim && Input.GetMouseButtonUp(0))
        {
            AttackAndAnimate();
        }
    }

    private void AttackAndAnimate()
    {
        if(Input.GetMouseButtonUp(0))
        {
            Debug.Log("Play attack animation");   
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            
            Destroy(other.gameObject);
            Debug.Log("Add Bullet explosion effect here");
        }
    }
}
