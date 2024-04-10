using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemCard : MonoBehaviour
{
    [Header("Components")]
    public TextMeshProUGUI nameText;
    public Image icon;

    [HideInInspector] public Item item;

    public void Setup(Item item)
    {
        nameText.text = item.GetNameWithRarityColor();
        icon.sprite = item.GetSprite();
        this.item = item;
    }

    public void DisplayItem()
    {
        GameObject.Find("CharacterEquipMenu").GetComponent<CharacterEquipMenu>().DisplayItem(item);
    }
}
