using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image itemImage;

    private Inventory inventory;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public int slotIndex;
    private Vector2 defaultPos;
    private Player player;

    /*
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        inventory = GameObject.Find("Player").GetComponent<Inventory>();
        player = GameObject.Find("Player").GetComponent<Player>();
        defaultPos = rectTransform.anchoredPosition;
    }

    public void SetSlotIndex(int index)
    {
        slotIndex = index;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {   
        if (!inventory.isSlotEmpty(slotIndex))
        {
            player.tooltip.Show(inventory.items[slotIndex].ToString());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        player.tooltip.Hide();
    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject dropSlot = eventData.pointerCurrentRaycast.gameObject;
        if (dropSlot != null && dropSlot.CompareTag("InventorySlot"))
        {
            InventorySlot targetInventorySlot = dropSlot.GetComponent<InventorySlot>();
            inventory.SwapItems(slotIndex, targetInventorySlot.slotIndex);
        }
        rectTransform.anchoredPosition = defaultPos;
        canvasGroup.blocksRaycasts = true; 
        canvasGroup.alpha = 1f;
        inventory.UpdateUI();
    }
    */
}