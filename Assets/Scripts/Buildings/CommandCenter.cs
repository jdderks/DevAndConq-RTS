using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;
using UnityEngine;

public class CommandCenter : Building
{
    CreateUnitRTSAction constructDozerAction = new CreateUnitRTSAction();

    [SerializeField] private PanelInfoScriptableObject dozerUnitInfo;// = GameManager.Instance.Settings.rtsActionSettings.bullDozerPanelInfo;
    //ActionQueue ActionQueue = new();

    private void Start()
    {
        SetTeam(teamByColour);

        constructDozerAction.PanelInfo = dozerUnitInfo;

        GetActions().Add(constructDozerAction);

        constructDozerAction.SetUnitValues(
            unitObject: dozerUnitInfo.actionPrefab,
            originBuilding: this,
            team: ownedByTeam);
    }

    public override GameObject GetGameObject()
    {
        if (!this)
            return null;
        else
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

    public override void Deselect()
    {
        Destroy(instantiatedSelectionObject);
    }

    public override void SetTeam(TeamByColour teamByColour)
    {
        ownedByTeam = GameManager.Instance.teamManager.GetTeamByColour(teamByColour);
        Renderer renderer = visualObject.GetComponentInChildren<Renderer>();
        var mats = renderer.materials;
        mats[1] = ownedByTeam.teamMaterial;
        renderer.materials = mats;
    }
}
