using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EquipmentManager : MonoBehaviour
{
    private string characterName;
    private Dictionary<Gear.SlotType, Gear> gear = new Dictionary<Gear.SlotType, Gear>()
    {
        { Gear.SlotType.Head,GearDatabase.GetGear("No Head")},
        { Gear.SlotType.Chest,GearDatabase.GetGear("No Chest")},
        { Gear.SlotType.Legs,GearDatabase.GetGear("No Legs")}
    };

    public EquipmentManager(string characterName)
    {
        this.characterName = characterName;
    }

    public void Equip(Gear.SlotType slot, Gear item)
    {
        gear[slot] = item;
        Save();
    }

    public int GetTotalHP()
    {
        int hp =  0;
        foreach (KeyValuePair<Gear.SlotType, Gear> kvp in gear)
        {
            hp += kvp.Value.hp;
        }
        return hp;
    }

    public int GetTotalDamage()
    {
        int damage = 0;
        foreach (KeyValuePair<Gear.SlotType, Gear> kvp in gear)
        {
            damage += kvp.Value.damage;
        }
        return damage;
    }
    public int[] GetTotalAbilityPower()
    {
        int[] abilityPower = new int[3] { 0, 0, 0 };
        foreach (KeyValuePair<Gear.SlotType, Gear> kvp in gear)
        {
            for (int i = 0; i < abilityPower.Length; i++)
            {
                abilityPower[i] += kvp.Value.abilityPower[i];
            }
        }
        return abilityPower;
    }
    public float GetTotalAttackSpeed()
    {
        float attackSpeed = 0;
        foreach (KeyValuePair<Gear.SlotType, Gear> kvp in gear)
        {
            attackSpeed += kvp.Value.attackSpeed;
        }
        return attackSpeed;
    }

    public void PrintGearInfo()
    {
        foreach (KeyValuePair<Gear.SlotType, Gear> kvp in gear)
        {
            Debug.Log(kvp.Value.ToString());
        }
    }

    public void Load()
    {
        gear[Gear.SlotType.Head] = GearDatabase.GetGear(PlayerPrefs.GetString(characterName + "_Head", "No Head"));
        gear[Gear.SlotType.Chest] = GearDatabase.GetGear(PlayerPrefs.GetString(characterName + "_Chest", "No Chest"));
        gear[Gear.SlotType.Legs] = GearDatabase.GetGear(PlayerPrefs.GetString(characterName + "_Legs", "No Legs"));
    }

    public void Save()
    {
        foreach (KeyValuePair<Gear.SlotType, Gear> kvp in gear)
        {
            PlayerPrefs.SetString(characterName + "_" + kvp.Key,kvp.Value.name);
        }
    }

}
