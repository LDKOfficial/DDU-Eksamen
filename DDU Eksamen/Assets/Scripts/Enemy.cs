using NUnit.Framework;
using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public HexGrid hexGrid;

    private Unit unit;

    int damage = 10;

    private BFSResult range;

    List<GameObject> playerUnits = new List<GameObject>();

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Something entered collider");
        if (collision.gameObject.tag == "Player")
        {
            playerUnits.Add(collision.gameObject);
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Something exited collider");
        playerUnits.Remove(collision.gameObject);
    }

    public void enemyTurn()
    {

        if (playerUnits.Count == 0)
        {
            Debug.Log("Enemy skips movement");
            return;
        }

        // try attact if not, move, then try attact again?
        Unit neigbouringPlayer = FindPlayerOnNeigboringHex();

        if (neigbouringPlayer != null)
        {
            // attack and skib player movement

            neigbouringPlayer.TakeDamage(damage);

        }
        else
        {
            MoveToClosestPlayer();

            neigbouringPlayer = FindPlayerOnNeigboringHex();

            // attack

            if (neigbouringPlayer != null)
            {
                // attack
                neigbouringPlayer.TakeDamage(damage);
            }

        }
            
    }


    private Unit FindPlayerOnNeigboringHex()
    {
        foreach (GameObject playerUnit in playerUnits)
        {
            foreach (Vector3Int neigbour in hexGrid.GetNeighboursFor(hexGrid.GetClosestHex(transform.position)))
            {
                if (neigbour == hexGrid.GetClosestHex(playerUnit.transform.position))
                {
                    return playerUnit.GetComponent<Unit>();
                }
            }
        }
        return null;
    }


    // kinda dont like this: Lasse, prefere the result other method
    private GameObject FindClosestPlayerUnit(List<GameObject> playerUnits)
    {
        float shortestDistance = Mathf.Infinity;

        GameObject closestPlayer = null;

        foreach (GameObject playerUnit in playerUnits)
        {
            if ((playerUnit.transform.position - transform.position).magnitude < shortestDistance)
            {
                closestPlayer = playerUnit;
            }
        }

        return closestPlayer;
    }

    // Finds the cheapest Neigbour of a player to get to
    private Vector3Int FindCheapestNeighbourOfAPlayer(List<GameObject> playerUnits)
    {
        range = GraphSearch.BFSGetRange(hexGrid, hexGrid.GetClosestHex(transform.position), 10000);

        Vector3Int cheapestNeigbour = hexGrid.GetClosestHex(transform.position);
        int cheapestCost = 100000;

        foreach (GameObject playerUnit in playerUnits)
        {
            foreach (Vector3Int neighbour in hexGrid.GetNeighboursFor(hexGrid.GetClosestHex(playerUnit.transform.position)))
            {
                if (!hexGrid.GetTileAt(neighbour).IsObstacle() & hexGrid.GetTileAt(neighbour).isOccupied == false)
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


    // should move enemy to closest player
    private void MoveToClosestPlayer()
    {
        //GetNeighbour to player and somehow select a nerby tile in range and move there
        // Use FindCheapestNeighbourOfAPlayer to find the position to move to, then use the movement system or something to move after finding path with graphsearch.


        Vector3Int cheapestNeigbour = FindCheapestNeighbourOfAPlayer(playerUnits);


        // not have infinite  movement

        List<Vector3Int> fullPath = range.GetPathTo(cheapestNeigbour);

        int costSoFar = 0;

        List<Vector3Int> path = new List<Vector3Int>();
        foreach (Vector3Int hex in fullPath)
        {
            costSoFar += hexGrid.GetTileAt(hex).GetCost();
            if (costSoFar <= unit.currentMovementPoints)
            {
                path.Add(hex);
            }
            else
            {
                break; // Wow i aktually tried to optimise code for once in my in my life
            }
        }

        unit.MoveThroughPath(path.Select(pos => hexGrid.GetTileAt(pos).transform.position).ToList());

    }


    // cant see what i would use this fore tbh
    private List<Vector3Int> FindCheapestPathToPlayer(GameObject player)
    {
        Vector3Int playerHex = hexGrid.GetClosestHex(player.transform.position);
        range = GraphSearch.BFSGetRange(hexGrid, hexGrid.GetClosestHex(transform.position), 10000);
        int lowestCost = 10000;
        Vector3Int closestNeighbour = new Vector3Int(10000, 10000, 10000);

        foreach(Vector3Int neighbour in hexGrid.GetNeighboursFor(playerHex))
        {
            if (range.costSoFar[neighbour] < lowestCost)
            {
                closestNeighbour = neighbour;
            }
        }
        return range.GetPathTo(closestNeighbour);
    }
}