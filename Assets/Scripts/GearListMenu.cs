using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GearListMenu : MonoBehaviour
{
    public GameObject scrollContent;
    public TextMeshProUGUI gearText;
    public Button equipButton;
    public TextMeshProUGUI gearNameText;
    public GameObject gearSlot;
    public GameObject gearSlotLock;

    private string characterName;
    private string selectedSlot;
    private float step = 225;
    private string selectedGearName;

    void Start()
    {
        characterName = PlayerPrefs.GetString("equipment_selected_character", "Wurst");
        selectedSlot = PlayerPrefs.GetString("equipment_selected_slot", "Head");
        PlayerPrefs.SetInt("No Head_unlocked",1);
        PlayerPrefs.SetInt("No Chest_unlocked", 1);
        PlayerPrefs.SetInt("No Legs_unlocked", 1);
        GenerateSlots();
    }

    public void EquipItem()
    {
        PlayerPrefs.SetString(characterName + "_" + selectedSlot, selectedGearName);
        Toolkit.GoToScene("CharacterEquipment");
    }

    public void SelectItem(string gearName)
    {
        equipButton.gameObject.SetActive(true);
        Gear gear = GearDatabase.GetGear(gearName);
        selectedGearName = gearName;
        gearText.text = gear.description + "\n" + gear.StatsToString();
        gearNameText.text = gear.name;
        gearNameText.color = gear.rarityColor;
    }

    private void GenerateSlots()
    {
        Vector2 pos = new Vector2(-step * 2, 525);
        List<Gear> gear = GearDatabase.GetGearBySlot(selectedSlot);
        foreach (Gear g in gear)
        {
            pos = new Vector2(pos.x + step, pos.y);
            if (pos.x > step)
            {
                pos = new Vector2(-step,pos.y - step);
            }
            GameObject slot = Instantiate(gearSlot,Vector2.zero,Quaternion.identity);
            slot.transform.parent = scrollContent.transform;
            RectTransform rectTransform = slot.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = pos;
            rectTransform.localScale = new Vector2(1, 1);
            slot.transform.GetChild(0).GetComponent<Image>().sprite = g.GetSprite();
            slot.GetComponent<Button>().onClick.AddListener(delegate { SelectItem(g.name); });
            if (PlayerPrefs.GetInt(g.name + "_unlocked", 0) == 0)
            {
                GameObject slotLock = Instantiate(gearSlotLock, Vector2.zero, Quaternion.identity);
                slotLock.transform.parent = scrollContent.transform;
                rectTransform = slotLock.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = pos;
                rectTransform.localScale = new Vector2(1, 1);
            }
        }
        Debug.Log(pos);
    }

}
