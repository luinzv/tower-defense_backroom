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

        GameObject towerToBuild = BuildManager.main.GetSelectedTower();

        if (towerToBuild == null)
        {
            return;
        }

        Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : transform.position;
        tower = Instantiate(towerToBuild, spawnPosition, Quaternion.identity);
    }
}
