using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    private Player player;
    public CharacterManager characterManager;

    private void Start()
    {
        player = this.gameObject.GetComponent<Player>();
       // characterManager = this.gameObject.GetComponent<CharacterManager>();
    }
    public void Load()
    {
        LoadVariables();
        //Load character icons and names
        for (int i = 0; i < 3; i++)
        {
            characterManager.characterIcons[i].sprite = characterManager.characters[i].GetIconSprite();
            characterManager.characterNameTexts[i].text = characterManager.characters[i].name;
        }
        characterManager.SwapCharacter(0);
    }

    private void LoadVariables()
    {
        //player.Level = PlayerPrefs.GetInt("level", 1);
       // player.Xp = PlayerPrefs.GetInt("xp", 0);
        string[] chars = { "Wurst", "Viktor", "Fleck" };
        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.SetString("character_" + i, chars[i]);
        }
        for (int i = 0; i < 3; i++)
        {
            characterManager.characters[i] = CharacterDatabase.GetCharacter(PlayerPrefs.GetString("character_" + i,"Wurst"));
        }
        
        /*
        player.Characters[0] = CharacterDatabase.GetCharacter("Wurst");
        player.Characters[1] = CharacterDatabase.GetCharacter("Viktor");
        player.Characters[2] = CharacterDatabase.GetCharacter("Fleck");
        */
    }

    private void SaveVariables()
    {
   
    }
}
