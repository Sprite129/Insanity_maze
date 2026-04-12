using UnityEngine;

public class EnemyDropHandler : MonoBehaviour
{
    [SerializeField] private WeaponDropTable dropTable;

    public void TryDrop()
    {
        if (dropTable == null)
            return;

        GameObject drop = dropTable.GetRandomDrop();
        if (drop != null)
            Instantiate(drop, transform.position, Quaternion.identity);
    }
}
