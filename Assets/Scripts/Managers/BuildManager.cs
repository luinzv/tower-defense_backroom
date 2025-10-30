using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private GameObject[] towerPrefabs;

    private int selectedTower = 0;

    private void Awake()
    {
        main = this;
    }

    public GameObject GetSelectedTower()
    {
        if (towerPrefabs == null || towerPrefabs.Length == 0)
        {
            Debug.LogError("❌ No hay prefabs de torres asignados en el BuildManager.");
            return null;
        }

        if (selectedTower < 0 || selectedTower >= towerPrefabs.Length)
        {
            Debug.LogError($"⚠ Índice de torre fuera de rango: {selectedTower}");
            return null;
        }

        return towerPrefabs[selectedTower];
    }
}
