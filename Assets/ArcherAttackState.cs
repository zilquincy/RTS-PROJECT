using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArcherAttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    ArcherAttackController attackController;
    public float stopAttackingDistance = 3.2f;
    public float attackingDistance = 3f; // Jarak ideal untuk menyerang
    public float attackRate = 1f;
    public float attackTimer;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        attackController = animator.GetComponent<ArcherAttackController>();
        attackController.SetAttackMaterial();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (attackController.TargetToAttack != null && animator.transform.GetComponent<UnitMovement>().isCommandedToMove == false)
        {
            LookAtTarget();

            float distanceFromTarget = Vector3.Distance(attackController.TargetToAttack.position, animator.transform.position);

            // PERBAIKAN: Hanya bergerak jika terlalu jauh dari jangkauan serang optimal
            if (distanceFromTarget > attackingDistance)
            {
                // Terlalu jauh, mendekati target
                agent.SetDestination(attackController.TargetToAttack.position);
            }
            else
            {
                // Sudah dalam jangkauan serang, BERHENTI di tempat
                agent.SetDestination(animator.transform.position);

                // Lakukan serangan
                if (attackTimer <= 0)
                {
                    Attack();
                    attackTimer = 1f / attackRate;
                }
                else
                {
                    attackTimer -= Time.deltaTime;
                }
            }

            // Keluar dari state attack jika target terlalu jauh atau hilang
            if (distanceFromTarget > stopAttackingDistance || attackController.TargetToAttack == null)
            {
                animator.SetBool("isAttacking", false);
            }
        }
        else
        {
            // Tidak ada target atau sedang diperintah bergerak
            animator.SetBool("isAttacking", false);
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