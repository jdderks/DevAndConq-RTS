using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AIWarFactoryDesireObject", menuName = "Custom/AIWarFactoryDesireScriptableObject")]
public class AIWarFactoryDesireObject : ScriptableObject
{
    protected AnimationCurve weightCurve = AnimationCurve.Linear(0, 0, 1, 1);

    public float CalculateDesire(int sampleSize, int minimumRequiredBulldozers = 0) //in this case minimumRequirement means the minimum amount of bulldozers
    {

        if (minimumRequiredBulldozers == 0)
        {
            return 0;
        }

        float normalizedSampleSize = Mathf.InverseLerp(0, weightCurve.length, sampleSize);
        float curveValue = weightCurve.Evaluate(normalizedSampleSize);

        return curveValue;
    }
}