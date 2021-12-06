using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class Inventory : MonoBehaviour
{
    public List<Item> inventory = new List<Item>();
    private itemDatabase db;

    public int slotX, slotY;  
    public List<Item> slots = new List<Item>(); 

    private bool showInventory = false;
    public GUISkin skin;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < slotX * slotY; i++)
        {
            slots.Add(new Item());
            inventory.Add(new Item());
        }
        db = GameObject.FindGameObjectWithTag("Item Database").GetComponent<itemDatabase>();

       // inventory[0] = db.items[0];
        for (int i = 0; i < slotX * slotY; i++)
        {
            if (db.items[i] != null)
            {
                inventory[i] = db.items[i];
            }
            else
            {

            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))

        {
            showInventory = !showInventory;
            
        }
    }
    void OnGUI()
    {
        GUI.skin = skin;
        if (showInventory)
        {
            DrawInventory();
        }
    }

    void DrawInventory()
    {
        int k = 0;
        for (int j = 0; j < slotY; j++)
        {

            for (int i = 0; i < slotX; i++)
            {
                Rect slotRect = new Rect(i * 52 + 100, j * 52 + 30, 50, 50);
                GUI.Box(slotRect, "", skin.GetStyle("slot background"));
                slots[k] = inventory[k];
                if (slots[k].itemName != null)
                {
                    //여기
                    //GUI.DrawTexture(slotRect, slots[k].itemIcon);
                    Debug.Log(slots[k].itemName);
                    Debug.Log("실행"+ k);
                }
                k++;
            }
        }
    }
}
*/