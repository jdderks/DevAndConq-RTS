using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AI Supply Center Desire Object", menuName = "Desire/SupplyDesireObject")]
public class AISupplyResourceDesireScriptableObject : ScriptableObject
{
    public float CalculateSupplyCenterDesire(int amountOfSupplyCenters)
    {
        if (amountOfSupplyCenters < 1)
        {
            return 1f;
        }
        return 0f;
    }

    public float CalculateSupplyTruckDesire(int supplyTruckAmount)
    {
        switch (supplyTruckAmount)
        {
            case 0:
                return 1f;
            case 1:
                return 0.5f;
            case 2:
                return 0.25f;
            default:
                return 0f;
        }
    }
}
