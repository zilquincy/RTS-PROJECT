using UnityEngine;

public class SimplePatrol : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5.0f;
    private bool movingForward = true;
    private float timer = 0.0f;
    private float switchDirectionTime = 5.0f;

    [Header("Attack Settings")]
    public bool canAttack = true;
    public float attackRange = 3f;
    public float attackRate = 1f;
    public int attackDamage = 10;
    public string targetTag = "Player";

    private Transform currentTarget;
    private float attackTimer;
    private bool isAttacking = false;

    void Update()
    {
        // Cari target dalam jangkauan
        if (canAttack)
        {
            FindTarget();
        }

        if (isAttacking && currentTarget != null)
        {
            // Mode attack - berhenti patrol dan serang
            AttackMode();
        }
        else
        {
            // Mode patrol normal
            PatrolMode();
        }
    }

    void PatrolMode()
    {
        timer += Time.deltaTime;
        if (timer >= switchDirectionTime)
        {
            movingForward = !movingForward;
            timer = 0.0f;
        }

        if (movingForward)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
    }

    void FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);

        currentTarget = null;
        foreach (Collider col in colliders)
        {
            if (col.CompareTag(targetTag))
            {
                currentTarget = col.transform;
                isAttacking = true;
                return;
            }
        }

        // Jika tidak ada target, keluar dari mode attack
        if (currentTarget == null)
        {
            isAttacking = false;
        }
    }

    void AttackMode()
    {
        if (currentTarget == null)
        {
            isAttacking = false;
            return;
        }

        // Cek jarak, jika terlalu jauh keluar dari attack mode
        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
        if (distanceToTarget > attackRange * 1.2f)
        {
            isAttacking = false;
            currentTarget = null;
            return;
        }

        // Look at target
        Vector3 direction = currentTarget.position - transform.position;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        // Attack timer
        if (attackTimer <= 0)
        {
            PerformAttack();
            attackTimer = 1f / attackRate;
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }

    void PerformAttack()
    {
        if (currentTarget == null) return;

        Debug.Log($"Dummy attacking: {currentTarget.name}");

        // Coba ambil komponen Unit untuk memberikan damage
        Unit targetUnit = currentTarget.GetComponent<Unit>();
        if (targetUnit != null)
        {
            targetUnit.TakeDamage(attackDamage);
        }
        else
        {
            Debug.LogWarning($"Target {currentTarget.name} tidak memiliki komponen Unit!");
        }
    }

    // Visualisasi di Scene view
    void OnDrawGizmosSelected()
    {
        // Attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Patrol path
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position - transform.forward * 5f, transform.position + transform.forward * 5f);
    }
}

