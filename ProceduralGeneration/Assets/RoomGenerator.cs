using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class RoomGenerator : MonoBehaviour
{

    void Start()
    {
        MakeRoom(0, 0);
        MakeDoor(0, 0, Direction.Right);

        MakeRoom(1, 0);
        MakeDoor(1, 0, Direction.Up);

        MakeRoom(1, 1);
        MakeDoor(1, 1, Direction.Up);

        MakeRoom(1, 2);
        MakeDoor(1, 2, Direction.Up);

        MakeRoom(1, 3);

    }

    void Update()
    {

    }

    public void MakeRoom(int x, int y)
    {
        GameObject room = Instantiate(Resources.Load<GameObject>("Room"));
        room.transform.position = new Vector3(x, y, 0);
    }

    public void MakeDoor(int x, int y, Direction sideOfRoom)
    {
        GameObject door = Instantiate(Resources.Load<GameObject>("Door"));

        Vector2 offSet = Vector2.zero;
        switch (sideOfRoom)
        {
            case Direction.Up:
                offSet = new Vector2(0, 0.5f);
            break;
            case Direction.Down:
                offSet = new Vector2(0, -0.5f);
            break;
            case Direction.Left:
                offSet = new Vector2(-0.5f, 0);
            break;
            case Direction.Right:
                offSet = new Vector2(0.5f, 0);
            break;
        }

        door.transform.position = (new Vector3(x, y, 0) + new Vector3(offSet.x, offSet.y, 0));
    }
}

public enum Direction
{
    Up, Down, Left, Right
}