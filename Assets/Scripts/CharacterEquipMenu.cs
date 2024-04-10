using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine.SceneManagement;


public class CharacterEquipMenu : MonoBehaviour
{
    [Header("Components")]
    public Animator characterAnimator;
    public Image[] characterIcons = new Image[3];
    public TextMeshProUGUI[] characterNameTexts = new TextMeshProUGUI[3];
    public Transform contentPanel;
    public Image itemInfoIcon;
    public TextMeshProUGUI itemInfoText;
    public Animator itemInfoPanelAnimator;
    public Animator itemScrollViewAnimator;
    public Image[] itemSlotIcons = new Image[3];

    [Header("Resources")]
    public GameObject itemCardPrefab;

    private string[] characters = new string[3];
    private string currentCharacter = null;
    private Item currentItem = null;
    private bool menuInitialized = false;
    void Start()
    {
        characters = GameSaveManager.LoadCharacterString();
        SelectCharacter(0);
        InitMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateUI()
    {
        ItemData.SlotType[] slotTypes = (ItemData.SlotType[])Enum.GetValues(typeof(ItemData.SlotType));
        for (int i = 0; i < slotTypes.Length; i++)
        {
            if (PlayerPrefs.GetString($"{currentCharacter}_{slotTypes[i].ToString()}", "Null") != "Null")
            {
                itemSlotIcons[i].sprite = GameSaveManager.GetEquippedItem(currentCharacter, slotTypes[i]).GetSprite();
            }
            else
            {
                switch (i)
                {
                    case 0: itemSlotIcons[i].sprite = Resources.Load<Sprite>("Sprites/UI/NoHead"); break;
                    case 1: itemSlotIcons[i].sprite = Resources.Load<Sprite>("Sprites/UI/NoChest"); break;
                    case 2: itemSlotIcons[i].sprite = Resources.Load<Sprite>("Sprites/UI/NoLegs"); break;
                    case 3: itemSlotIcons[i].sprite = Resources.Load<Sprite>("Sprites/UI/NoWeapon"); break;
                }
            }
        }
    }

    private void InitMenu()
    {
        for (int i = 0; i < characterIcons.Length; i++)
        {
            characterIcons[i].sprite = Resources.Load<Sprite>("Sprites/UI/CharacterIcons/Icon_" + characters[i]);
        }
        for (int i = 0; i < characterNameTexts.Length; i++)
        {
            characterNameTexts[i].text = characters[i];
        }
        menuInitialized = true;
    }

    public void SelectCharacter(int index)
    {
        currentCharacter = characters[index];
        characterAnimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/UI/Characters/" + currentCharacter);
        UpdateUI();
    }

    private void RenderItems(List<ItemData> items)
    {
        if (!itemScrollViewAnimator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
        {
            itemScrollViewAnimator.SetTrigger("open");
        }
        ClearItemCards();
        float posY = -62.5f;
        foreach (ItemData itemData in items)
        {
            GameObject itemCard = Instantiate(itemCardPrefab, contentPanel);
            itemCard.GetComponent<ItemCard>().Setup(new Item(itemData));
            RectTransform newItemTransform = itemCard.GetComponent<RectTransform>();
            newItemTransform.anchorMin = new Vector2(0.5f, 1);
            newItemTransform.anchorMax = new Vector2(0.5f, 1);
            newItemTransform.pivot = new Vector2(0.5f, 1f);
            newItemTransform.anchoredPosition = new Vector2(0, posY);
            posY -= newItemTransform.rect.height * newItemTransform.localScale.y + 50;
        }
        RectTransform contentRectTransform = contentPanel.GetComponent<RectTransform>();
        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, Mathf.Abs(items.Count * 218.75f));
    }
    public void RenderAllItems()
    {
        List<ItemData> allItems = Inventory.GetAllItems();
        RenderItems(allItems);
    }

    private void RenderItemsBySlot(ItemData.SlotType slotType)
    {
        List<ItemData> itemsBySlot = Inventory.GetItemsBySlot(slotType);
        RenderItems(itemsBySlot);
    }

    public void DisplayItem(Item item)
    {
        if (!itemInfoPanelAnimator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
        {
            itemInfoPanelAnimator.SetTrigger("open");
        }
        currentItem = item;
        itemInfoIcon.sprite = currentItem.GetSprite();
        itemInfoText.text = currentItem.ToString();
    }

    public void ClosePanels()
    {
        if (menuInitialized)
        {
            if (!itemInfoPanelAnimator.GetCurrentAnimatorStateInfo(0).IsName("Close") || !itemInfoPanelAnimator.GetCurrentAnimatorStateInfo(0).IsName("Empty"))
            {
                itemInfoPanelAnimator.SetTrigger("close");
            }
            if (!itemScrollViewAnimator.GetCurrentAnimatorStateInfo(0).IsName("Close") || !itemScrollViewAnimator.GetCurrentAnimatorStateInfo(0).IsName("Empty"))
            {
                itemScrollViewAnimator.SetTrigger("close");
            }
            currentItem = null;
        }
    }

    private void ClearItemCards()
    {
        GameObject[] itemCards = GameObject.FindGameObjectsWithTag("ItemCard");
        foreach (GameObject itemCard in itemCards)
        {
            Destroy(itemCard);
        }
    }

    public void SelectSlot(string slotTypeString)
    {
        ItemData.SlotType slotType;
        if (Enum.TryParse(slotTypeString, out slotType))
        {
            RenderItemsBySlot(slotType);
        }
        if (PlayerPrefs.GetString($"{currentCharacter}_{slotType.ToString()}", "Null") != "Null")
        {
            DisplayItem(GameSaveManager.GetEquippedItem(currentCharacter, slotType));
        }
    }

    public void EquipItem()
    {
        string serializedItemData = JsonConvert.SerializeObject(currentItem.itemData);
        PlayerPrefs.SetString($"{currentCharacter}_{currentItem.itemData.slotType.ToString()}", serializedItemData);
        UpdateUI();
    }
    private bool IsAnimationPlaying(Animator animator)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0 && !animator.IsInTransition(0))
        {
            return false;
        }
        return true;
    }

    public void GoToScene(string sceneName)
    {
        StartCoroutine(Toolkit.GoToScene(sceneName));
    }
}
