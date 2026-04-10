using UnityEngine;
using System.Collections.Generic;

public class RoomMonsterSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private List<GameObject> monsterPrefabs;
    [SerializeField] private int minSpawn = 1;
    [SerializeField] private int maxSpawn = 3;
    [SerializeField] private bool spawnOnStart = true;
    [SerializeField] private float spawnRadius = 3f;
    [SerializeField] private Collider2D spawnArea;
    [SerializeField] private float spawnPadding = 0.5f;

    private void Start()
    {
        if (spawnOnStart)
            SpawnMonsters();
    }

    public void SpawnMonsters()
    {
        if (monsterPrefabs == null || monsterPrefabs.Count == 0)
        {
            Debug.LogWarning("RoomMonsterSpawner: no monster prefabs assigned.");
            return;
        }

        int count = Random.Range(minSpawn, maxSpawn + 1);
        for (int i = 0; i < count; i++)
        {
            GameObject prefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Count)];
            Vector2 spawnPosition = GetSpawnPosition();
            Instantiate(prefab, spawnPosition, Quaternion.identity);
        }
    }

    private Vector2 GetSpawnPosition()
    {
        if (spawnArea != null)
        {
            Bounds bounds = spawnArea.bounds;
            return new Vector2(
                Random.Range(bounds.min.x + spawnPadding, bounds.max.x - spawnPadding),
                Random.Range(bounds.min.y + spawnPadding, bounds.max.y - spawnPadding)
            );
        }

        return (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
    }
}
