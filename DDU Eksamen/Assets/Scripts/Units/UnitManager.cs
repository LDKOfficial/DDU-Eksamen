using System.Collections.Generic;
using System.Linq;
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
        /*
        Debug.Log(canMove);
        if (canMove == false)
            return; // Does smth with turns might wanna look into it when done
        */
        Unit unitReference = unit.GetComponent<Unit>();

        //Debug.Log(unitReference.ToString());

        if (CheckIfSameUnitSelected(unitReference))
            return;

        //selectedUnit = unitReference;

        unitReference.UI.SetActive(true);
        
        //PrepareUnitForMovement(unitReference);
    }

    private bool CheckIfSameUnitSelected(Unit unitReference)
    {

        // i think this tries to deselect the unit, so update to also remove other selection when ui system actually further done
        if (selectedUnit == unitReference)
        {
            //ClearOldMovementSelection();
            selectedUnit.UI.SetActive(false);

            // if selectedunit er i movement mode, then Clear movement selection...
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

    public void HandleEnemySelected(GameObject enemy)
    {
        // bliver called af selection manager

        // check er den en valid selection, hvis ja attack


    }

    public void PrepareUnitForAttack(Unit unitReference)
    {
        // enable attack mode

        if (selectedUnit != null)
        {
            Debug.Log("Clear old");
            // clear old attack selection and previous selection with some if logic
        }

        selectedUnit = unitReference;

        // get neighbours
        List<Vector3Int> neigbours = hexGrid.GetNeighboursFor(hexGrid.GetClosestHex(selectedUnit.transform.position));

        List<Enemy> enemiesInRange = new List<Enemy>();

        List<Enemy> enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None).ToList();
        // Find which neigbours have enemies
        foreach (Vector3Int hex in neigbours)
        {
            if (hexGrid.GetTileAt(hex).isOccupied) // just some optimisation, isn't needed
            {
                foreach (Enemy enemy in enemies)
                {
                    if (hexGrid.GetClosestHex(enemy.transform.position) == hex)
                    {
                        enemiesInRange.Add(enemy);
                    }
                }
            }
        }

        foreach (Enemy enemy in enemiesInRange)
        {
            enemy.GetComponent<Unit>().highlight.ToggleValidSelectionHighlight(true);
        }

        // highlight enemies
    }

    public void PrepareUnitForMovement(Unit unitReference)
    {
        
        if (selectedUnit != null)
        {
            Debug.Log("Clear old");
            ClearOldMovementSelection(); 
            // also clear previous state with if logic
        }

        selectedUnit = unitReference;
       
        selectedUnit.highlight.ToggleSelectedHighlight(true);
        movementSystem.ShowRange(selectedUnit, hexGrid);
    }

    private void ClearOldMovementSelection()
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
            ClearOldMovementSelection();
        }
    }

    private bool HandleSelectedHexIsUnitHex(Vector3Int hexPosition)
    {
        if (hexPosition == hexGrid.GetClosestHex(selectedUnit.transform.position))
        {
            selectedUnit.highlight.ToggleSelectedHighlight(false);
            ClearOldMovementSelection();
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
