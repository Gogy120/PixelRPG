using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDatabase : MonoBehaviour
{
    public static List<Character> characters = new List<Character>();

    void Awake()
    {
        BuildDatabase();
    }

    public static Character GetCharacter(string name)
    {
        BuildDatabase();
        return characters.Find(item => item.name == name);
    }

    public static Character GetRandomCharacter()
    {
        BuildDatabase();
        return characters[Random.Range(0, characters.Count)];
    }

    public static List<Character> GetAllCharacters()
    {
        BuildDatabase();
        return characters;
    }

    static void BuildDatabase()
    {
        characters = new List<Character>()
        {
            new Character("Wurst","",125,8,0.35f,new int[3]{5,12,15},new int[3]{15,10,6}),
            new Character("Schmalz","",200,15,0.65f,new int[3]{5,5,5},new int[3]{5,5,5}),
            new Character("Fleck","",90,8,0.35f,new int[3]{6,15,20},new int[3]{20,5,12}),
            new Character("Viktor","",110,12,0.5f,new int[3]{5,15,5},new int[3]{6,5,8}),
        };
    }
}
