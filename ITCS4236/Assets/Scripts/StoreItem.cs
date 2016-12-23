using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class StoreItem
{
    public Sprite image { get; private set; }
    public string title { get; private set; }
    public string description { get; private set; }
    public int price { get; private set; }
    public float timeToMake { get; private set; }

    public StoreItem(Sprite image, string title, string description, int price, float timeToMake)
    {
        this.image = image;
        this.title = title;
        this.description = description;
        this.price = price;
        this.timeToMake = timeToMake;
    }
}
