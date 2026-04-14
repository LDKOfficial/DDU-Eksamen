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

    private List<Enemy> enemiesInRange = new List<Enemy>();

    private List<Enemy> enemiesInLineOfSite = new List<Enemy>();

    [SerializeField]
    private Unit selectedUnit;
    private Hex previouslySelectedHex;

    private bool preparedForAttack = false;

    private bool preparedForSpecial = false;

    private bool preparedForMove = false;

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

        if (selectedUnit != null)
        {
            selectedUnit.UI.SetActive(false);
        }
        

        selectedUnit = unitReference;

        selectedUnit.UI.SetActive(true);
        
        //PrepareUnitForMovement(unitReference);
    }

    private bool CheckIfSameUnitSelected(Unit unitReference)
    {
        Debug.Log("Checking if same unit selected");

        // i think this tries to deselect the unit, so update to also remove other selection when ui system actually further done
        
        if (selectedUnit != null)
        {
            if (selectedUnit.name == unitReference.name)
            {
                Debug.Log("Same Unit selected");
                //ClearOldMovementSelection();
                selectedUnit.UI.SetActive(false);
                selectedUnit = null;
                // should clear all selections
                // if selectedunit er i movement mode, then Clear movement selection...
                return true;
            }
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

        if (preparedForSpecial)
        { 
            if (enemiesInLineOfSite.Contains(enemy.GetComponent<Enemy>()))
            {
                enemy.GetComponent<Unit>().highlight.ToggleValidSelectionHighlight(false);
                enemy.GetComponent<Unit>().highlight.ToggleSelectedHighlight(true);
                selectedUnit.SpecialAttack(enemy.GetComponent<Unit>());
            }

            foreach (Enemy enemyInLineOfSite in enemiesInLineOfSite)
            {
                enemyInLineOfSite.GetComponent<Unit>().highlight.ToggleValidSelectionHighlight(false);
            }
        }
        else
        {
            // check er den en valid selection, hvis ja attack

            if (enemiesInRange.Contains(enemy.GetComponent<Enemy>()))
            {

                enemy.GetComponent<Unit>().highlight.ToggleValidSelectionHighlight(false);
                enemy.GetComponent<Unit>().highlight.ToggleSelectedHighlight(true);
                selectedUnit.Attack(enemy.GetComponent<Unit>());
            }

            foreach (Enemy enemyInRange in enemiesInRange)
            {
                enemyInRange.GetComponent<Unit>().highlight.ToggleValidSelectionHighlight(false);
            }
        }

    }

    public void PrepareUnitForAttack(Unit unitReference)
    {
        // enable attack mode

        preparedForSpecial = false; // needs to be moved

        if (selectedUnit != null)
        {
            Debug.Log("Clear old");
            // clear old attack selection and previous selection with some if logic
            if (preparedForAttack)
            {
                ClearOldAttackSelection();
            }
            else if (preparedForSpecial)
            {
                ClearOldSpecialSelection();
            }
            else if (preparedForMove)
            {
                ClearOldMovementSelection();
            }
        }
        preparedForAttack = true;
        selectedUnit = unitReference;
        Debug.Log(selectedUnit);
        Debug.Log(selectedUnit.transform.position);
        // get neighbours
        List<Vector3Int> neigbours = hexGrid.GetNeighboursFor(hexGrid.GetClosestHex(selectedUnit.transform.position));

        enemiesInRange = new List<Enemy>();

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

        // highlight enemies
        foreach (Enemy enemy in enemiesInRange)
        {
            enemy.GetComponent<Unit>().highlight.ToggleValidSelectionHighlight(true);
        }

        
    }

    public void PrepareUnitForSpecial(Unit unitReference)
    {


        if (selectedUnit != null)
        {
            Debug.Log("Clear old");
            // clear old special selection and previous selection with some if logic
            if (preparedForAttack)
            {
                ClearOldAttackSelection();
            }
            else if (preparedForSpecial)
            {
                ClearOldSpecialSelection();
            }
            else if (preparedForMove)
            {
                ClearOldMovementSelection();
            }
        }

        preparedForSpecial = true;
        selectedUnit = unitReference;

        foreach (Enemy enemy in selectedUnit.enemiesInSpecialRange)
        {
            List<RaycastHit2D> raycastHit2Ds = new List<RaycastHit2D>();

            Vector3 direction = enemy.transform.position - selectedUnit.transform.position;
            // raycast to see if obstructed by wall.
            int results = Physics2D.Raycast(selectedUnit.transform.position, direction, ContactFilter2D.noFilter, raycastHit2Ds, direction.magnitude);

            Debug.Log($"Number of hits{results}");
            bool hasLineofSight = true;
            foreach (RaycastHit2D hit2D in raycastHit2Ds)
            {
                if (hit2D.collider.gameObject.TryGetComponent<Hex>(out Hex hex))
                {
                    if (hex.hextype == Hextype.Wall)
                    {
                        hasLineofSight = false;
                        Debug.Log("Wall Detected");
                        break;
                    }
                }
            }

            if (hasLineofSight)
            {
                enemy.GetComponent<Unit>().highlight.ToggleValidSelectionHighlight(true);
                enemiesInLineOfSite.Add(enemy);
            }



        }
    }

    public void PrepareUnitForMovement(Unit unitReference) 
    {
        // also clear previous state with if logic after some check of what the fuck the previous state was
        if (selectedUnit != null)
        {
            if (preparedForAttack)
            {
                ClearOldAttackSelection();
            }
            else if (preparedForSpecial)
            {
                ClearOldSpecialSelection();
            }
            else if (preparedForMove)
            {
                ClearOldMovementSelection();
            }
        }
        preparedForMove = true;
        selectedUnit = unitReference;
       
        selectedUnit.highlight.ToggleSelectedHighlight(true);
        movementSystem.ShowRange(selectedUnit, hexGrid);
    }

    private void ClearOldAttackSelection()
    {
        foreach (Enemy enemy in enemiesInRange)
        {
            enemy.GetComponent<Unit>().highlight.ToggleValidSelectionHighlight(false);
        }
        preparedForAttack = false;
    }

    private void ClearOldSpecialSelection()
    {
        foreach (Enemy enemy in enemiesInLineOfSite)
        {
            enemy.GetComponent<Unit>().highlight.ToggleValidSelectionHighlight(false);
        }
        preparedForSpecial = false;
    }

    private void ClearOldMovementSelection()
    {
        previouslySelectedHex = null;
        selectedUnit.highlight.ToggleSelectedHighlight(false);
        movementSystem.HideRange(hexGrid);
        selectedUnit = null;
        preparedForMove = false;
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
            selectedUnit.movementFinished += ResetTurn; // Might wanna change this messes with turns
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
        selectedUnit.movementFinished -= ResetTurn;
        canMove = true;
    }

}
