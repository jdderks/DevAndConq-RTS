using UnityEngine;

public class Economy : MonoBehaviour
{
    public int startingMoney = 10000;
    public int currentAmountOfMoney = -1;

    private void Start()
    {
        currentAmountOfMoney = startingMoney;
    }

    public void IncreaseMoney(int amount)
    {
        currentAmountOfMoney += amount;
    }

    public bool DecreaseMoney(int amount)
    {
        if (currentAmountOfMoney - amount < 0)
        {
            return false;
        }
        currentAmountOfMoney -= amount;
        return true;
    }
}
