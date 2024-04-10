using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.EventSystems;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class Item : MonoBehaviour
{
    public ItemData itemData;


    public Item(ItemData itemData)
    {
        this.itemData = itemData;
    }


    public Item()
    {
        
    }
    public Sprite GetSprite()
    {
        if (Resources.Load<Sprite>("Sprites/Items/" + itemData.name) != null)
        {
            return Resources.Load<Sprite>("Sprites/Items/" + itemData.name);
        }
        return Resources.Load<Sprite>("Sprites/null");
    }
    public string GetRarityHexColor()
    {
        switch (itemData.rarity)
        {
            case ItemData.Rarity.COMMON: return "#FFFFFFFF";
            case ItemData.Rarity.UNCOMMON: return "#91DB69FF";
            case ItemData.Rarity.RARE: return "#4D9BE6FF";
            case ItemData.Rarity.EPIC: return "#A884F3";
            case ItemData.Rarity.LEGENDARY: return "#F9C22BFF";
            default: return "#FFFFFFFF";
        }
    }
    public override string ToString()
    {
        string text = "<u><color=" + GetRarityHexColor() + ">" + itemData.name + "</color></u>\n";
        if (itemData.description != "")
        {
            text += "'" + itemData.description + "'" + "\n";
        }
        if (itemData.physicalDamage > 0)
        {
            text += "<color=" + Toolkit.physicalDamageColor + ">" + "PDMG: +" + itemData.physicalDamage + "</color>\n";
        }
        if (itemData.magicDamage > 0)
        {
            text += "<color=" + Toolkit.magicDamageColor + ">" + "MDMG: +" + itemData.magicDamage + "</color>\n";
        }
        if (itemData.hp > 0)
        {
            text += "<color=" + Toolkit.hpColor + ">" + "HP: +" + itemData.hp + "</color>\n";
        }
        if (itemData.attackSpeed > 0)
        {
            text += "<color=" + Toolkit.attackSpeedColor + ">" + "AS: +" + itemData.attackSpeed + "%" + "</color>\n";
        }
        text += "Level: " + itemData.level;
        return text;
    }

    public string GetNameWithRarityColor()
    {
        return $"<color={GetRarityHexColor()}>{itemData.name}</color>";
    }
}
