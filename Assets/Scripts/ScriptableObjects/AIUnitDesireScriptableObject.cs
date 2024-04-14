using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIUnitDesireScriptableObject", menuName = "Desire/AIUnitDesireScriptableObject")]

public class AIUnitDesireScriptableObject : AIDesireScriptableObject
{
    public override float CalculateDesire(int sampleSize, int minimumRequirement = 0)
    {
        if (sampleSize > 0)
        {
            if (minimumRequirement != 0)
            {
                return Mathf.Abs(sampleSize / minimumRequirement - 1);
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return 0;
        }




        
        //return base.CalculateDesire(sampleSize, minimumRequirement);
    }
}
