using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 25f;
    [SerializeField] private float bps = 1f; // Bullets Per Second

    private Transform target;
    private float timeUntilFire;

    private void Update()
    {
        if (target == null || !IsTargetInRange())
        {
            FindTarget();
        }

        if (target != null)
        {
            RotateTowardsTarget();
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / bps)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    private bool IsTargetInRange()
    {
        return Vector2.Distance(target.position, turretRotationPoint.position) <= targetingRange;
    }

    private void FindTarget()
    {
        Vector3 origin = turretRotationPoint != null ? turretRotationPoint.position : transform.position;
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, targetingRange, enemyMask);

        if (hits.Length == 0)
        {
            target = null;
            return;
        }

        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (Collider2D hit in hits)
        {
            float distance = Vector2.Distance(origin, hit.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = hit.transform;
            }
        }

        target = nearestEnemy;
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();

        if (bulletScript != null)
            bulletScript.SetTarget(target);
    }

    private void RotateTowardsTarget()
    {
        Vector2 direction = target.position - turretRotationPoint.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
        turretRotationPoint.rotation = targetRotation;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 drawPosition = turretRotationPoint != null ? turretRotationPoint.position : transform.position;

        Handles.color = Color.cyan;
        Handles.DrawWireDisc(drawPosition, Vector3.forward, targetingRange);

        if (target != null)
        {
            Handles.color = Color.red;
            Handles.DrawLine(drawPosition, target.position);
        }
    }
#endif
}
