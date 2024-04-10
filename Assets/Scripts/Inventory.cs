using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

public class Inventory : MonoBehaviour
{
    private static string savePath;
    public static List<ItemData> inventory = new List<ItemData>();
    private void Awake()
    {
        inventory = GetAllItems();
    }
    public static void SaveInventory()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(savePath);
        formatter.Serialize(file, inventory);
        file.Close();
    }

    public static List<ItemData> GetAllItems()
    {
        savePath = Application.persistentDataPath + "/inventory.dat";
        if (!File.Exists(savePath)) //Return no items if inventory.dat doesnt exist and create inventory.dat
        {
            using (FileStream fs = File.Create(savePath))
            {
                return new List<ItemData>();
            }
        }
        if (new FileInfo(savePath).Length == 0) //Return no items if inventory.dat is empty
        {
            return new List<ItemData>();
        }
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(savePath, FileMode.Open);

        List<ItemData> loadedItems = (List<ItemData>)formatter.Deserialize(file);
        file.Close();
        return loadedItems;
    }

    public static List<ItemData> GetItemsBySlot(ItemData.SlotType slotType)
    {
        List<ItemData> itemsBySlot = new List<ItemData>();
        foreach (ItemData itemData in GetAllItems())
        {
            if (itemData.slotType == slotType)
            {
                itemsBySlot.Add(itemData);
            }
        }
        return itemsBySlot;
    }

    public static void AddItem(ItemData item)
    {
        inventory = GetAllItems();
        inventory.Add(item);
        SaveInventory();
    }

    public static void RemoveItem(ItemData item)
    {
        inventory.Remove(item);
        SaveInventory();
    }


    public List<ItemData> SortInventoryByLevel()
    {
        if (inventory.Count != 0)
        {
            List<ItemData> sorted_inventory = inventory.OrderBy(item => item.level).ToList();
            return sorted_inventory;
        }
        return null;
    }
}
