using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear
{
    public string name;
    public string description;
    public int damage;
    public int hp;
    public int[] abilityPower = new int[3];
    public float attackSpeed;
    public SlotType slotType;
    public Color32 rarityColor;
    public enum SlotType
    {
        Head,Chest,Legs
    }
    public Gear(string name, string description, SlotType slotType, int damage, int hp, float attackSpeed, int[] abilityPower, Color32 rarityColor)
    {
        this.name = name;
        this.description = description;
        this.slotType = slotType;
        this.damage = damage;
        this.abilityPower = abilityPower;
        this.attackSpeed = attackSpeed;
        this.hp = hp;
        this.rarityColor = rarityColor;
    }

    public Sprite GetSprite()
    {
        return Resources.Load<Sprite>("Sprites/Items/" + name);
    }

    public override string ToString()
    {
        string text = name + " \n" + description + "\n";
        if (damage > 0)
        {
            text += "DMG: +" + damage + "\n";
        }
        if (hp > 0)
        {
            text += "HP: +" + hp + "\n";
        }
        for (int i = 0; i < abilityPower.Length; i++)
        {
            if (abilityPower[i] > 0)
            {
                text += "AB" + i + ": +" + abilityPower[i] + "\n";
            }
        }
        if (attackSpeed > 0)
        {
            text += "AS: +" + attackSpeed + "%" + "\n";
        }
        return text;
    }

    public string StatsToString()
    {
        string text = "";
        if (damage > 0)
        {
            text += "DMG: +" + damage + "\n";
        }
        if (hp > 0)
        {
            text += "HP: +" + hp + "\n";
        }
        for (int i = 0; i < abilityPower.Length; i++)
        {
            if (abilityPower[i] > 0)
            {
                text += "AB" + i + ": +" + abilityPower[i] + "\n";
            }
        }
        if (attackSpeed > 0)
        {
            text += "AS: +" + attackSpeed + "%" + "\n";
        }
        return text;
    }
}
