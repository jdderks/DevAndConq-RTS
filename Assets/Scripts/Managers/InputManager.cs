using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Manager
{
    public int mouseInputToSelect = 0;
    public int mouseInputToMoveCamera = 1;

    public KeyCode keyToOrbitCameraWihoutMouse = KeyCode.LeftAlt;
    public KeyCode keyToKeepSelectedUnits = KeyCode.LeftShift;

    #region key to keep selected units (default = leftshift)
    public bool GetKeepSelectedUnitsInputDown()
    {
        if (Input.GetKeyDown(keyToKeepSelectedUnits))
            return true;
        else
            return false;
    }
    public bool GetKeepSelectedUnitsInput()
    {
        if (Input.GetKey(keyToKeepSelectedUnits))
            return true;
        else
            return false;
    }
    public bool GetKeepSelectedUnitsInputUp()
    {
        if (Input.GetKeyUp(keyToKeepSelectedUnits))
            return true;
        else
            return false;
    }
    #endregion

    #region key to orbit camera without mouse (default = LeftAlt)
    public bool GetOrbitWithoutMouseInputDown()
    {
        if (Input.GetKeyDown(keyToOrbitCameraWihoutMouse))
            return true;
        else
            return false;
    }
    public bool GetOrbitWithoutMouseInput()
    {
        if (Input.GetKey(keyToOrbitCameraWihoutMouse))
            return true;
        else
            return false;
    }
    public bool GetUnitMovementInputUp()
    {
        if (Input.GetKeyUp(keyToOrbitCameraWihoutMouse))
            return true;
        else
            return false;
    }
    #endregion

    #region mouse input to select units (default = 0)
    public bool GetMouseToSelectInputDown()
    {
        if (Input.GetMouseButtonDown(mouseInputToSelect))
            return true;
        else
            return false;
    }
    public bool GetMouseToSelectInput()
    {
        if (Input.GetMouseButton(mouseInputToSelect))
            return true;
        else
            return false;
    }
    public bool GetMouseToSelectInputUp()
    {
        if (Input.GetMouseButtonUp(mouseInputToSelect))
            return true;
        else
            return false;
    }
    #endregion

    #region mouse input to move camera (default = 1)
    public bool GetMouseToMoveInputDown()
    {
        if (Input.GetMouseButtonDown(mouseInputToMoveCamera))
            return true;
        else
            return false;
    }
    public bool GetMouseToMoveInput()
    {
        if (Input.GetMouseButton(mouseInputToMoveCamera))
            return true;
        else
            return false;
    }
    public bool GetMouseToMoveInputUp()
    {
        if (Input.GetMouseButtonUp(mouseInputToMoveCamera))
            return true;
        else
            return false;
    }
    #endregion
}
