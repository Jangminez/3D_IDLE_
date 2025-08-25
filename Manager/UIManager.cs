using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameManager gameManager;

    public Player Player { get; private set; }
    public GameUI GameUI { get; private set; }
    public InventoryUI InventoryUI { get; private set; }
    public ShopUI ShopUI { get; private set; }
    public StageUI StageUI { get; private set; }
    public StatusUI StatusUI { get; private set; }

    [Header("Tab Buttons")]
    [SerializeField] Button inventoryButton;
    [SerializeField] Button stageButton;
    [SerializeField] Button shopButton;
    [SerializeField] Button statusButton;

    List<GameObject> togglePanels = new List<GameObject>();

    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
        Player = gameManager.Player;

        GameUI = GetComponentInChildren<GameUI>();
        InventoryUI = GetComponentInChildren<InventoryUI>();
        ShopUI = GetComponentInChildren<ShopUI>();
        StageUI = GetComponentInChildren<StageUI>();
        StatusUI = GetComponentInChildren<StatusUI>();

        if (GameUI)
            GameUI.Init(gameManager);

        if (InventoryUI)
        {
            InventoryUI.Init(Player);
            togglePanels.Add(InventoryUI.gameObject);
            inventoryButton.onClick.AddListener(() => TogglePanel(InventoryUI.gameObject));
        }

        if (ShopUI)
        {
            ShopUI.Init(this, gameManager.DataManager);
            togglePanels.Add(ShopUI.gameObject);
            shopButton.onClick.AddListener(() => TogglePanel(ShopUI.gameObject));
        }

        if (StageUI)
        {
            StageUI.Init(gameManager);
            togglePanels.Add(StageUI.gameObject);
            stageButton.onClick.AddListener(() => TogglePanel(StageUI.gameObject));
        }
        
        if (StatusUI)
        {
            StatusUI.Init(Player);
            togglePanels.Add(StatusUI.gameObject);
            statusButton.onClick.AddListener(() => TogglePanel(StatusUI.gameObject));
        }

        InitPanel();
    }

    private void InitPanel()
    {
        foreach (var panel in togglePanels)
        {
            panel.SetActive(false);
        }
    }

    private void TogglePanel(GameObject panel)
    {
        bool isActive = panel.activeSelf;

        foreach (var p in togglePanels)
        {
            p.SetActive(false);
        }

        panel.SetActive(!isActive);
    }
}
