using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "New Personality Object", menuName = "Personality")]
public class AIPersonalityScriptableObject : ScriptableObject
{
    [Header("Fair Modifiers")]
    public float aggressivenessModifier;
    public float defensivenessModifier;
    [Header("Unfair Modifiers")]
    public float overpowerednessModifier;
    public float constructionSpeedMultiplier;
}
