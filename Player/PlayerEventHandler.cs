using System;

public class PlayerEventHandler
{
    public event Action<int> onGoldChanged;
    public event Action<int, int> onExpChanged;
    public event Action<int> onLevelChanged;

    public void RaiseGoldChanged(int gold) => onGoldChanged?.Invoke(gold);
    public void RaiseExpChanged(int currentExp, int requiredExp) => onExpChanged?.Invoke(currentExp, requiredExp);
    public void RaiseLevelChanged(int level) => onLevelChanged?.Invoke(level);
}
