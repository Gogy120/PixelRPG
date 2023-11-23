using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearDatabase : MonoBehaviour
{
    public static List<Gear> gear = new List<Gear>();
    private static Color32 commonColor = new Color32(255,255,255,255);
    private static Color32 uncommonColor = new Color32(145, 219, 105, 255);
    private static Color32 rareColor = new Color32(77, 155, 230, 255);
    private static Color32 epicColor = new Color32(144, 94, 169, 255);
    private static Color32 legendaryColor = new Color32(249, 194, 43, 255);

    void Awake()
    {
        BuildDatabase();
    }

    public static Gear GetGear(string name)
    {
        BuildDatabase();
        return gear.Find(gear => gear.name == name);
    }

    public static Gear GetRandomGear()
    {
        BuildDatabase();
        return gear[Random.Range(0, gear.Count)];
    }

    public static List<Gear> GetAllGear()
    {
        BuildDatabase();
        return gear;
    }

    public static List<Gear> GetGearBySlot(string slot)
    {
        List<Gear> gearBySlot = new List<Gear>();
        foreach (Gear g in GetAllGear())
        {
            if (g.slotType.ToString() == slot) { gearBySlot.Add(g); }
        }
        return gearBySlot;
    }

    static void BuildDatabase()
    {
        gear = new List<Gear>()
        {
            new Gear("No Head","",Gear.SlotType.Head,0,0,0,new int[3]{ 0,0,0},commonColor),
            new Gear("No Chest","",Gear.SlotType.Chest,0,0,0,new int[3]{ 0,0,0},commonColor),
            new Gear("No Legs","",Gear.SlotType.Legs,0,0,0,new int[3]{ 0,0,0},commonColor),
            new Gear("Vagabond's Cap","",Gear.SlotType.Head,2,8,0,new int[3]{ 0,0,0},commonColor),
            new Gear("Vagabond's Vest","Old vest of a stinky beggar",Gear.SlotType.Chest,1,12,0,new int[3]{ 0,0,0},commonColor),
            new Gear("Vagabond's Trousers","",Gear.SlotType.Legs,1,5,50,new int[3]{ 0,0,0},commonColor),
            new Gear("Straw Hat","",Gear.SlotType.Head,1,5,50,new int[3]{ 0,0,0},uncommonColor),
            new Gear("Blue Flannel Shirt","",Gear.SlotType.Chest,1,5,50,new int[3]{ 0,0,0},uncommonColor),
            new Gear("Green Shorts","",Gear.SlotType.Legs,1,5,50,new int[3]{ 0,0,0},uncommonColor),
            new Gear("Raincoat","",Gear.SlotType.Chest,1,5,50,new int[3]{ 0,0,0},rareColor),
            new Gear("Blue Cap","",Gear.SlotType.Head,1,5,50,new int[3]{ 0,0,0},uncommonColor),
        };
    }
}
