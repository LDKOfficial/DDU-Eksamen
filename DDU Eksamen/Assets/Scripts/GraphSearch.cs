using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


// BFS is short for Breath First Search
public class GraphSearch
{

    public static BFSResult BFSGetRange(HexGrid hexgrid, Vector3Int startPoint, int movementPoints)
    {
        Dictionary<Vector3Int, Vector3Int?> visitedNodes = new Dictionary<Vector3Int, Vector3Int?>(); // ? means it can be null
        Dictionary<Vector3Int, int> costSoFar = new Dictionary<Vector3Int, int>();
        Queue<Vector3Int> nodesToVisitQueue = new Queue<Vector3Int>();

        nodesToVisitQueue.Enqueue(startPoint); 
        costSoFar.Add(startPoint, 0);
        visitedNodes.Add(startPoint, null);

        while (nodesToVisitQueue.Count > 0)
        {
            Vector3Int currentNode = nodesToVisitQueue.Dequeue();

            foreach (Vector3Int neighbourPoistion in hexgrid.GetNeighboursFor(currentNode))
            {
                if (hexgrid.GetTileAt(neighbourPoistion).IsObstacle())
                    continue;

                int nodeCost = hexgrid.GetTileAt(neighbourPoistion).GetCost();
                int currenCost = costSoFar[currentNode];
                int newCost = currenCost + nodeCost;

                if (newCost <= movementPoints)
                {
                    if (!visitedNodes.ContainsKey(neighbourPoistion))
                    {
                        visitedNodes[neighbourPoistion] = currentNode;
                        costSoFar[neighbourPoistion] = newCost;
                        nodesToVisitQueue.Enqueue(neighbourPoistion);
                    }
                    else if (costSoFar[neighbourPoistion] > newCost)
                    {
                        costSoFar[neighbourPoistion] = newCost;
                        visitedNodes[neighbourPoistion] = currentNode;
                    }
                }
            }
        }
       

        return new BFSResult { visitedNodesDict = visitedNodes };
    }


    public static List<Vector3Int> GeneratePathBFS(Vector3Int current, Dictionary<Vector3Int, Vector3Int?> visitedNodesDict)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        path.Add(current);
        while (visitedNodesDict[current] != null)
        {
            path.Add(visitedNodesDict[current].Value);
            current = visitedNodesDict[current].Value;
        }

        path.Reverse();
        return path.Skip(1).ToList();
    }

}

public struct BFSResult
{
    public Dictionary<Vector3Int, Vector3Int?> visitedNodesDict;

    public List<Vector3Int> GetPathTo(Vector3Int destination)
    {
        if (visitedNodesDict.ContainsKey(destination) == false)
            return new List<Vector3Int>();
        return GraphSearch.GeneratePathBFS(destination, visitedNodesDict);
    }

    public bool IsHexPositionInRange(Vector3Int position)
    {
        return visitedNodesDict.ContainsKey(position);
    }

    public IEnumerable<Vector3Int> GetRangePositions()
        => visitedNodesDict.Keys;
}
