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

        if (CheckIfSameUnitSelected(unitReference))
            return;

        PrepareUnitForMovement(unitReference);
    }

    private bool CheckIfSameUnitSelected(Unit unitReference)
    {
        if (this.selectedUnit == unitReference)
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
        if (this.selectedUnit != null)
        {
            ClearOldSelection();
        }

        this.selectedUnit = unitReference; //dunno why this.selectedUnit and not selectedUnit
        this.selectedUnit.highlight.ToggleSelectedHighlight(true);
        movementSystem.ShowRange(this.selectedUnit, this.hexGrid);
    }

    private void ClearOldSelection()
    {
        previouslySelectedHex = null;
        this.selectedUnit.highlight.ToggleSelectedHighlight(false);
        movementSystem.HideRange(this.hexGrid);
        this.selectedUnit = null;
    }

    private void HandleTargetHexSelected(Hex selectedHex)
    {
        if (previouslySelectedHex == null || previouslySelectedHex != selectedHex)
        {
            previouslySelectedHex = selectedHex;
            movementSystem.ShowPath(selectedHex.HexCoords, this.hexGrid);
        }
        else
        {
            movementSystem.MoveUnit(selectedUnit, this.hexGrid);
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
