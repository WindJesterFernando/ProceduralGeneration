using System.Collections.Generic;
using Unity.VisualScripting;

public static partial class ProceduralDungeonGenerator
{
    public static void ProcedurallyGenerateDungeon()
    {
        for (int i = 0; i < 100; i++)
        {
            if (Roll(10))
                UnityEngine.Debug.Log("Roll with 10% chance: True");
            else
                UnityEngine.Debug.Log("Roll with 10% chance: False");
        }

        Room startingRoom = AddRoom(RoomType.Start, new Coordinate(0, 0));

        Room roomWestOfStartingRoom = AddRoom(RoomType.Normal, new Coordinate(1, 0));
        AddDoor(DoorType.Open, startingRoom, roomWestOfStartingRoom);

        Room roomEastOfStartingRoom = AddRoom(RoomType.Normal, new Coordinate(-1, 0));
        AddDoor(DoorType.Open, startingRoom, roomEastOfStartingRoom);

        Room roomSouthOfStartingRoom = AddRoom(RoomType.Normal, new Coordinate(0, -1));
        AddDoor(DoorType.Open, startingRoom, roomSouthOfStartingRoom);

        Room shopRoom = AddRoom(RoomType.Shop, new Coordinate(-2, 0));
        AddDoor(DoorType.Locked, roomEastOfStartingRoom, shopRoom);

        Room roomWithPassageToSecretRoom = AddRoom(RoomType.Trap, new Coordinate(1, -1));
        AddDoor(DoorType.Open, roomWestOfStartingRoom, roomWithPassageToSecretRoom);
        AddDoor(DoorType.Open, roomSouthOfStartingRoom, roomWithPassageToSecretRoom);

        Room corridorRoom1 = AddRoom(RoomType.Normal, new Coordinate(0, -2));
        AddDoor(DoorType.Open, roomSouthOfStartingRoom, corridorRoom1);

        Room corridorRoom2 = AddRoom(RoomType.Trap, new Coordinate(0, -3));
        AddDoor(DoorType.Open, corridorRoom1, corridorRoom2);

        Room treasureRoom = AddRoom(RoomType.Treasure, new Coordinate(1, -3));
        AddDoor(DoorType.Locked, corridorRoom2, treasureRoom);

        Room secretRoom = AddRoom(RoomType.Secret, new Coordinate(1, -2));
        AddDoor(DoorType.Bombable, roomWithPassageToSecretRoom, secretRoom);
        AddDoor(DoorType.Bombable, corridorRoom1, secretRoom);
        AddDoor(DoorType.Bombable, treasureRoom, secretRoom);

        Room bossRoom = AddRoom(RoomType.Boss, new Coordinate(-1, -3));
        AddDoor(DoorType.Open, corridorRoom2, bossRoom);

        Room superSecretRoom = AddRoom(RoomType.SuperSecret, new Coordinate(0, -4));
        AddDoor(DoorType.Bombable, corridorRoom2, superSecretRoom);

        foreach(Room r in GetDungeonRooms())
        {

        }

        foreach(Door d in GetDungeonDoors())
        {
            
        }

        UnityEngine.Debug.Log("There are " + GetDungeonRooms().Count + " rooms in this dungeon.");
        UnityEngine.Debug.Log("There are " + GetDungeonDoors().Count + " doors in this dungeon.");
    }

}
