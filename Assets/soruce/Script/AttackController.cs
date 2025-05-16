using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Transform TargetToAttack;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && TargetToAttack == null)
        {
            TargetToAttack = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && TargetToAttack != null)
        {
            TargetToAttack = null;
        }
    }
}
