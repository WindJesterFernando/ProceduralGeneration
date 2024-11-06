using System;
using System.Collections;
using System.Collections.Generic;

public static class ProceduralDungeonGenerator
{
    static LinkedList<Room> rooms;
    static LinkedList<Door> doors;
    static System.Random rand;

    public static void Init()
    {
        rand = new System.Random(50);
        rooms = new LinkedList<Room>();
        doors = new LinkedList<Door>();
        ProcedurallyGenerateDungeon();
    }
    
    public static void ProcedurallyGenerateDungeon()
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

    public static void DestroyDungeon()
    {
        rooms.Clear();
        doors.Clear();
    }

    private static bool Roll(float percentageChance)
    {
        return rand.NextDouble() < percentageChance / 100f;
    }

    public static LinkedList<Room> GetRooms()
    {
        return rooms;
    }

    public static LinkedList<Door> GetDoors()
    {
        return doors;
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

public class Coordinate
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

public class Door
{
    public UnityEngine.GameObject visual;
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

public class Room
{
    public UnityEngine.GameObject visual;
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
