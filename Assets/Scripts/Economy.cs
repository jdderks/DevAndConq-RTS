using UnityEngine;

public class Economy : MonoBehaviour
{
    [SerializeField] private int startingMoney = 10000;
    [SerializeField] private int currentAmountOfMoney = -1;

    public int CurrentAmountOfMoney
    {
        get => currentAmountOfMoney;
        set
        {
            currentAmountOfMoney = value;
            GameManager.Instance.uiManager.UpdateEconomyUI();
        }
    }

    private void Start()
    {
        CurrentAmountOfMoney = startingMoney;
    }

    public bool CanAffordAction(RtsAction action)
    {
        var actionCost = action.GetPanelInfo().cost;
        return CurrentAmountOfMoney - actionCost >= 0;
    }

    public void IncreaseMoney(int amount)
    {
        CurrentAmountOfMoney += amount;
    }

    public bool DecreaseMoney(int amount)
    {
        if (CurrentAmountOfMoney - amount < 0) return false;

        CurrentAmountOfMoney -= amount;
        Debug.Log($"Money decreased by {amount.ToString()}, new amount: {CurrentAmountOfMoney}");
        return true;
    }
}
