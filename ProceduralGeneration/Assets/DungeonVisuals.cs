using System.Collections.Generic;
using UnityEngine;

public static class DungeonVisuals
{
    static LinkedList<GameObject> roomVisuals, doorVisuals;

    public static void Init()
    {
        roomVisuals = new LinkedList<GameObject>();
        doorVisuals = new LinkedList<GameObject>();
        CreateVisualsFromModelData();
    }

    public static void DestroyDungeonVisuals()
    {
        foreach (GameObject r in roomVisuals)
            UnityEngine.GameObject.Destroy(r);
        foreach (GameObject d in doorVisuals)
            UnityEngine.GameObject.Destroy(d);

        roomVisuals.Clear();
        doorVisuals.Clear();
    }

    private static GameObject MakeRoomVisual(Room room)
    {
        GameObject roomVisual = UnityEngine.GameObject.Instantiate(Resources.Load<GameObject>("Room"));
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

    private static void MakeDoorVisual(Door door)
    {
        GameObject doorVisual = UnityEngine.GameObject.Instantiate(Resources.Load<GameObject>("Door"));
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

    public static void CreateVisualsFromModelData()
    {
        foreach (Room r in ProceduralDungeonGenerator.GetRooms())
        {
            MakeRoomVisual(r);
        }
        foreach (Door d in ProceduralDungeonGenerator.GetDoors())
        {
            MakeDoorVisual(d);
        }
    }

}
