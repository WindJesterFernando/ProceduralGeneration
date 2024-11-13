using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ProceduralDungeonGenerator
{
    static LinkedList<Room> rooms;
    static LinkedList<Door> doors;
    static System.Random rand;
    static int roomCounter = 10;
    
    const int RandomGenSeed = 50;

    public static void Init()
    {
        rand = new System.Random(RandomGenSeed);
        rooms = new LinkedList<Room>();
        doors = new LinkedList<Door>();
        ProcedurallyGenerateDungeon(roomCounter);
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

    private static Direction RandomDirection() => (Direction) Random.Range(0, 4);
    

    private static List<Room> GenerateNeighbours(int doorCount,Room parent)
    {
        List<Room> neighbours = new List<Room>();   
        // temp
        
        foreach(Door door in parent.doors)
        {
            Coordinate newRoomCoordinate = CalculateNewRoomCoordinate(parent.coordinate, door.sideOfRoom);
            var newRoom = AddRoom(RoomType.Normal, newRoomCoordinate);
            roomCounter++;

            // only 2 options

            for (int i = 0; i < doorCount; i++)
            {
                #region Ensure No Repeat Doors
                Direction newRoomDirection = RandomDirection();
                var directions = newRoom.doors.Select(door => door.sideOfRoom);
                while (directions.Contains(newRoomDirection)) 
                {
                    newRoomDirection = RandomDirection();
                }
                #endregion
                
                newRoom.doors.AddLast(AddDoor(DoorType.Open, newRoomDirection, newRoom));
                neighbours.Add(newRoom);
            }
        }
        return neighbours;
    }


    public static void ProcedurallyGenerateDungeon(int roomCount, Room previousRoom = null)
    {
        Room room = previousRoom;
        if (roomCount == 0)
        {
            //BossRoom
            return;
        }

        if (room == null)
        {
            //StartingRoom
            room = AddRoom(RoomType.Start, new Coordinate(0, 0));
            room.doors.AddLast(AddDoor(DoorType.Open, Direction.Right, room));
            room.doors.AddLast(AddDoor(DoorType.Open, Direction.Left, room));
        }


        #region Genertion of Door Counts
        int doorCount = Roll(50) ? 2 : 3;

        #endregion

        var childRooms = GenerateNeighbours(doorCount, room);
        foreach(Room childRoom in childRooms)
        {
            ProcedurallyGenerateDungeon(roomCount - 1, childRoom);
        }
        //TASK LIST:
        //Starting room is always at the center
        // Starting room must have 2 or 3 doors
        //Clean up code




        // We want 20 rooms
        // All rooms must be connected
        // we want branching paths.....


        // “We take a room, choose a direction, we make a room/door in that direction”
        // “Repeat 20 times”
        // “Roll to see how many directions we have for a room”
        // “Make sure there’s no room there”, do not create repeat rooms
        // “Deciding which room is the current room to go with”
        // “Tree not bush”
        // “The last direction generated has a highest chance of being repeat”




        // Secret doors must not interfere with main path, they must be off to the side
        // One Shop room
        // Shop room must be locked & only have one door accessing it
        // One Boss room
        // Boss room must not be next to the start, must be a certain distance
        // Boss room must only be connected to one other room


        //int roomCounter

        // Room CreateStartingRoom() => AddRoom(RoomType.Start, new Coordinate(0, 0));

        //Room startingRoom = AddRoom(RoomType.Start, new Coordinate(0, 0));

        // #region Create Rooms Connected to Starting Room

        // //AddRoom(RoomType.Normal, new Coordinate(1, 0));

        // //AddRoom(RoomType.Normal, new Coordinate(-1, 0));

        // // if (Roll(50))
        // // {
        // //     room.doors.AddLast(AddDoor(DoorType.Open, Direction.Down, /*startingRoom*/room));
        // //     //AddRoom(RoomType.Normal, new Coordinate(0, -1));
        // // }

        // #endregion


    }
  
        
        // foreach(Door door in startingRoom.doors)
        // {
        //     Coordinate newRoomCoordinate = CalculateNewRoomCoordinate(startingRoom.coordinate, door.sideOfRoom);
        //     var newRoom = AddRoom(RoomType.Normal,newRoomCoordinate);

        //     // only 2 options
        //     int doorCount = Roll(50) ? 2 : 3;//Random.Range(2, 4);

        //     for (int i = 0; i < doorCount; i++)
        //     {
        //         Direction newRoomDirection = RandomDirection();
        //         var directions = newRoom.doors.Select(door => door.sideOfRoom);
        //         while (directions.Contains(newRoomDirection)) 
        //         {
        //             newRoomDirection = RandomDirection();
        //         }
                
        //         //Door newDoor = AddDoor(DoorType.Open, newRoomDirection, newRoom);
        //         newRoom.doors.AddLast(AddDoor(DoorType.Open, newRoomDirection, newRoom));

        //         // if(Roll(50))
        //         // {
        //         //     //AddDoor(DoorType.Open, sideofRoom, newRoom);
        //         // }
        //         // else
        //         // {
        //         //     AddDoor(DoorType.Open, RandomDirection(), newRoom);
        //         // }
        //     }


       

        
        // for loop
        // Get the direction of previous room: rooms.First.direction = 
        // Use the direction to influence room creation of the next room: Random.Range(0, 4);
        // Edge case we need some way to escape from trying to create impossible rooms:
        // E.X: Tries to create two rooms when there's only space for one
        // We know with the 4 directions
        // Exit code if number of rooms in all possible directions cannot be achieved to avoid infinite loop 
    
    
        //give more weight to current direction
        //if repeatdirection new direction = old direction, else new direction = Random.Range(0,4)
    
    
    

    private static Coordinate GetOffset(Direction sideOfRoom)
    {
        Coordinate offset = new Coordinate(0, 0);
        switch(sideOfRoom)
        {
            case Direction.Up:
                offset.y = 1;
                break;
            case Direction.Down:
                offset.y = -1;
                break;
            case Direction.Left:
                offset.x = -1;
                break;
            case Direction.Right:
                offset.x = 1;
                break;
        }
        return offset;
    }

    private static Coordinate CalculateNewRoomCoordinate(Coordinate startingRoomCoordinate, Direction sideOfRoom)
    {
        Coordinate offset = GetOffset(sideOfRoom);
        return new Coordinate(startingRoomCoordinate.x + offset.x, startingRoomCoordinate.y + offset.y);
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