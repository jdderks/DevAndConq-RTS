using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SupplyCenter : Building
{
    CreateUnitRTSAction constructSupplyTruck = new CreateUnitRTSAction();
    [SerializeField] private PanelInfoScriptableObject supplyTruckUnitInfo;// = GameManager.Instance.Settings.rtsActionSettings.lightTankPanelInfo;
    [SerializeField] private Transform interactPosition;

    public Transform InteractPosition { get => interactPosition; set => interactPosition = value; }

    public void Start()
    {
        if (!isInstantiated) SetTeam(colourEnum);

        constructSupplyTruck.PanelInfo = supplyTruckUnitInfo;
        GetActions().Add(constructSupplyTruck);

        constructSupplyTruck.SetUnitValues(
            unitObject: supplyTruckUnitInfo.actionPrefab,
            originBuilding: this,
            team: ownedByTeam);

        //Spawns a supplytruck as soon as the building is done :)
        OnConstructionFinished += ConstructSupplyTruckOnSpawn;

        void ConstructSupplyTruckOnSpawn()
        {
            constructSupplyTruck.Activate();
            OnConstructionFinished -= ConstructSupplyTruckOnSpawn;
        }
    }

    public override void Deselect()
    {
        Destroy(instantiatedSelectionObject);
    }

    public override GameObject GetGameObject()
    {
        return gameObject;
    }

    public override void Select()
    {
        Assert.IsNotNull(selectableHighlightParent, "Parent object not set in prefab.");
        instantiatedSelectionObject = Instantiate(
            GameManager.Instance.selectionManager.SelectionPrefab,
            selectableHighlightParent
            );
    }

    public override void SetTeam(TeamColour teamByColour)
    {
        ownedByTeam = GameManager.Instance.teamManager.GetTeamByColour(teamByColour);

        GameObject item = visualObject;
        Renderer renderer = item.GetComponent<Renderer>();
        var mats = renderer.materials;
        for (int j = 0; j < mats.Length; j++)
        {
            if (mats[j].name.Contains("Team_color"))
                mats[j] = ownedByTeam.teamMaterial;
        }

        renderer.materials = mats;
    }

    public override bool UnitInteract(Unit unit)
    {
        if (unit is not SupplyTruck) return false;

        SupplyTruck supplyTruck = unit as SupplyTruck;
        if (supplyTruck.HasLoad)
        {
            var economy = GameManager.Instance.economyManager.GetEconomy(supplyTruck.OwnedByTeam.teamByColour);
            if (economy != null)
            {

                economy.IncreaseMoney(500);
                supplyTruck.HasLoad = false;
            }
            return true;
        }
        return false;
    }

}
