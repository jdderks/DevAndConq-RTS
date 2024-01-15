using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AIWarFactoryDesireObject", menuName = "Custom/AIWarFactoryDesireScriptableObject")]
public class AIWarFactoryDesireObject : AIDesireScriptableObject
{

    public override float CalculateDesire(int sampleSize, int minimumRequirement = 0) //in this case minimumRequirement means the minimum amount of bulldozers
    {

        if (minimumRequirement == 0)
        {
            return 0;
        }
        return base.CalculateDesire(sampleSize, 0);
    }
}