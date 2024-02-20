using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyDock : MonoBehaviour
{
    //Make sure the range of this slider is always the maximum amount of crates!
    [SerializeField, Range(0, 55)] private int amountOfCrates;

    [SerializeField] private List<GameObject> crates = new();


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

    public void AmountOfCratesChanged()
    {
        //enable or disable the crates gameobject at index amountofcrates in reverse order
        for (int i = 0; i < crates.Count; i++)
        {
            if (i < amountOfCrates)
            {
                crates[i].SetActive(true);
            }
            else
            {
                crates[i].SetActive(false);
            }
        }
    }
}
