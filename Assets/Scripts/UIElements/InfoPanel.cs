using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    [Header("Descriptors")]
    [SerializeField] private TextMeshProUGUI infoPanelHeaderDescriptor;
    [SerializeField] private TextMeshProUGUI amountOfUnitsSelectedDescriptor;
    [Space]
    [Header("Values")]
    [SerializeField] private TextMeshProUGUI amountOfUnitsSelectedValue;

    public TextMeshProUGUI InfoPanelHeaderDescriptor { get => infoPanelHeaderDescriptor; set => infoPanelHeaderDescriptor = value; }
    public TextMeshProUGUI AmountOfUnitsSelectedDescriptor { get => amountOfUnitsSelectedDescriptor; set => amountOfUnitsSelectedDescriptor = value; }
}