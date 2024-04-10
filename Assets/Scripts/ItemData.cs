using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[Serializable]
public class ItemData
{
    public string name;
    public string description;
    public SlotType slotType;
    public Rarity rarity;
    public int physicalDamage;
    public int magicDamage;
    public int hp;
    public float attackSpeed;
    public int level;

    public enum SlotType
    {
        Head, Chest, Legs, Weapon
    }
    public enum Rarity
    {
        COMMON,
        UNCOMMON,
        RARE,
        EPIC,
        LEGENDARY
    }

    public ItemData(string name, string description, SlotType slotType)
    {
        this.name = name;
        this.description = description;
        this.slotType = slotType;
    }
    [JsonConstructor]
    public ItemData(string name, string description, int level, SlotType slotType, Rarity rarity, int physicalDamage, int magicDamage, int hp, float attackSpeed)
    {
        this.name = name;
        this.description = description;
        this.level = level;
        this.slotType = slotType;
        this.rarity = rarity;
        this.physicalDamage = physicalDamage;
        this.magicDamage = magicDamage;
        this.hp = hp;
        this.attackSpeed = attackSpeed;
    }

    public static ItemData FromJson(string json)
    {
        Dictionary<string, string> keyValuePairs = ParseJson(json);
        string name = keyValuePairs["name"];
        return null;
    }

    private static Dictionary<string, string> ParseJson(string jsonString)
    {
        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
        string[] pairs = jsonString.Trim('{', '}').Split(',');
        foreach (string pair in pairs)
        {
            string[] keyValue = pair.Split(':');
            string key = keyValue[0].Trim(' ', '"');
            string value = keyValue[1].Trim(' ', '"');
            keyValuePairs.Add(key, value);
        }
        return keyValuePairs;
    }
}
