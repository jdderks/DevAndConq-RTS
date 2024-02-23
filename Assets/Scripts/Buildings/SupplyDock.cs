using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyDock : MonoBehaviour
{
    //Make sure the range of this slider is always the maximum amount of crates!
    [SerializeField] private int amountOfMoneyPerCrate = 500;
    
    [SerializeField, Range(0, 68)] private int amountOfCrates;

    [SerializeField] private List<GameObject> crates = new();

    [SerializeField] private Transform interactPosition;

    public Transform InteractPosition { get => interactPosition; set => interactPosition = value; }
    public int AmountOfMoneyPerCrate { get => amountOfMoneyPerCrate; set => amountOfMoneyPerCrate = value; }
    public int AmountOfCrates
    {
        get => amountOfCrates;
        set
        {
            amountOfCrates = value;
            AmountOfCratesChanged();
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {

    }

    private void OnValidate()
    {
        AmountOfCratesChanged();
    }

    [Button("Reverse crates list")]
    private void ReverseCrateslist()
    {
        crates.Reverse();
    }

    public void AmountOfCratesChanged()
    {
        //enable or disable the crates gameobject at index amountofcrates in reverse order
        for (int i = 0; i < crates.Count; i++)
        {
            if (i < AmountOfCrates)
            {
                crates[i].SetActive(true);
            }
            else
            {
                crates[i].SetActive(false);
            }
        }
    }

    public void RemoveSupplies(Unit gatheringUnit)
    {
        AmountOfCrates--;
    }
}
