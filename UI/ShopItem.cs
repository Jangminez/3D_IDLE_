using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    private ShopUI shopUI;
    public ItemInstance Item { get; private set; }

    [Header("UI Elements")]
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemCostText;
    private Button shopItemButton;

    public void Init(ShopUI shopUI, ItemInstance item)
    {
        this.shopUI = shopUI;
        Item = item;

        itemIcon.sprite = Item.ItemData.itemIcon;
        itemCostText.text = Item.ItemData.itemCost.ToString();

        if (TryGetComponent(out shopItemButton))
        {
            shopItemButton.interactable = !item.IsBuy;
            shopItemButton.onClick.AddListener(OpenBuyPopUp);
        }
    }

    private void OpenBuyPopUp()
    {
        if (Item == null) return;

        shopUI.OpenBuyItemPopUp(Item, this);
    }

    public void SetBuyButton()
    {
        shopItemButton.interactable = false;
    }
}
