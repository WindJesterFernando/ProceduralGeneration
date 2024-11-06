using System;
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

        #region Make Starting Room and Connected Rooms

        MakeRoomVisual(0, 0, RoomType.Start);

        if (Roll(50))
        {
            MakeDoorVisual(0, 0, Direction.Right, DoorType.Open);
            MakeRoomVisual(1, 0, RoomType.Normal);

            MakeDoorVisual(0, 0, Direction.Down, DoorType.Open);
            MakeRoomVisual(0, -1, RoomType.Normal);

            MakeDoorVisual(0, 0, Direction.Left, DoorType.Open);
            MakeRoomVisual(-1, 0, RoomType.Normal);
        }
        else
        {
            MakeDoorVisual(0, 0, Direction.Right, DoorType.Open);
            MakeRoomVisual(1, 0, RoomType.Normal);

            MakeDoorVisual(0, 0, Direction.Left, DoorType.Open);
            MakeRoomVisual(-1, 0, RoomType.Normal);
        }

        #endregion

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

    private void MakeRoomVisual(int x, int y, RoomType roomType)
    {
        GameObject roomVisual = Instantiate(Resources.Load<GameObject>("Room"));
        roomVisual.name = "Room " + x + "," + y;
        roomVisual.transform.position = new Vector3(x, y, 0);

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

        roomVisual.GetComponent<SpriteRenderer>().color = roomColor;

        #endregion

        rooms.AddLast(roomVisual);
    }

    private void MakeDoorVisual(int x, int y, Direction sideOfRoom, DoorType doorType)
    {
        GameObject doorVisual = Instantiate(Resources.Load<GameObject>("Door"));
        doorVisual.name = "Door " + x + "," + y + ", " + sideOfRoom;

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

        doorVisual.transform.position = (new Vector3(x, y, 0) + new Vector3(offSet.x, offSet.y, 0));

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

        doorVisual.GetComponent<SpriteRenderer>().color = doorColor;

        #endregion

        doors.AddLast(doorVisual);
    }

    private bool Roll(float percentageChance)
    {
        return rand.NextDouble() < percentageChance / 100f;
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

class Coordinate
{
    public int x, y;

    public Coordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public bool IsEqualTo(Coordinate otherCoord)
    {
        if(x == otherCoord.x && y == otherCoord.y)
            return true;
        return false;
    }
}

class Door
{
    public GameObject gameObject;
    public DoorType type;
    public Direction direction;
    public Room room;

    public Door(DoorType type, Direction direction, Room room)
    {
        this.type = type;
        this.direction = direction;
        this.room = room;
    }

}

class Room
{
    public GameObject gameObject;
    public RoomType type;
    public Coordinate coordinate;
    public LinkedList<Door> doors;
    public LinkedList<Room> neighbourRooms;
    
    public Room(RoomType type, Coordinate coordinate)
    {
        this.type = type;
        this.coordinate = coordinate;
        doors = new LinkedList<Door>();
        neighbourRooms = new LinkedList<Room>();
    }
}
