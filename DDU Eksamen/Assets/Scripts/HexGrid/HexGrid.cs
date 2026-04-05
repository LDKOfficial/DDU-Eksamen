using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    Dictionary<Vector3Int, Hex> hexTileDict = new Dictionary<Vector3Int, Hex>();
    Dictionary<Vector3Int, List<Vector3Int>> hexTileNeighboursDict = new Dictionary<Vector3Int, List<Vector3Int>>();
    private void Awake()
    {
        foreach (Hex hex in FindObjectsByType<Hex>(FindObjectsSortMode.None))
        {
            hexTileDict[hex.HexCoords] = hex;
        }
    }


    public Hex GetTileAt(Vector3Int hexCoordinates)
    {
        Hex result = null;
        hexTileDict.TryGetValue(hexCoordinates, out result);
        return result;
    }

    public List<Vector3Int> GetNeighboursFor(Vector3Int hexCoordinates)
    {
        if (hexTileDict.ContainsKey(hexCoordinates) == false)
            return new List<Vector3Int>();

        if (hexTileNeighboursDict.ContainsKey(hexCoordinates))
            return hexTileNeighboursDict[hexCoordinates];
        
        hexTileNeighboursDict.Add(hexCoordinates, new List<Vector3Int>());

        foreach (Vector3Int direction in Direction.GetDirectionList(hexCoordinates.y))
        {
            if (hexTileDict.ContainsKey(hexCoordinates + direction))
            {
                hexTileNeighboursDict[hexCoordinates].Add(hexCoordinates + direction);
            }   
        }
        return hexTileNeighboursDict[hexCoordinates];
    }

    public Vector3Int GetClosestHex(Vector3 worldPosition)
    {
        worldPosition.z = 0; // might be smth else
        return HexCoordinates.ConvertPositionToOffset(worldPosition);
    }
    
}

public static class Direction
{
    public static List<Vector3Int> directionsOffsetOdd = new List<Vector3Int>
    {
        new Vector3Int(-1,1,0), //N1
        new Vector3Int(0,1,0), //N2
        new Vector3Int(1,0,0), //E
        new Vector3Int(0,-1,0), //S2
        new Vector3Int(-1,-1, 0), //S1
        new Vector3Int(-1,0,0), //W
    };

    public static List<Vector3Int> directionsOffsetEven = new List<Vector3Int>
    {
        new Vector3Int(0,1,0), //N1
        new Vector3Int(1,1, 0), //N2
        new Vector3Int(1,0,0), //E
        new Vector3Int(1,-1, 0), //S2
        new Vector3Int(0,-1,0), //S1
        new Vector3Int(-1,0,0), //NW
    };

    public static List<Vector3Int> GetDirectionList(int y) => y % 2 == 0 ? directionsOffsetEven : directionsOffsetOdd; // if else statement


    // not using this, but would use it if we had the correct walking animations for all the directions.
    public static int GetDirection(Vector3Int StartPos, Vector3Int EndPos)
    {
        int direction = 0; // 0: NW, 1: NE, 2: E, 3: SW, 4: SE, 5: W    This does not align with the animator as it is set up for the bad 4 direction animations we have

        List<Vector3Int> directionList = GetDirectionList(StartPos.y);

        Vector3Int directionVector = EndPos - StartPos;

        foreach (Vector3Int directionCheck in directionList)
        {
            if (directionCheck == directionVector)
            {
                break;
            }
            direction++;
        }


        return direction;
    }
}
