using NUnit.Framework;
using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.EventSystems.EventTrigger;

public class Enemy : MonoBehaviour
{
    private Unit unit;

    private BFSResult range;

    List<GameObject> playerUnits = new List<GameObject>();

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Something entered collider");
        if (collision.gameObject.tag == "Player" && !collision.isTrigger)
        {
            playerUnits.Add(collision.gameObject);
        }
        
    }

    public void enemyTurn()
    {

        if (playerUnits.Count == 0)
        {
            Debug.Log("Enemy skips movement");
            return;
        }

        // try attact if not, move, then try attact again

        if (!AttackClosestPlayer())
        {
            MoveToClosestPlayer();
        }
            
    }

    private bool AttackClosestPlayer()
    {
        Unit neigbouringPlayer = FindPlayerOnNeigboringHex();

        if (neigbouringPlayer != null)
        {

            Vector3 rotation = neigbouringPlayer.transform.position - transform.position;

            float rotationZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

            unit.attackPivot.transform.rotation = Quaternion.Euler(0, 0, rotationZ);

            unit.animator.SetTrigger("Attack");

            neigbouringPlayer.TakeDamage(unit.damage);

            unit.attackSound.Play();

            return true;
        }
        else
        {
            return false;
        }
    }

    public void Attack()
    {
        bool attacked = AttackClosestPlayer();  
    }

    private Unit FindPlayerOnNeigboringHex()
    {
        foreach (GameObject playerUnit in playerUnits)
        {
            foreach (Vector3Int neigbour in unit.hexGrid.GetNeighboursFor(unit.hexGrid.GetClosestHex(transform.position)))
            {
                if (neigbour == unit.hexGrid.GetClosestHex(playerUnit.transform.position))
                {
                    return playerUnit.GetComponent<Unit>();
                }
            }
        }
        return null;
    }

    // Finds the cheapest Neigbour of a player to get to
    private Vector3Int FindCheapestNeighbourOfAPlayer(List<GameObject> playerUnits)
    {
        range = GraphSearch.BFSGetRange(unit.hexGrid, unit.hexGrid.GetClosestHex(transform.position), 10000);

        Vector3Int cheapestNeigbour = unit.hexGrid.GetClosestHex(transform.position);
        int cheapestCost = 100000;

        foreach (GameObject playerUnit in playerUnits)
        {
            foreach (Vector3Int neighbour in unit.hexGrid.GetNeighboursFor(unit.hexGrid.GetClosestHex(playerUnit.transform.position)))
            {
                if (!unit.hexGrid.GetTileAt(neighbour).IsObstacle() & unit.hexGrid.GetTileAt(neighbour).isOccupied == false)
                {
                    if (range.costSoFar[neighbour] < cheapestCost)
                    {
                        cheapestCost = range.costSoFar[neighbour];
                        cheapestNeigbour = neighbour;
                    }
                }
            }
        }

        return cheapestNeigbour;
    }


    // move enemy to closest player
    private void MoveToClosestPlayer()
    {

        Vector3Int cheapestNeigbour = FindCheapestNeighbourOfAPlayer(playerUnits);

        List<Vector3Int> fullPath = range.GetPathTo(cheapestNeigbour);

        int costSoFar = 0;

        List<Vector3Int> path = new List<Vector3Int>();
        foreach (Vector3Int hex in fullPath)
        {
            costSoFar += unit.hexGrid.GetTileAt(hex).GetCost();
            if (costSoFar <= unit.actionPoints)
            {
                path.Add(hex);
            }
            else
            {
                break; 
            }
        }

        unit.MoveThroughPath(path.Select(pos => unit.hexGrid.GetTileAt(pos).transform.position).ToList());

    }
}