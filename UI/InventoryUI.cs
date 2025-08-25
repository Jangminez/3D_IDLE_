using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private Player player;

    [SerializeField] Transform itemSlotParent;
    [SerializeField] GameObject itemSlotPrefab;
    [SerializeField] EquipmentSlot[] equipmentSlots;
    private Dictionary<EquipType, EquipmentSlot> equipmentSlotDict;
    private List<ItemSlot> itemSlots = new List<ItemSlot>();

    [Header("UI PopUp")]
    [SerializeField] GameObject equipmentPopUp;
    [SerializeField] GameObject consumablePopUp;

    [Header("Equipment PopUp")]
    [SerializeField] TextMeshProUGUI equipNameText;
    [SerializeField] TextMeshProUGUI equipDescText;
    [SerializeField] TextMeshProUGUI equipButtonText;
    [SerializeField] TextMeshProUGUI upgradeCostText;
    [SerializeField] Button upgradeButton;
    [SerializeField] Button equipButton;
    [SerializeField] Button equipExitButton;

    [Header("Consumable PopUp")]
    [SerializeField] TextMeshProUGUI consumableNameText;
    [SerializeField] TextMeshProUGUI consumableDescText;
    [SerializeField] Button useButton;
    [SerializeField] Button consumableExitButton;

    private ItemInstance selectedItem;
    private ItemSlot selectedSlot;

    public Action<ItemInstance, bool> onItemEquipped;

    public void Init(Player player)
    {
        this.player = player;

        equipExitButton.onClick.AddListener(ClosePopUp);
        consumableExitButton.onClick.AddListener(ClosePopUp);

        equipmentSlotDict = new Dictionary<EquipType, EquipmentSlot>();
        itemSlots = new List<ItemSlot>();

        foreach (var slot in equipmentSlots)
        {
            slot.Init(this);
            equipmentSlotDict.Add(slot.SlotType, slot);
        }
    }

    private void OnEnable()
    {
        ClosePopUp();
    }

    public void AddItem(ItemInstance itemInstance)
    {
        GameObject slot = Instantiate(itemSlotPrefab, itemSlotParent);

        if (slot.TryGetComponent(out ItemSlot itemSlot))
        {
            itemSlot.Init(this);
            itemSlot.SetSlot(itemInstance);

            itemSlots.Add(itemSlot);
        }
    }

    public void AddItem(ItemInstance itemInstance, int count)
    {
        ItemSlot tempSlot = itemSlots.FirstOrDefault(i => i.SlotItem.ItemData == itemInstance.ItemData);

        if (tempSlot != null)
        {
            tempSlot.AddItem(itemInstance, count);
        }

        else
        {
            GameObject slot = Instantiate(itemSlotPrefab, itemSlotParent);

            if (slot.TryGetComponent(out ItemSlot itemSlot))
            {
                itemSlot.Init(this);
                itemSlot.SetSlot(itemInstance, count);

                itemSlots.Add(itemSlot);
            }
        }
    }

    public void EquipItem()
    {
        if (selectedItem == null) return;

        EquipmentSlot slot = equipmentSlotDict[selectedItem.ItemData.equipType];

        if (slot.EquippedItem != null)
            UnEquipItem();

        slot.Equip(selectedItem);

        player.CalculateBonusStat(GetEquippedItems());

        SetEquipmentPopUp(selectedItem);
    }

    public void UnEquipItem()
    {
        if (selectedItem == null) return;

        EquipmentSlot slot = equipmentSlotDict[selectedItem.ItemData.equipType];
        slot.UnEquip();

        player.CalculateBonusStat(GetEquippedItems());

        SetEquipmentPopUp(selectedItem);
    }

    public void SetEquipmentPopUp(ItemInstance itemInstance)
    {
        if (itemInstance == null) return;
        equipButton.onClick.RemoveAllListeners();

        selectedItem = itemInstance;

        equipNameText.text = selectedItem.ItemData.itemName;
        equipDescText.text = selectedItem.GetFormattedDescription();
        upgradeCostText.text = selectedItem.UpgradeInfo.cost.ToString();

        if (selectedItem.IsEquip)
        {
            equipButtonText.text = "UnEquip";
            equipButton.onClick.AddListener(UnEquipItem);
        }
        else
        {
            equipButtonText.text = "Equip";
            equipButton.onClick.AddListener(EquipItem);
        }

        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(UpgradeItem);

        equipmentPopUp.SetActive(true);
    }

    public void UpgradeItem()
    {
        if (player.UseGold(selectedItem.UpgradeInfo.cost))
        {
            selectedItem.UpgradeInfo.Upgrade();
            SetEquipmentPopUp(selectedItem);

            if (selectedItem.IsEquip)
                player.CalculateBonusStat(GetEquippedItems());
        }
    }

    private void ClosePopUp()
    {
        equipmentPopUp.SetActive(false);
        consumablePopUp.SetActive(false);
    }

    public List<ItemInstance> GetEquippedItems()
    {
        List<ItemInstance> equippedItems = new List<ItemInstance>();

        foreach (var slot in equipmentSlots)
        {
            if (slot.EquippedItem != null)
                equippedItems.Add(slot.EquippedItem);
        }

        return equippedItems;
    }

    public void SetConsumablePopUp(ItemInstance itemInstance, ItemSlot itemSlot)
    {
        if (itemInstance == null || itemSlot == null) return;

        selectedItem = itemInstance;
        selectedSlot = itemSlot;

        consumableNameText.text = selectedItem.ItemData.itemName;
        consumableDescText.text = selectedItem.GetFormattedDescription();

        useButton.onClick.RemoveAllListeners();
        useButton.onClick.AddListener(UseItem);

        consumablePopUp.SetActive(true);
    }

    private void UseItem()
    {
        selectedSlot.ConsumeItem();
        player.ApplyItemEffect(selectedItem);
    }
}
