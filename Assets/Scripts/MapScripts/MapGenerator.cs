using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    public static MapGenerator Instance;

    public GameObject mainRoom;
    public List<GameObject> roomPrefabs;

    private Dictionary<Vector2Int, GameObject> spawnedRooms = new Dictionary<Vector2Int, GameObject>();

    public float roomOffset = 20f;

    void Awake() {
        Instance = this;
    }

    void Start() {
        SpawnMainRoom();
    }

    void SpawnMainRoom() {
        Vector2Int pos = Vector2Int.zero;

        GameObject room = Instantiate(mainRoom, Vector3.zero, Quaternion.identity);
        room.GetComponent<Room>().Init(pos, true);

        spawnedRooms.Add(pos, room);
    }

    public GameObject SpawnRoom(Vector2Int pos, Direction fromDirection) {
        if (spawnedRooms.ContainsKey(pos)) return spawnedRooms[pos];

        GameObject prefab = roomPrefabs[Random.Range(0, roomPrefabs.Count)];

        Vector3 worldPos = new Vector3(pos.x * roomOffset, pos.y * roomOffset, 0);
        GameObject room = Instantiate(prefab, worldPos, Quaternion.identity);

        room.GetComponent<Room>().Init(pos, false, fromDirection);

        spawnedRooms.Add(pos, room);

        return room;
    }

    public bool RoomExists(Vector2Int pos) {
        return spawnedRooms.ContainsKey(pos);
    }
}

public enum Direction {
    Up,
    Down,
    Left,
    Right
}