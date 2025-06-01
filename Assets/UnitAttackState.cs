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

            var damageToInflict = attackController.unitDamage;

            attackController.TargetToAttack.GetComponent<Enemy>().ReceiveDamage(damageToInflict);


            float distanceFromTarget = Vector3.Distance(attackController.TargetToAttack.position, animator.transform.position);
            if (distanceFromTarget > stopAttackingDistance || attackController.TargetToAttack == null)
            {
                animator.SetBool("isAttacking", false); 
            }
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
