using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CharacterEquipmentMenu : MonoBehaviour
{
    public Image[] gearSprites = new Image[3];
    public TextMeshProUGUI[] gearNameTexts = new TextMeshProUGUI[3];
    public Animator characterAnimator;
    private string characterName;

    private string[] gearSlots = { "Head", "Chest","Legs"};
    private List<Gear> equippedGear = new();
    void Start()
    {
        characterName = PlayerPrefs.GetString("equipment_selected_character","Wurst");
        characterAnimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/Characters/" + characterName + "/" + characterName + "_UI");
        equippedGear = GetEquippedGear();
        SetGearSprites();
        SetGearNameTexts();
    }
    public void SelectGear(string slot)
    {
        PlayerPrefs.SetString("equipment_selected_slot", slot);
        Toolkit.GoToScene("GearList");
    }

    public void GotoScene(string scene)
    {
        Toolkit.GoToScene(scene);
    }

    private void SetGearSprites()
    {
        for (int i = 0; i < gearSprites.Length; i++)
        {
            gearSprites[i].sprite = equippedGear[i].GetSprite();
        }
    }

    private List<Gear> GetEquippedGear()
    {
        List<Gear> eg = new();
        for (int i = 0; i < gearSlots.Length; i++)
        {
            eg.Add(GearDatabase.GetGear(PlayerPrefs.GetString(characterName + "_" + gearSlots[i], "No " + gearSlots[i])));
        }
        return eg;
    }

    private void SetGearNameTexts()
    {
        for (int i = 0; i < gearSprites.Length; i++)
        {
            gearNameTexts[i].text = equippedGear[i].name;
            gearNameTexts[i].color = equippedGear[i].rarityColor;       
        }
    }
}
