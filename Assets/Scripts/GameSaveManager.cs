using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

public class GameSaveManager : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = this.GetComponent<Player>();
        Load();
    }
    public void Load()
    {
        //LoadVariables();
        //Load character icons and names
        player.characterManager.characters = player.gameSaveManager.LoadCharacters();
        for (int i = 0; i < 3; i++)
        {
            player.characterIcons[i].sprite = player.characterManager.characters[i].GetIconSprite();
            player.characterNameTexts[i].text = player.characterManager.characters[i].name;
        }
        player.characterManager.SwapCharacter(0);
        SceneManager.LoadScene(PlayerPrefs.GetString("current_map", "Brawl1"), LoadSceneMode.Additive);
    }

    private void LoadVariables()
    {
        string[] chars = { "Wurst", "Fleck", "Viktor" };
        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.SetString("character_" + i, chars[i]);
        }
        for (int i = 0; i < 3; i++)
        {
            ICharacter character = CharacterDatabase.GetCharacter(PlayerPrefs.GetString("character_" + i, "Wurst"), player);
            player.characterManager.characters[i] = character;
        }
    }

    public static string[] LoadCharacterString()
    {
        string[] characters = new string[3];
        for (int i = 0; i < characters.Length; i++)
        {
            string defaultCharacter = "Wurst";
            if (i == 1) { defaultCharacter = "Schmalz"; }
            else if (i == 2) { defaultCharacter = "Fleck"; }
            characters[i] = PlayerPrefs.GetString("character_" + i, defaultCharacter);
        }
        return characters;
    }

    public ICharacter[] LoadCharacters()
    {
        ICharacter[] characters = new ICharacter[3];
        for (int i = 0; i < 3; i++)
        {
            string defaultCharacter = "Wurst";
            if (i == 1) { defaultCharacter = "Viktor"; }
            else if (i == 2) { defaultCharacter = "Fleck"; }
            ICharacter character = CharacterDatabase.GetCharacter(PlayerPrefs.GetString("character_" + i, defaultCharacter), player);
            characters[i] = character;
        }
        return characters;
    }

    public static Item GetEquippedItem(string characterName, ItemData.SlotType slotType)
    {
        string itemJson = PlayerPrefs.GetString($"{characterName}_{slotType.ToString()}", "Null");
        if (itemJson.Equals("Null"))
        {
            return null;
        }
        ItemData itemData = JsonConvert.DeserializeObject<ItemData>(itemJson);
        Item item = new Item(itemData);
        return item;
    }

    public static ItemData GetEquippedItemData(string characterName, ItemData.SlotType slotType)
    {
        string itemJson = PlayerPrefs.GetString($"{characterName}_{slotType.ToString()}", "Null");
        if (itemJson.Equals("Null"))
        {
            return null;
        }
        ItemData itemData = JsonConvert.DeserializeObject<ItemData>(itemJson);
        return itemData;
    }

    public void Save()
    {
       // PlayerPrefs.SetString("currentScene", SceneManager.GetAllScenes()[1].name);
        PlayerPrefs.SetFloat("posX",this.transform.position.x);
        PlayerPrefs.SetFloat("posY",this.transform.position.y);
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    public static Dictionary<string, KeyCode> LoadKeyBinds(Dictionary<string,KeyCode> keyBinds)
    {
        Dictionary<string, KeyCode> loadedKeyBinds = new Dictionary<string, KeyCode>();
        foreach (KeyValuePair<string, KeyCode> kvp in keyBinds)
        {
            KeyCode loadedKey;
            if (Enum.TryParse(PlayerPrefs.GetString(kvp.Key + "_key","default"), out loadedKey))
            {

            }
            else
            {
                loadedKey = kvp.Value;
            }
            loadedKeyBinds.Add(kvp.Key, loadedKey);
        }
        return loadedKeyBinds;
    }
}
