using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WarFactory : Building
{
    CreateUnitRTSAction constructLightTankAction = new CreateUnitRTSAction();

    [SerializeField] private PanelInfoScriptableObject lightTankUnitInfo;// = GameManager.Instance.Settings.rtsActionSettings.lightTankPanelInfo;

    // Start is called before the first frame update
    void Start()
    {
        constructLightTankAction.PanelInfo = lightTankUnitInfo;
        GetActions().Add(constructLightTankAction);

        constructLightTankAction.SetUnitValues(
            unitObject: lightTankUnitInfo.actionPrefab,
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
}
