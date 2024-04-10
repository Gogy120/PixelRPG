using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDatabase : MonoBehaviour
{
    public static List<ICharacter> characters = new List<ICharacter>();

    void Awake()
    {
        BuildDatabase();
    }

    public static ICharacter GetCharacter(string name, Player player)
    {
        EquipmentManager equipmentManager = new EquipmentManager(name);
        switch (name)
        {
            case "Wurst": return new Wurst(player, equipmentManager);
            case "Schmalz": return new Schmalz(player, equipmentManager);
            case "Fleck": return new Fleck(player, equipmentManager);
            case "Viktor": return new Viktor(player, equipmentManager);
            case "Hermann": return new Hermann(player, equipmentManager);
            case "Frank": return new Frank(player, equipmentManager);
            default: return null;
        }
    }

    public static ICharacter GetRandomCharacter()
    {
        BuildDatabase();
        return characters[Random.Range(0, characters.Count)];
    }

    public static List<ICharacter> GetAllCharacters()
    {
        BuildDatabase();
        return characters;
    }

    static void BuildDatabase()
    {
        characters = new List<ICharacter>()
        {
            /*
            new ICharacter("Wurst","",125,8,0.35f,new int[3]{5,12,15},new int[3]{15,10,6}),
            new ICharacter("Schmalz","",200,15,0.65f,new int[3]{5,5,5},new int[3]{5,5,5}),
            new ICharacter("Fleck","",90,8,0.35f,new int[3]{6,15,20},new int[3]{20,5,12}),
            new ICharacter("Viktor","",110,12,0.5f,new int[3]{5,15,5},new int[3]{6,5,8}),
            */

        };
    }
}
