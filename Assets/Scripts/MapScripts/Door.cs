using UnityEngine;

public class Door : MonoBehaviour {
    public Direction direction;
    public Room currentRoom;

    private void Awake() {
        if (currentRoom == null) currentRoom = GetComponentInParent<Room>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag("Player")) return;

        Vector2Int newPos = currentRoom.roomPos;

        switch (direction) {
            case Direction.Up: newPos += Vector2Int.up; break;
            case Direction.Down: newPos += Vector2Int.down; break;
            case Direction.Left: newPos += Vector2Int.left; break;
            case Direction.Right: newPos += Vector2Int.right; break;
        }

        GameObject newRoom = MapGenerator.Instance.SpawnRoom(newPos, direction);

        MovePlayer(collision.gameObject, newRoom.GetComponent<Room>(), direction);
    }

    void MovePlayer(GameObject player, Room newRoom, Direction dir) {
        Transform spawnPoint = null;

        switch (Room.Opposite(dir)) {
            case Direction.Up: spawnPoint = newRoom.doorUp?.transform; break;
            case Direction.Down: spawnPoint = newRoom.doorDown?.transform; break;
            case Direction.Left: spawnPoint = newRoom.doorLeft?.transform; break;
            case Direction.Right: spawnPoint = newRoom.doorRight?.transform; break;
        }

        if (spawnPoint != null) player.transform.position = spawnPoint.position;
        else player.transform.position = newRoom.transform.position;
    }
}