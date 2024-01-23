using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Personality Object", menuName = "Personality")]
public class AIPersonalityScriptableObject : ScriptableObject
{
    public float aggressivenessModifier;
    public float defensivenessModifier;
    public float overpowerednessModifier;
    public float constructionSpeedMultiplier;
}
