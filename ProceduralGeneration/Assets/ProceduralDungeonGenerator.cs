using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralDungeonGenerator : MonoBehaviour
{
    LinkedList<GameObject> roomVisuals, doorVisuals;
    LinkedList<Room> rooms;
    LinkedList<Door> doors;
    System.Random rand;

    void Start()
    {
        rand = new System.Random(50);
        rooms = new LinkedList<Room>();
        doors = new LinkedList<Door>();
        roomVisuals = new LinkedList<GameObject>();
        doorVisuals = new LinkedList<GameObject>();
        ProceduralGenerateDungeon();
        CreateVisualsFromModelData();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            DestroyDungeon();
            ProceduralGenerateDungeon();
            CreateVisualsFromModelData();
        }
    }

    private void ProceduralGenerateDungeon()
    {
        //int roomCounter

        #region Make Starting Room and Connected Rooms

        Room startingRoom = new Room(RoomType.Start, new Coordinate(0, 0));
        rooms.AddLast(startingRoom);

        if (Roll(50))
        {
            Door d = new Door(DoorType.Open, Direction.Right, startingRoom);
            doors.AddLast(d);

            Room r = new Room(RoomType.Normal, new Coordinate(1, 0));
            rooms.AddLast(r);

            d = new Door(DoorType.Open, Direction.Down, startingRoom);
            doors.AddLast(d);
            r = new Room(RoomType.Normal, new Coordinate(0, -1));
            rooms.AddLast(r);

            d = new Door(DoorType.Open, Direction.Left, startingRoom);
            doors.AddLast(d);
            r = new Room(RoomType.Normal, new Coordinate(-1, 0));
            rooms.AddLast(r);
        }
        else
        {
            Door d = new Door(DoorType.Open, Direction.Right, startingRoom);
            doors.AddLast(d);

            Room r = new Room(RoomType.Normal, new Coordinate(1, 0));
            rooms.AddLast(r);

            d = new Door(DoorType.Open, Direction.Left, startingRoom);
            doors.AddLast(d);
            r = new Room(RoomType.Normal, new Coordinate(-1, 0));
            rooms.AddLast(r);
        }

        #endregion

    }

    private void DestroyDungeon()
    {
        foreach (GameObject r in roomVisuals)
            Destroy(r);
        foreach (GameObject d in doorVisuals)
            Destroy(d);

        roomVisuals.Clear();
        doorVisuals.Clear();

        rooms.Clear();
        doors.Clear();
    }

    private GameObject MakeRoomVisual(Room room)
    {
        GameObject roomVisual = Instantiate(Resources.Load<GameObject>("Room"));
        room.visual = roomVisual;
        Coordinate coord = room.coordinate;
        roomVisual.name = "Room " + coord.x + "," + coord.y;
        roomVisual.transform.position = new Vector3(coord.x, coord.y, 0);

        #region Determine Room Color Based on Type

        Color roomColor = Color.magenta;

        switch (room.type)
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

        roomVisuals.AddLast(roomVisual);
        return roomVisual;
    }

    private void MakeDoorVisual(Door door)
    {
        GameObject doorVisual = Instantiate(Resources.Load<GameObject>("Door"));
        door.visual = doorVisual;
        Coordinate coord = door.room.coordinate;
        doorVisual.name = "Door " + coord.x + "," + coord.y + ", " + door.sideOfRoom;

        #region Determine Postition OffSet Based On SideOfRoom

        Vector2 offSet = Vector2.zero;
        switch (door.sideOfRoom)
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

        #endregion

        doorVisual.transform.position = (new Vector3(coord.x, coord.y, 0) + new Vector3(offSet.x, offSet.y, 0));

        #region Determine Door Color Based on Type

        Color doorColor = Color.magenta / 2f + Color.black / 2f;

        switch (door.type)
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

        doorVisuals.AddLast(doorVisual);
    }

    private void CreateVisualsFromModelData()
    {
        foreach (Room r in rooms)
        {
            MakeRoomVisual(r);
        }
        foreach (Door d in doors)
        {
            MakeDoorVisual(d);
        }
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
        if (x == otherCoord.x && y == otherCoord.y)
            return true;
        return false;
    }
}

class Door
{
    public GameObject visual;
    public DoorType type;
    public Direction sideOfRoom;
    public Room room;

    public Door(DoorType type, Direction sideOfRoom, Room room)
    {
        this.type = type;
        this.sideOfRoom = sideOfRoom;
        this.room = room;
    }

}

class Room
{
    public GameObject visual;
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
