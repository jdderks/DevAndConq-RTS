using UnityEngine;

public class Economy : MonoBehaviour
{
    [SerializeField] private int startingMoney = 10000;
    [SerializeField] private int currentAmountOfMoney = -1;

    private void Start()
    {
        currentAmountOfMoney = startingMoney;
    }

    public bool CanAffordAction(RtsAction action)
    {
        var actionCost = action.GetPanelInfo().cost;
        return currentAmountOfMoney - actionCost >= 0;
    }

    public void IncreaseMoney(int amount)
    {
        currentAmountOfMoney += amount;
    }

    public bool DecreaseMoney(int amount)
    {
        if (currentAmountOfMoney - amount < 0) return false;

        currentAmountOfMoney -= amount;
        Debug.Log($"Money decreased by {amount.ToString()}, new amount: {currentAmountOfMoney}");
        return true;
    }
}
