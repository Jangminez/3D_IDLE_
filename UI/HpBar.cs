using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    UnitBase unit;
    Camera cam;

    [SerializeField] private Transform hpCanvas;
    [SerializeField] private Image hpBar;

    public void Init(UnitBase unit)
    {
        this.unit = unit;
        cam = Camera.main;

        OnEnable();
    }

    void Update()
    {
        if(cam == null || unit == null) return;

        hpCanvas.LookAt(cam.transform);    
    }

    void OnEnable()
    {
        if (unit == null) return;

        unit.onHealthChanged += UpdateHpBar;
    }

    void OnDisable()
    {
        if (unit == null) return;
        
        unit.onHealthChanged -= UpdateHpBar;
    }

    void UpdateHpBar(float currentHealth, float maxHealth)
    {
        hpBar.fillAmount = currentHealth / maxHealth;
    }
}
