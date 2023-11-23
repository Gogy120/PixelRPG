using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDropManager : MonoBehaviour
{
    [Header("Variables")]
    public List<string> loot;
    public int dropChance;
    [Header("Components")]
    public GameObject droppedLoot;
    void Start()
    {
        
    }

    public void DropLoot(Vector2 pos)
    {
        if (loot.Count > 0 && Random.Range(0,dropChance) == 0)
        {
            string selectedLoot = loot[Random.Range(0, loot.Count)];
            GameObject dl = Instantiate(droppedLoot, pos, Quaternion.identity);
            dl.GetComponent<DroppedLoot>().lootName = selectedLoot;
            dl.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Items/" + selectedLoot);
        }
    }

    
}
