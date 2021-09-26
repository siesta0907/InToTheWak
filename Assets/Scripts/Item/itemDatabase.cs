using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemDatabase : MonoBehaviour
{
    public List<Item> items = new List<Item>();


    void Start()
    {

        items.Add(new Item("0", "Iron Sword", 1001, "강력한 검이다.", 10, 10, 10, 0, Item.ItemType.Weapon));

    }

    void Update()

    {
        
    }
}
