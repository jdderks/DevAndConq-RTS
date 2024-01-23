using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Personality Object", menuName = "Personality")]
public class AIPersonalityScriptableObject : ScriptableObject
{
    [SerializeField] private float aggressivenessModifier;
    [SerializeField] private float defensivenessModifier;
    [SerializeField] private float overpowerednessModifier;
    [SerializeField] private float constructionSpeedMultiplier;
}
