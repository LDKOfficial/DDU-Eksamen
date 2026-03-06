using NUnit.Framework;
using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public HexGrid hexGrid;

    private Unit unit;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    private List<GameObject> FindAllPlayerUnits()
    {
        List<GameObject> playerUnits = GameObject.FindGameObjectsWithTag("Player").ToList<GameObject>();

        return playerUnits;
    }

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

    public void MoveToClosestPlayer()
    {
        //GetNeighbour to player and somehow select a nerby tile in range and move there
        //
    }

    private List<Vector3Int> FindShortestPathToPlayer(GameObject player)
    {
        Vector3Int playerHex = hexGrid.GetClosestHex(player.transform.position);
        BFSResult range = GraphSearch.BFSGetRange(hexGrid, hexGrid.GetClosestHex(transform.position), 10000);
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