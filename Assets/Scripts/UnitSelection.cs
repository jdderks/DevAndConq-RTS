using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitSelectionHelper;

//Made following: https://youtu.be/OL1QgwaDsqo by Seabass under the open-source MIT License
/// <summary>
/// This script is responsable for all the unit-selection and movement commands of units.
/// The script may need to split up into multiple other scripts in the future to adhere to the single-responsibility prinsiple
/// </summary>
public class UnitSelection : MonoBehaviour
{
    [SerializeField] private Camera cam;


    RaycastHit hit;
    bool dragSelect;

    //Selection collider variables
    //=======================================================//

    MeshCollider selectionBox;
    Mesh selectionMesh;

    Vector3 p1;
    Vector3 p2;

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
        if (Input.GetMouseButtonDown(0)) //TODO: Make InputManager 
        {
            p1 = Input.mousePosition;
        }

        //2. while left mouse button held
        if (Input.GetMouseButton(0)) //TODO: Make InputManager 
        {
            if ((p1 - Input.mousePosition).magnitude > 40)
            {
                dragSelect = true;
            }
        }

        //3. when mouse button comes up
        if (Input.GetMouseButtonUp(0)) //TODO: Make InputManager 
        {
            if (dragSelect == false) //single select
            {
                Ray ray = cam.ScreenPointToRay(p1);

                if (Physics.Raycast(ray, out hit, 50000.0f))
                {
                    if (Input.GetKey(KeyCode.LeftShift)) //inclusive select
                    {
                        GameManager.Instance.unitManager.AddSelected(hit.transform.gameObject);//selectableCollection.addSelected(hit.transform.gameObject);
                    }
                    else //exclusive selected
                    {
                        GameManager.Instance.unitManager.DeselectAll();
                        GameManager.Instance.unitManager.AddSelected(hit.transform.gameObject);//selectableCollection.addSelected(hit.transform.gameObject);
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
                p2 = Input.mousePosition;
                corners = getBoundingBox(p1, p2);

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
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            int groundMask = 1 << 8;
            if (Physics.Raycast(ray, out hit, 50000.0f, groundMask))
            {
                var instantiatedObject = Instantiate(GameManager.Instance.Settings.ModelSettings.terrainInteractionObject, hit.point, Quaternion.identity);
                foreach (ISelectable item in GameManager.Instance.SelectableCollection.selectedTable.Values)
                {
                    if (item.GetGameObject().GetComponent<Unit>() != null)
                    {
                        var unit = item.GetGameObject().GetComponent<Unit>();
                        unit.StartTask(new MoveUnitTask(unit, hit.point));
                    }
                }
                Destroy(instantiatedObject, 0.4f);
            }
        }

        #endregion
    }

    private void OnGUI()
    {
        if (dragSelect == true)
        {
            var rect = GetScreenRect(p1, Input.mousePosition);
            DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
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
        }
    }
}
