using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Personality Timer", menuName = "Personality Timer")]
public class PersonalityTimer : ScriptableObject
{
    public AnimationCurve modifierCurve;
    public AIPersonalityScriptableObject startPersonalityValues;
    public AIPersonalityScriptableObject endPersonalityValues;

    public float modifierStrength = 1;

    public float maxDuration = 10f;
    private float currentTime = 0f;

    public void OnUpdate(float dt)
    {
        LerpModifierValues();
    }

    public AIPersonalityScriptableObject LerpModifierValues()
    {
        currentTime += Time.deltaTime;
        float t = currentTime / maxDuration;

        AIPersonalityScriptableObject currentValues = new();

        currentValues.aggressivenessModifier = Mathf.Lerp(startPersonalityValues.aggressivenessModifier, endPersonalityValues.aggressivenessModifier, modifierCurve.Evaluate(t) * modifierStrength);
        currentValues.defensivenessModifier = Mathf.Lerp(startPersonalityValues.defensivenessModifier, endPersonalityValues.defensivenessModifier, modifierCurve.Evaluate(t) * modifierStrength);
        currentValues.overpowerednessModifier = Mathf.Lerp(startPersonalityValues.overpowerednessModifier, endPersonalityValues.overpowerednessModifier, modifierCurve.Evaluate(t) * modifierStrength);
        currentValues.constructionSpeedMultiplier = Mathf.Lerp(startPersonalityValues.constructionSpeedMultiplier, endPersonalityValues.constructionSpeedMultiplier, modifierCurve.Evaluate(t) * modifierStrength);

        return currentValues;
    }


}
