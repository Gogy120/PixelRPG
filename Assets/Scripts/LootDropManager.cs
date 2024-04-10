using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDropManager : MonoBehaviour
{
    [Header("Variables")]
    public int dropChance;
    [Header("Components")]
    public GameObject droppedLoot;

    private const float scaleConstant = 1.13f;
    private const float baseStat = 2;
    void Start()
    {
        
    }

    public void DropLoot(Vector2 pos)
    {
        if (Random.Range(0,100) <= dropChance)
        {
            GameObject loot = Instantiate(droppedLoot, pos, Quaternion.identity);
            Item lootItem = loot.GetComponent<Item>();
            Item randomItem = GetRandomItem();
            //Set loot object Item properties
            lootItem.itemData = randomItem.itemData;


            loot.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Sprites/Items/Rarities/{lootItem.itemData.slotType}_{lootItem.itemData.rarity}");           
            return;
        }

    }

    public ItemData.Rarity GetRandomRarity()
    {
        int randomNumber = Random.Range(1, 101); // Generate a random number between 1 and 100

        if (randomNumber <= 50) // 50% chance for COMMON
            return ItemData.Rarity.COMMON;
        else if (randomNumber <= 80) // 30% chance for UNCOMMON (total 80%)
            return ItemData.Rarity.UNCOMMON;
        else if (randomNumber <= 95) // 15% chance for RARE (total 95%)
            return ItemData.Rarity.RARE;
        else if (randomNumber <= 99) // 4% chance for EPIC (total 99%)
            return ItemData.Rarity.EPIC;
        else // 1% chance for LEGENDARY (total 100%)
            return ItemData.Rarity.LEGENDARY;
    }

    public Item GetRandomItem()
    {
        int level = PlayerPrefs.GetInt("level", 1);
        ItemData.Rarity rarity = GetRandomRarity();

        int physicalDamage = 0;
        int magicDamage = 0;
        if (Random.Range(0, 2) == 0)
        {
            physicalDamage = GenerateStat(rarity);
        }
        else
        {
            magicDamage = GenerateStat(rarity);
        }
        int hp = GenerateHp(rarity);
        int attackSpeed = GenerateAttackSpeed(rarity);

        Item randomItem = ItemDatabase.Instance().GetRandomItem();
        Item droppedItem = new Item(new ItemData(randomItem.itemData.name, randomItem.itemData.description, level, randomItem.itemData.slotType, rarity, physicalDamage, magicDamage, hp, attackSpeed));
        return droppedItem;
    }

    private int GenerateStat(ItemData.Rarity rarity)
    {
        int level = PlayerPrefs.GetInt("level", 1);
        int mid = Mathf.RoundToInt(baseStat * Mathf.Pow(scaleConstant, level - 1) * GetRarityMultiplier(rarity));
        int max = Mathf.RoundToInt(mid * 1.15f + 1);
        int min = Mathf.RoundToInt(mid * 0.85f);
        int result = Mathf.RoundToInt(Random.Range(min, max));
        return result;
    }

    private int GenerateHp(ItemData.Rarity rarity)
    {
        return Mathf.RoundToInt(GenerateStat(rarity) * 2f);
    }

    private int GenerateAttackSpeed(ItemData.Rarity rarity)
    {
        return Mathf.Clamp(Mathf.RoundToInt(GenerateStat(rarity) * 0.5f),0,100);
    }

    private float GetRarityMultiplier(ItemData.Rarity rarity)
    {
        switch (rarity)
        {
            case ItemData.Rarity.COMMON: return 1;
            case ItemData.Rarity.UNCOMMON: return 1.5f;
            case ItemData.Rarity.RARE: return 2;
            case ItemData.Rarity.EPIC: return 2.5f;
            case ItemData.Rarity.LEGENDARY: return 3;
            default: return 1;
        }
    }

    private int GetRarityStatAmount(ItemData.Rarity rarity)
    {
        switch (rarity)
        {
            case ItemData.Rarity.COMMON: return 1;
            case ItemData.Rarity.UNCOMMON: return 2;
            case ItemData.Rarity.RARE: return 3;
            case ItemData.Rarity.EPIC: return 4;
            case ItemData.Rarity.LEGENDARY: return 5;
            default: return 0;
        }
    }
}
