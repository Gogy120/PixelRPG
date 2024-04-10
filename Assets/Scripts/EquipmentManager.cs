using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class EquipmentManager 
{
    private string characterName;
    private Dictionary<ItemData.SlotType, ItemData> gear = new Dictionary<ItemData.SlotType, ItemData>()
    {
        { ItemData.SlotType.Head,null},
        { ItemData.SlotType.Chest,null},
        { ItemData.SlotType.Legs,null}
    };

    public EquipmentManager(string characterName)
    {
        this.characterName = characterName;
        Load();
    }


    public int GetTotalHP()
    {
        int hp =  0;
        foreach (KeyValuePair<ItemData.SlotType, ItemData> kvp in gear)
        {
            if (kvp.Value == null)
            {
                hp += 0;
            }
            else
            {
                hp += kvp.Value.hp;
            }
        }
        return hp;
    }

    public int GetTotalPhysicalDamage()
    {
        int damage = 0;
        foreach (KeyValuePair<ItemData.SlotType, ItemData> kvp in gear)
        {
            if (kvp.Value == null)
            {
                damage += 0;
            }
            else
            {
                damage += kvp.Value.physicalDamage;
            }
        }
        return damage;
    }
    public int GetTotalMagicDamage()
    {
        int damage = 0;
        foreach (KeyValuePair<ItemData.SlotType, ItemData> kvp in gear)
        {
            if (kvp.Value == null)
            {
                damage += 0;
            }
            else
            {
                damage += kvp.Value.magicDamage;
            }
        }
        return damage;
    }
    public float GetTotalAttackSpeed()
    {
        float attackSpeed = 0;
        foreach (KeyValuePair<ItemData.SlotType, ItemData> kvp in gear)
        {
            if (kvp.Value == null)
            {
                attackSpeed += 0;
            }
            else
            {
                attackSpeed += kvp.Value.attackSpeed;
            }
        }
        return attackSpeed;
    }

    public void PrintGearInfo()
    {
        ItemData.SlotType[] slotTypes = (ItemData.SlotType[])Enum.GetValues(typeof(ItemData.SlotType));
        foreach (KeyValuePair<ItemData.SlotType,ItemData> kvp in gear)
        {
            if (kvp.Value != null)
            {
                Debug.Log(kvp.Value.name);
            }
        }
    }

    public void Load()
    {
        ItemData.SlotType[] slotTypes = (ItemData.SlotType[])Enum.GetValues(typeof(ItemData.SlotType));
        foreach (ItemData.SlotType slotType in slotTypes)
        {
            ItemData item = GameSaveManager.GetEquippedItemData(characterName, slotType);
            gear[slotType] = item;        
        }
        PrintGearInfo();
    }
}
