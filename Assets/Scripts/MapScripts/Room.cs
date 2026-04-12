using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
    public Vector2Int roomPos;

    public GameObject doorUp;
    public GameObject doorDown;
    public GameObject doorLeft;
    public GameObject doorRight;

    public GameObject wallUp;
    public GameObject wallDown;
    public GameObject wallLeft;
    public GameObject wallRight;

    private List<Direction> activeDoors = new List<Direction>();

    public void Init(Vector2Int pos, bool isMain, Direction fromDir = Direction.Up) {
        roomPos = pos;

        ResetDoors();

        if (isMain) ActivateAllDoors();
        else GenerateDoors(fromDir);
    }

    void ResetDoors() {
        activeDoors.Clear();

        doorUp.SetActive(false);
        doorDown.SetActive(false);
        doorLeft.SetActive(false);
        doorRight.SetActive(false);

        wallUp.SetActive(true);
        wallDown.SetActive(true);
        wallLeft.SetActive(true);
        wallRight.SetActive(true);
    }

    void ActivateAllDoors() {
        ActivateDoor(Direction.Up);
        ActivateDoor(Direction.Down);
        ActivateDoor(Direction.Left);
        ActivateDoor(Direction.Right);
    }

    void GenerateDoors(Direction fromDir) {
        ActivateDoor(Opposite(fromDir));

        int extraDoors = Random.Range(0, 3);

        List<Direction> possible = new List<Direction> {
            Direction.Up, Direction.Down, Direction.Left, Direction.Right
        };

        possible.Remove(Opposite(fromDir));

        for (int i = 0; i < extraDoors; i++) {
            if (possible.Count == 0) break;

            Direction dir = possible[Random.Range(0, possible.Count)];
            ActivateDoor(dir);
            possible.Remove(dir);
        }
    }

    void ActivateDoor(Direction dir) {
        if (activeDoors.Contains(dir)) return;

        switch (dir) {
            case Direction.Up:
                doorUp.SetActive(true);
                wallUp.SetActive(false);
                break;

            case Direction.Down:
                doorDown.SetActive(true);
                wallDown.SetActive(false);
                break;

            case Direction.Left:
                doorLeft.SetActive(true);
                wallLeft.SetActive(false);
                break;

            case Direction.Right:
                doorRight.SetActive(true);
                wallRight.SetActive(false);
                break;
        }

        activeDoors.Add(dir);
    }

    public static Direction Opposite(Direction dir) {
        switch (dir) {
            case Direction.Up: return Direction.Down;
            case Direction.Down: return Direction.Up;
            case Direction.Left: return Direction.Right;
            case Direction.Right: return Direction.Left;
        }
        
        return Direction.Up;
    }
}