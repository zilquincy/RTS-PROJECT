using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitAttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    AttackController attackController;
    public float stopAttackingDistance = 1.2f;

    public float attackRate = 1f;

    public float attackTimer;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        attackController = animator.GetComponent<AttackController>();
        attackController.SetAttackMaterial();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (attackController.TargetToAttack != null && animator.transform.GetComponent<UnitMovement>().isCommandedToMove == false)
        {
            LookAtTarget();
            agent.SetDestination(attackController.TargetToAttack.position);

            if (attackTimer <= 0)
            {
                Attack();
                attackTimer = 1f / attackRate;
            }
            else
            {
                attackTimer -= Time.deltaTime;
            }


            float distanceFromTarget = Vector3.Distance(attackController.TargetToAttack.position, animator.transform.position);
            if (distanceFromTarget > stopAttackingDistance || attackController.TargetToAttack == null)
            {
                animator.SetBool("isAttacking", false); 
            }
        }
    }
    private void Attack()
    {
        Debug.Log("Attacking: " + attackController.TargetToAttack.name);
        var damageToInflict = attackController.unitDamage;

        var unit = attackController.TargetToAttack.GetComponent<Unit>();
        if (unit != null)
        {
            unit.TakeDamage(damageToInflict);
        }
        else
        {
            Debug.LogWarning("Target does not have Unit script!");
        }
    }

    private void LookAtTarget()
    {
        Vector3 direction = attackController.TargetToAttack.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
    }
}
