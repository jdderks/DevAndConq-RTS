using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AIWarFactoryDesireObject", menuName = "Desire/AIWarFactoryDesireScriptableObject")]
public class AIWarFactoryDesireObject : ScriptableObject
{
    [SerializeField] protected AnimationCurve weightCurve = AnimationCurve.Linear(0, 0, 1, 1);

    public float CalculateDesire(int sampleSize, int dozerAmount = 0) //in this case minimumRequirement means the minimum amount of bulldozers
    {
        Debug.Log(sampleSize + "  aa  " + dozerAmount);
        if (dozerAmount == 0) return 0;//this requires at least one dozer to exist

        float normalizedSampleSize = Mathf.InverseLerp(0, weightCurve.length, sampleSize);
        float curveValue = weightCurve.Evaluate(normalizedSampleSize);

        return curveValue;

    }
}