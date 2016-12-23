using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MapGenerator
{
    public enum Core { COMBAT, UTILITY, CONSTRUCTION, MAIN };
    public Vector3 origin { get; private set; }
    public Core[] coreQuadrants;
    public Dictionary<Core, Room[,]> coreToRooms;

    public int Size;
    public Room[,] rooms;

    public const int COMBAT_CORE_SIZE       = 10;
    public const int UTILITY_CORE_SIZE      = 10;
    public const int CONSTRUCTION_CORE_SIZE = 10;
    public const int MAIN_CORE_SIZE         = 10;

    public static GameObject door;
    public static readonly Vector3 doorOffset = new Vector3(0.0f, -2.56f, 0.0f);

    private static Transform PlayerTransform;

    public int branchFactor = 3;

    public MapGenerator(Vector3 origin, int seed = 0)
    {
        if(seed != 0)
            UnityEngine.Random.InitState(seed);

        this.origin = origin;
        PlayerTransform = GameObject.FindWithTag("Player").transform;
        coreToRooms = new Dictionary<Core, Room[,]>();
        coreQuadrants = new Core[4] { Core.COMBAT, Core.UTILITY, Core.CONSTRUCTION, Core.MAIN };
        Utils.Shuffle(coreQuadrants);        

        door = Resources.Load<GameObject>("SciFi_Door\\Prefabs\\SF_Door");

        GenerateQuadrant(coreQuadrants[0]);
    }

    private void PrintMap()
    {
        for(int i = 0; i < Size; i++)
        {
            string line = string.Empty;
            for(int j = 0; j < Size; j++)
            {
                line += rooms[i, j] == null ? "0" : "1";
            }

            Debug.Log(line);
        }
    }

    private void GenerateWorld()
    {
    }

    public Room GetPlayerRoom()
    {
        Vector3 playerLocation = PlayerTransform.position;

        int xPos = (int)((playerLocation.x) / Room.LENGTH);
        int zPos = (int)((playerLocation.z) / Room.LENGTH);

        Debug.Log(xPos + ", " + zPos + " " + playerLocation + " max " + Size);

        return rooms[xPos, zPos];
    }

    public Room GetRoomAt(int i, int j)
    {
        return rooms[i, j];
    }

    private void GenerateQuadrant(Core core)
    {
        Size = GetSizeFromCore(core);
        rooms = new Room[Size, Size];

        for(int i = 0; i < Size; i++)
        {
            for(int j = 0; j < Size; j++)
            {
                rooms[i, j] = new Room(new Vector3(i * Room.LENGTH, 0.0f, j * Room.LENGTH), new Point(i,j));
            }
        }

        List<Room> path = AStarSearch(rooms, core, new Point(0, 0), new Point(Size - 1, Size - 1));

        Debug.Log(path.Count + " LOASKDOAISNF");

        GameObject prevRoomObject = null;
        GameObject roober = null;     

        for(int i = 0; i < path.Count; i++)
        {
            Room room = path[i];

            if (roober != null)
            {
                prevRoomObject = roober;
            }

            roober = room.Instantiate(room.roomIndex);

            if (i != 0)
            {
                Room prevRoom = path[i - 1];

                generateDoors(roober, prevRoomObject, room, prevRoom);
            }           

        }    

        for (int j = 0; j < branchFactor; j++)
        {

            int endX = (int)Math.Floor(UnityEngine.Random.value * rooms.GetLength(0));
            int endY = (int)Math.Floor(UnityEngine.Random.value * rooms.GetLength(1));

            Room branchEnd  = path[(int)Math.Floor(UnityEngine.Random.value * path.Count)];
            Room branchRoom = path[(int)Math.Floor(UnityEngine.Random.value * path.Count)]; 


            //DEBUG
            //todo: implement dead end max

            //generates a branch between a random room in the path and any random room (could be dead end)
            //List<Room> branch = AStarSearch(rooms, core, branchRoom.roomIndex, new Point(endX, endY), true);

            //generates a branch between a random room in the path to another random room in the path
            List<Room> branch = AStarSearch(rooms, core, branchRoom.roomIndex, branchEnd.roomIndex, true);


            for (int i = 0; i < branch.Count; i++)
            {
                Room room = branch[i];

                if (roober != null)
                {
                    prevRoomObject = roober;
                }

                if (room.active)
                    roober = room.GetRoom();
                else
                    roober = room.Instantiate(room.roomIndex);

                if (i != 0)
                {
                    Room prevRoom = branch[i - 1];
                    generateDoors(roober, prevRoomObject, room, prevRoom);
                }

            }
        }
       
    }

    private void generateDoors(GameObject roober, GameObject prevRoomObject, Room room, Room prevRoom)
    {
        GameObject first = null, second = null;
        Quaternion rotation = Quaternion.identity;

        bool shouldMakeDoor = false;

        // Eww

        //left door
        if (room.roomIndex.x - 1 == prevRoom.roomIndex.x && room.roomIndex.y == prevRoom.roomIndex.y && !room.doors[2])
        {
            first = roober.transform.Find("WallLeft").gameObject.transform.Find("WallLeftCubeM").gameObject;
            second = prevRoomObject.transform.Find("WallRight").gameObject.transform.Find("WallRightCubeM").gameObject;
            rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
            room.doors[2] = shouldMakeDoor = true;
        }
        //right door
        else if (room.roomIndex.x + 1 == prevRoom.roomIndex.x && room.roomIndex.y == prevRoom.roomIndex.y && !room.doors[0])
        {
            first = roober.transform.Find("WallRight").gameObject.transform.Find("WallRightCubeM").gameObject;
            second = prevRoomObject.transform.Find("WallLeft").gameObject.transform.Find("WallLeftCubeM").gameObject;
            rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            room.doors[0] = shouldMakeDoor = true;
        }
        //top door
        else if (room.roomIndex.x == prevRoom.roomIndex.x && room.roomIndex.y + 1 == prevRoom.roomIndex.y && !room.doors[1])
        {
            first = roober.transform.Find("WallTop").gameObject.transform.Find("WallTopCubeM").gameObject;
            second = prevRoomObject.transform.Find("WallBottom").gameObject.transform.Find("WallBottomCubeM").gameObject;
            room.doors[1] = shouldMakeDoor = true;
        }
        //bottom door
        else if (room.roomIndex.x == prevRoom.roomIndex.x && room.roomIndex.y - 1 == prevRoom.roomIndex.y && !room.doors[3])
        {
            first = roober.transform.Find("WallBottom").gameObject.transform.Find("WallBottomCubeM").gameObject;
            second = prevRoomObject.transform.Find("WallTop").gameObject.transform.Find("WallTopCubeM").gameObject;
            room.doors[3] = shouldMakeDoor = true;
        }

        if(shouldMakeDoor)
            InstantiateDoor(first.transform.position, rotation);

        UnityEngine.Object.Destroy(first);
        UnityEngine.Object.Destroy(second);
    }

    private void InstantiateDoor(Vector3 location, Quaternion rotation)
    {
        GameObject generatedDoor = (GameObject) GameObject.Instantiate(door, location + doorOffset, rotation);
        generatedDoor.transform.localScale = new Vector3(Room.SCALE, 1.0f, Room.SCALE);
        //generatedDoor.transform.position -= new Vector3(0f, 2.5f, 0f);
    }

    private int GetSizeFromCore(Core core)
    {
        switch (core)
        {
            case Core.COMBAT:       return COMBAT_CORE_SIZE;
            case Core.UTILITY:      return UTILITY_CORE_SIZE;
            case Core.CONSTRUCTION: return CONSTRUCTION_CORE_SIZE;
            case Core.MAIN:         return MAIN_CORE_SIZE;
            default:                return 0;
        }

    }

    //setting branch to true greatly increases the "randomness" of the generated path IN CONTRAST TO the previously generated path
    //if you only have one path, setting branch to true will not make the path "more random"
    private List<Room> AStarSearch(Room[,] rooms, Core core, Point start, Point end, bool branch = false)
    {
        int size = GetSizeFromCore(core);

        HashSet<Room> visited = new HashSet<Room>();
        RoomPriorityQueue pQueue = new RoomPriorityQueue(size * size);

        List<Room> path = new List<Room>();
        List<Room> newPath = null;
        float newPathDistance = 0.0f;
        Vector3 finalRoomLocation = rooms[end.x, end.y].location;

        path.Add(rooms[start.x, start.y]);

        pQueue.Add(new RoomTuple(rooms[start.x, start.y], path,
            Utils.ManhattanDistance(rooms[start.x, start.y].location, finalRoomLocation)));
         
        while (!pQueue.IsEmpty())
        {
            RoomTuple current = pQueue.Remove();

            if (current.room.location == finalRoomLocation)
            {
                return current.path;
            } 

            if (!visited.Contains(current.room))
            {
                visited.Add(current.room);

                foreach(var successor in GetSuccessors(rooms, current.room, core)) 
                {
                    if (!visited.Contains(successor))
                    {
                        newPath = new List<Room>(current.path);

                        if (branch)
                            successor.weight = UnityEngine.Random.value;   

                        newPath.Add(successor);
                        newPathDistance = Utils.ManhattanDistance(successor.location, finalRoomLocation);
                        pQueue.Add(new RoomTuple(successor, newPath, newPathDistance));
                    }
                }
            }
        }

        return new List<Room>();
    }

    private List<Room> GetSuccessors(Room[,] rooms, Room room, Core core)
    {
        int size = GetSizeFromCore(core);
        int x = room.roomIndex.x;
        int y = room.roomIndex.y;
        List<Room> successors = new List<Room>();

        if (x + 1 < size)
        {
            successors.Add(rooms[x + 1, y]);
        }

        if (x - 1 >= 0)
        {
            successors.Add(rooms[x - 1, y]);
        }

        if (y + 1 < size)
        {
            successors.Add(rooms[x, y + 1]);
        }

        if (y - 1 >= 0)
        {
            successors.Add(rooms[x, y - 1]);
        }

        return successors;
    }
}
