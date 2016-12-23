using UnityEngine;
using System.Collections;

public class Room
{
    public static readonly float MODEL_SIZE = 25.0f;
    public static readonly float LENGTH = 1.0f * MODEL_SIZE;
    public static readonly float SCALE = LENGTH / MODEL_SIZE;
    public static readonly float WALL_WIDTH = 0.2f;

    private GameObject room;    
    private string[] room_models = { "Room001Fixed", "Room004Fixed", "Room005", "Room006" };

    public Vector3 location { get; private set; }
    public float weight;
    public Point roomIndex;
    public bool active = false;
    public bool[] doors = {false, false, false, false};

    public Room(Vector3 location, Point roomIndex)
    {
        this.location = location;
        weight = Random.value;
        this.roomIndex = roomIndex;
    }

    public GameObject Instantiate(Point orientation)
    {
        room = (GameObject)GameObject.Instantiate(Resources.Load(GetRandomRoomModel()), location, new Quaternion());

        room.name = "room" + orientation.ToString();
        room.transform.localScale = new Vector3(SCALE, 1.0f, SCALE);

        this.active = true;

        return room;
    }

    public GameObject GetRoom()
    {
        return room;
    }

    public override string ToString()
    {
        return location.ToString() + " " + roomIndex.ToString();
    }

    private string GetRandomRoomModel()
    {
        return room_models[Random.Range(0, room_models.Length)];
    }
}
