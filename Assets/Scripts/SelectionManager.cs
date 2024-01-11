using JetBrains.Annotations;
using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnitSelectionHelper;
using static UnityEngine.Rendering.DebugUI.MessageBox;

//CREDIT: Made following: https://youtu.be/OL1QgwaDsqo by Seabass under the open-source MIT License
/// <summary>
/// This script is responsable for all the unit-selection and building selection.
/// The script may need to split up into multiple other scripts in the future to adhere to the single-responsibility prinsiple
/// </summary>

public enum CursorInputState
{
    None = 0,
    HoldingLeft = 1,
    HoldingRight = 2
}


public class SelectionManager : Manager
{
    [SerializeField] private Camera cam;
    [SerializeField] private bool debugMouseState = true;

    [SerializeField] private GameObject selectionPrefab;
    [SerializeField] private GameObject unitMovementPrefab;
    public GameObject SelectionPrefab { get => selectionPrefab; set => selectionPrefab = value; }

    RaycastHit hit;
    bool dragSelect;
    bool lineInput;

    Vector2 lineSelectOrigin;

    public CursorInputState cursorState = CursorInputState.None;

    //Selection collider variables
    //=======================================================//

    MeshCollider selectionBox;
    Mesh selectionMesh;

    Vector3 lineSelectionPosition1 = new Vector3(); //Beginning of the line
    Vector3 lineSelectionPosition2 = new Vector3(); //End of the line

    Vector3 marqueePosition1;
    Vector3 marqueePosition2;

    //the corners of our 2d selection box
    Vector2[] corners;

    //the vertices of our meshcollider
    Vector3[] verts;
    Vector3[] vecs;


    // Start is called before the first frame update
    void Start()
    {
        dragSelect = false;
    }

    // Update is called once per frame
    void Update()
    {
        #region unit selection
        //1. when left mouse button clicked (but not released)
        if (GameManager.Instance.inputManager.GetMouseToSelectInputDown())
            if (!EventSystem.current.IsPointerOverGameObject())
                marqueePosition1 = Input.mousePosition;

        //2. while left mouse button held
        if (GameManager.Instance.inputManager.GetMouseToSelectInput()) //TODO: Make InputManager 
            if (!EventSystem.current.IsPointerOverGameObject())
                if ((marqueePosition1 - Input.mousePosition).magnitude > 40)
                    dragSelect = true;

        //3. when mouse button comes up
        if (GameManager.Instance.inputManager.GetMouseToSelectInputUp()) //TODO: Make InputManager 
        {
            if (dragSelect == false) //single select
            {
                Ray ray = cam.ScreenPointToRay(marqueePosition1);

                if (Physics.Raycast(ray, out hit, 50000.0f))
                {
                    if (Input.GetKey(KeyCode.LeftShift)) //inclusive select
                    {
                        GameManager.Instance.unitManager.AddSelected(hit.transform.gameObject);
                    }
                    else //exclusive selected
                    {
                        GameManager.Instance.unitManager.DeselectAll();
                        GameManager.Instance.unitManager.AddSelected(hit.transform.gameObject);

                        UpdatePanelWhenOneSelected();
                    }
                }
                else //if we didnt hit something
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        //do nothing
                    }
                    else
                    {
                        GameManager.Instance.unitManager.DeselectAll();
                    }
                }
            }
            else //marquee select
            {
                verts = new Vector3[4];
                vecs = new Vector3[4];
                int i = 0;
                marqueePosition2 = Input.mousePosition;
                corners = getBoundingBox(marqueePosition1, marqueePosition2);

                foreach (Vector2 corner in corners)
                {
                    Ray ray = cam.ScreenPointToRay(corner);

                    if (Physics.Raycast(ray, out hit, 50000.0f, (1 << 8)))
                    {
                        verts[i] = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                        vecs[i] = ray.origin - hit.point;
                        Debug.DrawLine(cam.ScreenToWorldPoint(corner), hit.point, Color.red, 1.0f);
                    }
                    i++;
                }

                //generate the mesh
                selectionMesh = CreateSelectionMesh(verts, vecs);

                selectionBox = gameObject.AddComponent<MeshCollider>();
                selectionBox.sharedMesh = selectionMesh;
                selectionBox.convex = true;
                selectionBox.isTrigger = true;

                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    GameManager.Instance.unitManager.DeselectAll();
                }

                Destroy(selectionBox, 0.02f);



            }//end marquee select

            dragSelect = false;

        }
        #endregion

        #region unit movement & unit interaction

        int groundMask = 1 << 8;

        //1. register start of line input
        if (GameManager.Instance.inputManager.GetMouseToMoveInputDown())
        {
            lineSelectOrigin = Input.mousePosition;
        }

        if (GameManager.Instance.inputManager.GetMouseToMoveInput())
        {
            //Check whether or not the mouse has moved more than pixels, if so the player is doing line input.
            if (Vector2.Distance(lineSelectOrigin, Input.mousePosition) > 10)
            {
                lineInput = true;
                //Debug line drawing here
            }
        }

        if (GameManager.Instance.inputManager.GetMouseToMoveInputUp())//(Input.GetMouseButtonUp(1))
        {
            var selectables = GameManager.Instance.SelectableCollection.selectedTable.Values;
            var units = new List<Unit>();

            foreach (var item in selectables)
            {
                if (item.GetGameObject().GetComponent<Unit>() != null)
                {
                    units.Add(item.GetGameObject().GetComponent<Unit>());
                }
            }

            if (lineInput == true)
            {
                Ray ray1 = cam.ScreenPointToRay(lineSelectOrigin);//Ray 1 of the line
                Ray ray2 = cam.ScreenPointToRay(Input.mousePosition); //Ray 2 of the line

                RaycastHit hit1;
                RaycastHit hit2;


                if (Physics.Raycast(ray1, out hit1, 5000f, groundMask))
                {
                    lineSelectionPosition1 = hit1.point;
                }
                if (Physics.Raycast(ray2, out hit2, 5000f, groundMask))
                {
                    lineSelectionPosition2 = hit2.point;
                }

                List<Vector3> movementPoints = PointGenerator.GeneratePointsInLine(lineSelectionPosition1, lineSelectionPosition2, units.Count).ToList();

                for (int i = 0; i < units.Count; i++)
                {
                    var instantiatedObject = Instantiate(unitMovementPrefab, movementPoints[i], Quaternion.identity);
                    Unit unit = units[i];
                    unit.StartTask(new MoveUnitTask(unit, movementPoints[i]));
                    Destroy(instantiatedObject, 0.4f);
                }
            }

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 50000.0f, groundMask))
            {
                if (lineInput == false)
                {
                    GiveCommandToUnits(units);
                }
            }
            lineInput = false;
        }

        #endregion
    }

    private void GiveCommandToUnits(List<Unit> units)
    {
        List<Vector3> movementPoints = PointGenerator.GenerateSunflowerPoints(hit.point, numberOfPoints: units.Count, radius: units.Count * 0.5f);

        for (int i = 0; i < units.Count; i++)
        {
            var instantiatedObject = Instantiate(unitMovementPrefab, movementPoints[i], Quaternion.identity);
            Unit unit = units[i];
            var task = new MoveUnitTask(unit, movementPoints[i]);
            unit.StartTask(task);
            Destroy(instantiatedObject, 0.4f);
        }
    }

    private static void UpdatePanelWhenOneSelected()
    {
        ISelectable singleSelectable = GameManager.Instance.SelectableCollection.GetSingleSelectable();
        GameManager.Instance.uiManager.UpdateRtsActionPanel(singleSelectable);

        if (singleSelectable is Building building)
        {
            if (building.actionQueue == null)
            {
                building.actionQueue = new();
                building.actionQueue.SetActions();
            }
            GameManager.Instance.uiManager.OpenActionQueuePanel(building.actionQueue);
        }
        else
        {
            GameManager.Instance.uiManager.CloseActionQueuePanel();
        }
    }

    private void OnGUI()
    {
        if (dragSelect == true)
        {
            var rect = GetScreenRect(marqueePosition1, Input.mousePosition);
            DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
        if (debugMouseState)
        {
            if (lineInput)
            {
                ShowTextBelowCursor("Drawing a line.");
            }
            else
            {
                ShowTextBelowCursor(cursorState.ToString());
            }
        }

    }

    private void OnDrawGizmos()
    {
        if (lineInput)
        {
            var p1 = lineSelectionPosition1;
            var p2 = lineSelectionPosition2;
            var thickness = 3;
            Handles.DrawBezier(p1, p2, p1, p2, Color.red, null, thickness);
        }
    }

    void ShowTextBelowCursor(string text, float textSize = 16f)
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = Mathf.RoundToInt(textSize);

        Vector3 mousePosition = Input.mousePosition;
        GUI.Label(new Rect(mousePosition.x, Screen.height - mousePosition.y, 200, 30), text, style);
    }

    //create a bounding box (4 corners in order) from the start and end mouse position
    Vector2[] getBoundingBox(Vector2 p1, Vector2 p2)
    {
        // Min and Max to get 2 corners of rectangle regardless of drag direction.
        var bottomLeft = Vector3.Min(p1, p2);
        var topRight = Vector3.Max(p1, p2);

        // 0 = top left; 1 = top right; 2 = bottom left; 3 = bottom right;
        Vector2[] corners =
        {
            new Vector2(bottomLeft.x, topRight.y),
            new Vector2(topRight.x, topRight.y),
            new Vector2(bottomLeft.x, bottomLeft.y),
            new Vector2(topRight.x, bottomLeft.y)
        };
        return corners;

    }

    //generate a mesh from the 4 bottom points
    Mesh CreateSelectionMesh(Vector3[] corners, Vector3[] vecs)
    {
        Vector3[] verts = new Vector3[8];
        int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 }; //map the tris of our frustum/pyramid

        for (int i = 0; i < 4; i++)
        {
            verts[i] = corners[i];
        }

        for (int j = 4; j < 8; j++)
        {
            verts[j] = corners[j - 4] + vecs[j - 4];
        }

        Mesh selectionMesh = new Mesh();
        selectionMesh.vertices = verts;
        selectionMesh.triangles = tris;

        return selectionMesh;
    }

    //Used to check for multiple units inside of the marquee collider   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Unit>())
        {
            GameManager.Instance.unitManager.AddSelected(other.gameObject);
            UpdatePanelWhenOneSelected();
        }
    }


    public GameObject IsBuildingNearby(Vector3 positionToCheck)
    {
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");

        foreach (GameObject building in buildings)
        {
            float distance = Vector3.Distance(positionToCheck, building.transform.position);
            if (distance <= 5)
            {
                return building;
            }
        }
        return null;
    }
}
