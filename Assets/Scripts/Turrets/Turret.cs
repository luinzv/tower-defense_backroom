using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 25f;
    [SerializeField] private float bps = 1f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private int baseUpgradeCost = 100;
    private float bpsBase;
    private float targetingRangeBase;
    private Transform target;
    private float timeUntilFire;
    private int level = 1;

    private void Start()
    {
        bpsBase = bps;
        targetingRangeBase = targetingRange;

        upgradeButton.onClick.AddListener(Upgrade);
    }

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

    private void OnMouseDown()
    {
        OpenUpgradeUI();
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
        if (target == null) return;

        Vector2 direction = target.position - turretRotationPoint.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.RotateTowards(
            turretRotationPoint.rotation,
            Quaternion.Euler(0f, 0f, angle),
            rotationSpeed * Time.deltaTime
        );

        turretRotationPoint.rotation = targetRotation;
    }

    // --- UI DE MEJORA ---
    public void OpenUpgradeUI()
    {
        if (upgradeUI != null)
            upgradeUI.SetActive(true);
    }

    public void CloseUpgradeUI()
    {
        if (upgradeUI != null)
            upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);
    }

    public void Upgrade()
    {
        int cost = CalculateCost();

        if (cost > LevelManager.main.currency)
        {
            Debug.Log("❌ No tienes suficiente dinero para mejorar la torreta.");
            return;
        }

        LevelManager.main.SpendCurrency(cost);
        level++;

        bps = CalculateBPS();
        targetingRange = CalculateRange();

        Debug.Log($"✅ Torreta mejorada al nivel {level}\n" +
                  $"💰 Costo: {cost}\n" +
                  $"⚡ BPS: {bps:F2}\n" +
                  $"🎯 Rango: {targetingRange:F2}");

        CloseUpgradeUI();
    }

    private int CalculateCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    }

    private float CalculateBPS()
    {
        return bpsBase * Mathf.Pow(level, 0.6f);
    }

    private float CalculateRange()
    {
        return targetingRangeBase * Mathf.Pow(level, 0.4f);
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
