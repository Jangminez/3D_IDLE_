using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    private InventoryUI inventoryUI;

    private ItemInstance equippedItem;
    public ItemInstance EquippedItem { get => equippedItem; }

    [SerializeField] EquipType slotType;
    public EquipType SlotType { get => slotType; }

    [Header("UI")]
    [SerializeField] Image icon;
    private Button slotBtn;

    public void Init(InventoryUI inventoryUI)
    {
        this.inventoryUI = inventoryUI;

        slotBtn = GetComponent<Button>();

        if (transform.Find("Icon").TryGetComponent(out icon))
        {
            icon.enabled = false;
        }

        slotBtn.onClick.AddListener(OpenDescription);
    }

    public void Equip(ItemInstance itemInstance)
    {
        if (itemInstance == null) return;

        equippedItem = itemInstance;
        equippedItem.EquipItem(true);

        icon.sprite = equippedItem.ItemData.itemIcon;
        icon.enabled = true;

        inventoryUI.onItemEquipped?.Invoke(itemInstance, true);
    }

    public void UnEquip()
    {
        inventoryUI.onItemEquipped?.Invoke(equippedItem, false);

        equippedItem.EquipItem(false);
        equippedItem = null;
        icon.enabled = false;
    }

    private void OpenDescription()
    {
        if (equippedItem == null) return;

        inventoryUI.SetEquipmentPopUp(equippedItem);
    }
}
