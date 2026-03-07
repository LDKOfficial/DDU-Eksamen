using NUnit.Framework;
using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public HexGrid hexGrid;

    private Unit unit;

    private BFSResult range;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    private List<GameObject> FindAllPlayerUnits()
    {
        List<GameObject> playerUnits = GameObject.FindGameObjectsWithTag("Player").ToList<GameObject>();

        return playerUnits;
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

    // Finds the cheapes Neigbour of a player to get to
    private Vector3Int FindCheapestNeighbourOfAPlayer(List<GameObject> playerUnits)
    {
        range = GraphSearch.BFSGetRange(hexGrid, hexGrid.GetClosestHex(transform.position), 10000);

        Vector3Int cheapestNeigbour = hexGrid.GetClosestHex(transform.position);
        int cheapestCost = 100000;

        foreach (GameObject playerUnit in playerUnits)
        {
            foreach (Vector3Int neighbour in hexGrid.GetNeighboursFor(hexGrid.GetClosestHex(playerUnit.transform.position)))
            {
                if (range.costSoFar[neighbour] < cheapestCost)
                {
                    cheapestCost = range.costSoFar[neighbour];
                    cheapestNeigbour = neighbour;
                }
            }
        }

        return cheapestNeigbour;
    }


    // should move enemy to closest player
    public void MoveToClosestPlayer()
    {
        //GetNeighbour to player and somehow select a nerby tile in range and move there
        // Use FindCheapestNeighbourOfAPlayer to find the position to move to, then use the movement system or something to move after finding path with graphsearch.
        List<GameObject> playerUnits = FindAllPlayerUnits();

        Vector3Int cheapesNeigbour = FindCheapestNeighbourOfAPlayer(playerUnits);

        List<Vector3Int> path = range.GetPathTo(cheapesNeigbour);

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