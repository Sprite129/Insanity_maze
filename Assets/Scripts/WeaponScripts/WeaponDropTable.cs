using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDropTable", menuName = "Weapons/Drop Table")]
public class WeaponDropTable : ScriptableObject
{
    [System.Serializable]
    public struct WeaponDropEntry
    {
        public GameObject pickupPrefab;
        [Range(0f, 100f)] public float dropChance;
    }

    [SerializeField] private WeaponDropEntry[] weapons;
    [Range(0f, 100f)]
    [SerializeField] private float overallDropChance = 40f;

    public GameObject GetRandomDrop()
    {
        if (Random.Range(0f, 100f) > overallDropChance)
            return null;

        float totalWeight = 0f;
        foreach (var entry in weapons)
            totalWeight += entry.dropChance;

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var entry in weapons)
        {
            cumulative += entry.dropChance;
            if (roll <= cumulative)
                return entry.pickupPrefab;
        }

        return null;
    }
}
