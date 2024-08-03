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
        if (_trajectory.CharacterAim && Input.GetMouseButtonUp(0))
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

    // private void OnCollisionExit(Collision other)
    // {
    //     if (other.gameObject.CompareTag("enemy"))
    //     {
    //         Debug.Log("Play Enemy Death Animation");
    //         Destroy(other.gameObject,0.5f);
    //     }
    // }
}
