using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilitySlot : MonoBehaviour, IPointerExitHandler, IPointerMoveHandler
{
    public int ability;
    private Player player;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        player.tooltip.Show(player.characterManager.GetCurrentCharacter().GetAbilityTooltip(ability));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        player.tooltip.Hide();
    }
}
