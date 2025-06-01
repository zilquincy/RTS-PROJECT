using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Transform TargetToAttack;
    public Material idleStateMaterial;
    public Material followStateMaterial;
    public Material attackStateMaterial;
    internal object unitDamage;

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
    public void SetIdleMaterial()
    {
        GetComponent<Renderer>().material = idleStateMaterial;
    }
    public void SetFollowMaterial()
    {
        GetComponent<Renderer>().material = followStateMaterial;
    }

    public void SetAttackMaterial()
    {
        GetComponent<Renderer>().material = attackStateMaterial;
    }
    private void OnDrawGizmos()
    {
        // follow distance Area 
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 10*0.2f);

        // attack distance area
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);
        // stop attack distance

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 1.2f);
    }

}
