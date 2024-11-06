using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralDungeonGenerator : MonoBehaviour
{
    LinkedList<GameObject> rooms, doors;
    System.Random rand;

    void Start()
    {
        rand = new System.Random(50);
        rooms = new LinkedList<GameObject>();
        doors = new LinkedList<GameObject>();
        ProceduralGenerateDungeon();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            DestroyDungeon();
            ProceduralGenerateDungeon();
        }
    }

    private void ProceduralGenerateDungeon()
    {
        //int roomCounter

        MakeRoom(0, 0, RoomType.Start);

        if (rand.NextDouble() > 0.5f)
        {
            MakeDoor(0, 0, Direction.Right, DoorType.Open);
            MakeRoom(1, 0, RoomType.Normal);

            MakeDoor(0, 0, Direction.Down, DoorType.Open);
            MakeRoom(0, -1, RoomType.Normal);

            MakeDoor(0, 0, Direction.Left, DoorType.Open);
            MakeRoom(-1, 0, RoomType.Normal);
        }
        else
        {
            MakeDoor(0, 0, Direction.Right, DoorType.Open);
            MakeRoom(1, 0, RoomType.Normal);

            MakeDoor(0, 0, Direction.Left, DoorType.Open);
            MakeRoom(-1, 0, RoomType.Normal);
        }
        
    }

    private void DestroyDungeon()
    {
        foreach (GameObject r in rooms)
            Destroy(r);
        foreach (GameObject d in doors)
            Destroy(d);

        rooms.Clear();
        doors.Clear();
    }

    private void MakeRoom(int x, int y, RoomType roomType)
    {
        GameObject room = Instantiate(Resources.Load<GameObject>("Room"));
        room.name = "Room " + x + "," + y;
        room.transform.position = new Vector3(x, y, 0);

        #region Determine Room Color Based on Type

        Color roomColor = Color.magenta;

        switch (roomType)
        {
            case RoomType.Normal:
                roomColor = Color.white;
                break;
            case RoomType.Start:
                roomColor = Color.green / 2f + Color.white / 2f;
                break;
            case RoomType.Secret:
                roomColor = Color.blue / 2f + Color.white / 2f;
                break;
            case RoomType.Shop:
                roomColor = Color.yellow / 2f + Color.white / 2f;
                break;
            case RoomType.Boss:
                roomColor = Color.red / 2f + Color.white / 2f;
                break;
            case RoomType.None:
                roomColor = Color.magenta;
                break;
        }

        room.GetComponent<SpriteRenderer>().color = roomColor;

        #endregion

        rooms.AddLast(room);
    }

    private void MakeDoor(int x, int y, Direction sideOfRoom, DoorType doorType)
    {
        GameObject door = Instantiate(Resources.Load<GameObject>("Door"));
        door.name = "Door " + x + "," + y + ", " + sideOfRoom;

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

        #region Determine Door Color Based on Type

        Color doorColor = Color.magenta / 2f + Color.black / 2f;

        switch (doorType)
        {
            case DoorType.Open:
                doorColor = Color.white / 2f + Color.black / 2f;
                break;
            case DoorType.Locked:
                doorColor = Color.green / 2f + Color.black / 2f;
                break;
            case DoorType.Bombable:
                doorColor = Color.blue / 2f + Color.black / 2f;
                break;
            case DoorType.ShutUntilRoomIsCleared:
                doorColor = Color.red / 2f + Color.black / 2f;
                break;
            case DoorType.None:
                doorColor = Color.magenta / 2f + Color.black / 2f;
                break;

        }

        door.GetComponent<SpriteRenderer>().color = doorColor;

        #endregion

        doors.AddLast(door);
    }

}

public enum Direction
{
    Up, Down, Left, Right
}

public enum RoomType
{
    None,
    Start,
    Normal,
    Boss,
    Secret,
    Shop
}

public enum DoorType
{
    None,
    Open,
    Locked,
    ShutUntilRoomIsCleared,
    Bombable,
}

