using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase
{
    private List<Item> items = new List<Item>();
    private static ItemDatabase intance;

    public static ItemDatabase Instance()
    {
        if (intance == null)
        {
            intance = new ItemDatabase();
        }
        return intance;
    }

    public ItemDatabase()
    {
        items = new List<Item>()
        {
            new Item(new ItemData("Blue Cap","",ItemData.SlotType.Head)),
            new Item(new ItemData("Straw Hat","",ItemData.SlotType.Head)),
            new Item(new ItemData("Blue Flannel Shirt","",ItemData.SlotType.Chest)),
            new Item(new ItemData("Green Shorts","",ItemData.SlotType.Legs)),
            new Item(new ItemData("Raincoat","Desc",ItemData.SlotType.Chest)),
            new Item(new ItemData("Vagabond's Cap","",ItemData.SlotType.Head)),
            new Item(new ItemData("Vagabond's Vest","",ItemData.SlotType.Chest)),
            new Item(new ItemData("Vagabond's Trousers","",ItemData.SlotType.Legs)),
            new Item(new ItemData("Bucket","Desc",ItemData.SlotType.Head)),
            new Item(new ItemData("Mafia Fedora","",ItemData.SlotType.Head)),
            new Item(new ItemData("Baseball Cap","",ItemData.SlotType.Head)),
            new Item(new ItemData("Special Forces Beret","",ItemData.SlotType.Head)),
            new Item(new ItemData("Headband","",ItemData.SlotType.Head)),
            new Item(new ItemData("Supreme Jacket","",ItemData.SlotType.Chest)),
            new Item(new ItemData("Stick","",ItemData.SlotType.Weapon)),
            new Item(new ItemData("Grandma's Candle","",ItemData.SlotType.Weapon)),
            new Item(new ItemData("Pitchfork","",ItemData.SlotType.Weapon))
        };
        Debug.Log("<color=blue>Item Database loaded</color>");
    }
    public Item GetItem(string name)
    {
         return items.Find(item => item.itemData.name == name);
    }

    public Item GetRandomItem()
    {
        return items[Random.Range(0, items.Count)];
    }

    public List<Item> GetAllItems()
    {
        List<Item> gear = new List<Item>();
        foreach (Item item in items)
        {
            if (item is Item) { gear.Add((Item)item); }
        }
        return gear;
    }

    public List<Item> GetItemBySlot(string slot)
    {
        List<Item> gearBySlot = new List<Item>();
        foreach (Item g in GetAllItems())
        {
            if (g.itemData.slotType.ToString() == slot) { gearBySlot.Add(g); }
        }
        return gearBySlot;
    }
}
