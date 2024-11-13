using System.Collections.Generic;
using Unity.Burst.CompilerServices;

public static class ProceduralDungeonGenerator
{
    static LinkedList<Room> rooms;
    static LinkedList<Door> doors;
    static System.Random rand;
    const int RandomGenSeed = 50;

    public static void Init()
    {
        rand = new System.Random(RandomGenSeed);
        rooms = new LinkedList<Room>();
        doors = new LinkedList<Door>();
        ProcedurallyGenerateDungeon();
    }

    private static Room AddRoom(RoomType roomType, Coordinate coordinate)
    {
        Room room = new Room(roomType, coordinate);
        rooms.AddLast(room);
        return room;
    }

    private static Door AddDoor(DoorType doorType, Direction direction, Room startingRoom) 
    {
        Door door = new Door(doorType, direction, startingRoom);
        doors.AddLast(door);
        return door;
    }

    private static bool Roll(float percentageChance)
    {
        return rand.NextDouble() < percentageChance / 100f;
    }

    public static void ProcedurallyGenerateDungeon()
    {

        //TASK LIST:
        //Starting room is always at the center
        // Starting room must have 2 or 3 doors
        //Clean up code

        // We want 20 rooms
        // All rooms must be connected

        // Secret doors must not interfere with main path, they must be off to the side
        // One Shop room
        // Shop room must be locked & only have one door accessing it
        // One Boss room
        // Boss room must not be next to the start, must be a certain distance
        // Boss room must only be connected to one other room


        //int roomCounter

        // Room CreateStartingRoom() => AddRoom(RoomType.Start, new Coordinate(0, 0));
        Room startingRoom = AddRoom(RoomType.Start, new Coordinate(0, 0));

        #region Create Rooms Connected to Starting Room

        AddDoor(DoorType.Open, Direction.Right, startingRoom);
        AddRoom(RoomType.Normal, new Coordinate(1, 0));

        AddDoor(DoorType.Open, Direction.Left, startingRoom);
        AddRoom(RoomType.Normal, new Coordinate(-1, 0));

        if (Roll(50))
        {
            AddDoor(DoorType.Open, Direction.Down, startingRoom);
            AddRoom(RoomType.Normal, new Coordinate(0, -1));
        }

        #endregion

    }


    public static void DestroyDungeon()
    {
        rooms.Clear();
        doors.Clear();
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
