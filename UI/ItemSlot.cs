using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    private InventoryUI inventoryUI;
    public ItemInstance SlotItem { get; private set; }

    [Header("UI")]
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI equipText;
    [SerializeField] TextMeshProUGUI countText;
    private Button slotBtn;

    public void Init(InventoryUI inventoryUI)
    {
        this.inventoryUI = inventoryUI;

        OnEnable();
    }

    void OnEnable()
    {
        if (inventoryUI == null || SlotItem == null) return;

        SetSlotUI();
        inventoryUI.onItemEquipped += SetEquipState;
    }

    void OnDisable()
    {
        inventoryUI.onItemEquipped -= SetEquipState;
    }

    /// <summary>
    /// 장비 아이템 초기 세팅
    /// </summary>
    /// <param name="itemData"></param>
    public void SetSlot(ItemInstance itemInstance)
    {
        if (itemInstance == null) return;

        SlotItem = itemInstance;
        countText.enabled = false;
        equipText.enabled = false;
        icon.sprite = SlotItem.ItemData.itemIcon;

        if (TryGetComponent(out slotBtn))
        {
            slotBtn.onClick.AddListener(OpenDescription);
        }
    }

    /// <summary>
    /// 소모품 아이템 초기 세팅
    /// </summary>
    /// <param name="itemData"></param>
    /// <param name="itemCount"></param>
    public void SetSlot(ItemInstance itemInstance, int itemCount)
    {
        if (itemInstance == null) return;

        SlotItem = itemInstance;

        if (SlotItem.ItemData.isConsumable)
        {
            equipText.enabled = false;

            SlotItem.AddItem(itemCount);
            countText.text = itemCount.ToString();
            icon.sprite = SlotItem.ItemData.itemIcon;

            if (TryGetComponent(out slotBtn))
            {
                slotBtn.onClick.AddListener(OpenDescription);
            }
        }
    }

    /// <summary>
    /// 기존 아이템에 개수 추가
    /// </summary>
    /// <param name="itemData"></param>
    /// <param name="itemCount"></param>
    public void AddItem(ItemInstance itemInstance, int itemCount)
    {
        if (SlotItem.ItemData != itemInstance.ItemData) return;

        SlotItem.AddItem(itemCount);
        countText.text = itemCount.ToString();
    }

    public void SetEquipState(ItemInstance itemInstance, bool isEquipped)
    {
        if (SlotItem != itemInstance) return;

        equipText.enabled = isEquipped;
    }

    public void ConsumeItem()
    {
        if (SlotItem == null) return;

        if (SlotItem.UseItem())
        {
            Destroy(gameObject);
        }
        countText.text = SlotItem.Count.ToString();
    }

    private void OpenDescription()
    {
        if (SlotItem.ItemData.isConsumable)
            inventoryUI.SetConsumablePopUp(SlotItem, this);
        else
            inventoryUI.SetEquipmentPopUp(SlotItem);
    }

    private void SetSlotUI()
    {
        if (SlotItem.ItemData.isConsumable)
        {
            equipText.enabled = false;

            countText.text = SlotItem.Count.ToString();
            icon.sprite = SlotItem.ItemData.itemIcon;

            if (TryGetComponent(out slotBtn))
            {
                slotBtn.onClick.AddListener(OpenDescription);
            }
        }

        else
        {
            countText.enabled = false;
            equipText.enabled = SlotItem.IsEquip;
            icon.sprite = SlotItem.ItemData.itemIcon;
        }
    }
}
