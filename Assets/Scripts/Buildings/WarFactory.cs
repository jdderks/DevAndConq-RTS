using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WarFactory : Building
{
    CreateUnitRTSAction constructLightTankAction = new CreateUnitRTSAction();
    CreateUnitRTSAction constructHeavyTankAction = new CreateUnitRTSAction();

    [SerializeField] private PanelInfoScriptableObject lightTankUnitInfo;// = GameManager.Instance.Settings.rtsActionSettings.lightTankPanelInfo;
    [SerializeField] private PanelInfoScriptableObject heavyTankUnitInfo;// = GameManager.Instance.Settings.rtsActionSettings.lightTankPanelInfo;


    // Start is called before the first frame update
    void Start()
    {
        if (!isInstantiated) SetTeam(colourEnum);
        constructLightTankAction.PanelInfo = lightTankUnitInfo;
        constructHeavyTankAction.PanelInfo = heavyTankUnitInfo;

        GetActions().Add(constructLightTankAction);
        GetActions().Add(constructHeavyTankAction);

        constructLightTankAction.SetUnitValues(
            unitObject: lightTankUnitInfo.actionPrefab,
            originBuilding: this,
            team: ownedByTeam);

        constructHeavyTankAction.SetUnitValues(
            unitObject: heavyTankUnitInfo.actionPrefab,
            originBuilding: this,
            team: ownedByTeam);
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

        //mats[1] = OwnedByTeam.teamMaterial;
        renderer.materials = mats;



        //Renderer renderer = visualObject.GetComponent<Renderer>();
        //var mats = renderer.materials;
        //mats[1] = ownedByTeam.teamMaterial;
        //renderer.materials = mats;
        //foreach (var item in renderer.materials)
        //{
        //    Debug.Log(item);
        //}
        //renderer.materials[1] = ownedByTeam.teamMaterial;
    }

    public override bool UnitInteract(Unit unit) { return false; } //Left empty on purpose
}
