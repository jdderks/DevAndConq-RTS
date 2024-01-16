using UnityEngine;

[CreateAssetMenu(fileName = "AIDesireScriptableObject", menuName = "Custom/AIDesireScriptableObject")]
public class AIDesireScriptableObject : ScriptableObject
{
    [SerializeField]
    protected AnimationCurve weightCurve = AnimationCurve.Linear(0, 0, 1, 1);

    public virtual float CalculateDesire(int sampleSize, int minimumRequirement = 0)
    {
        if (sampleSize < minimumRequirement)
        {
            return 1;
        }
        float normalizedSampleSize = Mathf.InverseLerp(0, weightCurve.length, sampleSize);
        float curveValue = weightCurve.Evaluate(normalizedSampleSize);

        return curveValue;
    }
}
