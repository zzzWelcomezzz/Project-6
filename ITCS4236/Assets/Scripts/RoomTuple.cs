using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomTuple {
    public Room room { get; private set; }
    public List<Room> path { get; private set; }
    public float distance { get; private set; }

    public RoomTuple (Room room, List<Room> path, float distance)
    {
        this.room = room;
        this.path = path;
        this.distance = this.distance;
    }
}
