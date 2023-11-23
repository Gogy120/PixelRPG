using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EquipmentCharacterSelect : MonoBehaviour
{
    public Animator[] characterAnimators = new Animator[3];
    public TextMeshProUGUI[] characterNameTexts = new TextMeshProUGUI[3];

    private string[] characterNames = new string[3];
    void Start()
    {
        for (int i = 0; i < characterNames.Length; i++)
        {
            characterNames[i] = PlayerPrefs.GetString("character_" + i, "Wurst");
            characterNameTexts[i].text = characterNames[i];
            characterAnimators[i].runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/Characters/" + characterNames[i] + "/" + characterNames[i] + "_UI");
        }     
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F10)) { Toolkit.GoToScene("MainScene"); }
        if (Input.GetKeyDown(KeyCode.F12)) { PlayerPrefs.DeleteAll(); }
    }

    public void SelectCharacter(int index)
    {
        PlayerPrefs.SetString("equipment_selected_character", characterNames[index]);
        Toolkit.GoToScene("CharacterEquipment");
    }
}
