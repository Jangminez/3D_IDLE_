using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    private UIManager uIManager;
    private DataManager dataManager;
    private Player player;
    private InventoryUI inventoryUI;

    [SerializeField] GameObject shopItemPrefab;
    [SerializeField] Transform shopItemContent;

    [Header("Buy Item PopUp")]
    [SerializeField] GameObject buyItemPopUp;
    [SerializeField] TextMeshProUGUI buyItemName;
    [SerializeField] TextMeshProUGUI buyItemDescription;
    [SerializeField] TextMeshProUGUI buyItemCost;

    [SerializeField] Button buyButton;
    [SerializeField] Button exitButton;

    private ItemInstance selectedItem;
    private ShopItem shopItem;

    public void Init(UIManager uIManager, DataManager dataManager)
    {
        this.uIManager = uIManager;
        this.dataManager = dataManager;
        player = uIManager.Player;
        inventoryUI = uIManager.InventoryUI;

        exitButton.onClick.AddListener(CloseBuyItemPopUp);
        SetShopItem();
    }

    void OnEnable()
    {
        CloseBuyItemPopUp();
    }

    private void SetShopItem()
    {
        Dictionary<int, ItemData> itemDict = dataManager.GetItemDict();

        foreach (var key in itemDict.Keys)
        {
            ShopItem shopItem = Instantiate(shopItemPrefab, shopItemContent).GetComponent<ShopItem>();
            shopItem.Init(this, dataManager.GetItemByKey(key));
        }
    }

    private void CloseBuyItemPopUp()
    {
        buyItemPopUp.SetActive(false);

        selectedItem = null;
        shopItem = null;
    }

    public void OpenBuyItemPopUp(ItemInstance item, ShopItem shopItem)
    {
        selectedItem = item;
        this.shopItem = shopItem;

        buyItemName.text = item.ItemData.itemName;
        buyItemDescription.text = item.GetFormattedDescription();
        buyItemCost.text = item.ItemData.itemCost.ToString();

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyItem);

        buyItemPopUp.SetActive(true);
    }

    private void BuyItem()
    {
        if (player.UseGold(selectedItem.ItemData.itemCost))
        {

            if (selectedItem.ItemData.isConsumable)
            {
                inventoryUI.AddItem(selectedItem, 1);
            }
            else
            {
                inventoryUI.AddItem(selectedItem);
                selectedItem.BuyItem();
                shopItem.SetBuyButton();
            }
        }
    }
}
