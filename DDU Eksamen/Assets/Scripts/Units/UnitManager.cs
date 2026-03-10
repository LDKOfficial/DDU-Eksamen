using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField]
    private HexGrid hexGrid;

    [SerializeField]
    private MovementSystem movementSystem;

    public bool canMove { get; private set; } = true;

    [SerializeField]
    private Unit selectedUnit;
    private Hex previouslySelectedHex;

    public void HandleUnitSelected(GameObject unit)
    {
        if (canMove == false)
            return; // Does smth with turns might wanna look into it when done

        Unit unitReference = unit.GetComponent<Unit>();

        //Debug.Log(unitReference.ToString());

        if (CheckIfSameUnitSelected(unitReference))
            return;

        PrepareUnitForMovement(unitReference);
    }

    private bool CheckIfSameUnitSelected(Unit unitReference)
    {
        if (selectedUnit == unitReference)
        {
            ClearOldSelection();
            return true;
        }
        return false;
    }

    public void HandleTerrainSelected(GameObject hexGo)
    {
        if (selectedUnit == null || canMove == false)
            return;

        Hex selectedHex = hexGo.GetComponent<Hex>();

        if (HandleHexOutOfRange(selectedHex.HexCoords) || HandleSelectedHexIsUnitHex(selectedHex.HexCoords))
            return;

        HandleTargetHexSelected(selectedHex);
    }

    private void PrepareUnitForMovement(Unit unitReference)
    {
        if (selectedUnit != null)
        {
            Debug.Log("Clear old");
            ClearOldSelection();
        }

        selectedUnit = unitReference; //dunno why this.selectedUnit and not selectedUnit
        selectedUnit.highlight.ToggleSelectedHighlight(true);
        movementSystem.ShowRange(selectedUnit, hexGrid);
    }

    private void ClearOldSelection()
    {
        previouslySelectedHex = null;
        selectedUnit.highlight.ToggleSelectedHighlight(false);
        movementSystem.HideRange(hexGrid);
        selectedUnit = null;
    }

    private void HandleTargetHexSelected(Hex selectedHex)
    {
        if (previouslySelectedHex == null || previouslySelectedHex != selectedHex)
        {
            previouslySelectedHex = selectedHex;
            movementSystem.ShowPath(selectedHex.HexCoords, hexGrid);
        }
        else
        {
            movementSystem.MoveUnit(selectedUnit, hexGrid);
            canMove = false; // Might wanna change this messes with turns
            selectedUnit.MovementFinished += ResetTurn; // Might wanna change this messes with turns
            // turn might just be to disable player doing stuff while we be movin
            ClearOldSelection();
        }
    }

    private bool HandleSelectedHexIsUnitHex(Vector3Int hexPosition)
    {
        if (hexPosition == hexGrid.GetClosestHex(selectedUnit.transform.position))
        {
            selectedUnit.highlight.ToggleSelectedHighlight(false);
            ClearOldSelection();
            return true;
        }
        return false;
    }

    private bool HandleHexOutOfRange(Vector3Int hexPosition)
    {
        if (movementSystem.IsHexInRange(hexPosition) == false)
        {
            Debug.Log("Hex Out of range!");
            return true;
        }
        return false;
    }

    private void ResetTurn(Unit selectedUnit) // bad name, doesnt interact with turn system
    {
        selectedUnit.MovementFinished -= ResetTurn;
        canMove = true;
    }

}
