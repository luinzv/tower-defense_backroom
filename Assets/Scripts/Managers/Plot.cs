using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Transform spawnPoint;
    private GameObject tower;
    private Color startColor;

    private void Start()
    {
        startColor = sr.color;

        if (spawnPoint == null)
            spawnPoint = transform.Find("SpawnPoint");
    }

    private void OnMouseEnter()
    {
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    private void OnMouseDown()
{
    if (tower != null) return;

    Tower towerToBuild = BuildManager.main.GetSelectedTower();

    if (towerToBuild == null)
        return;

    if (LevelManager.main.currency < towerToBuild.cost)
    {
        Debug.Log("No tienes suficiente dinero para construirlo ");
        return;
    }

    LevelManager.main.SpendCurrency(towerToBuild.cost);

    Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : transform.position;
    tower = Instantiate(towerToBuild.prefab, spawnPosition, Quaternion.identity);

    Debug.Log($"Construida torre Dinero {LevelManager.main.currency}");
}


    private void AlignTurretParts(GameObject towerInstance)
    {
        Transform basePart = towerInstance.transform.Find("Base");
        Transform rotatePart = towerInstance.transform.Find("RotatePoint");

        if (basePart == null || rotatePart == null)
        {
            return;
        }

        rotatePart.position = basePart.position;
        rotatePart.rotation = basePart.rotation;
    }
}
