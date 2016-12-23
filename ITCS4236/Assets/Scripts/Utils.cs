using UnityEngine;
using System.Collections;

public class Utils
{
    public static readonly string CompanyName = "Ruuber INC";
    public static readonly int MENU_SCENE = 0;
    public static readonly int LOBBY_SCENE = 1;
    public static readonly int FLOOR_SCENE = 3;

    public static void Shuffle<T>(T[] array)
    {
        for(int i = 0; i < array.Length; i++)
        {
            int rand = i + (int)(Random.value * (array.Length - i));

            T temp = array[rand];
            array[rand] = array[i];
            array[i] = temp;
        }
    }

    public static float ManhattanDistance(Vector3 first, Vector3 second)
    {
        return Mathf.Abs(first.x - second.x) + Mathf.Abs(first.y - second.y);        
    }

    public static float DistanceIgnoreHeight(Vector3 first, Vector3 second)
    {
        return Mathf.Abs(Mathf.Pow(second.x - first.x, 2.0f) + Mathf.Pow(second.z - first.z, 2.0f));
    }
}
