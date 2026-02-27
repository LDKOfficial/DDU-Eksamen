using System;
using UnityEngine;


[SelectionBase]
public class Hex : MonoBehaviour
{
    private HexCoordinates hexcoordinates;

    public Highlight highlight;


    [SerializeField]
    private Hextype hextype; 

    public Vector3Int HexCoords => hexcoordinates.GetHexCoords();

    private void Awake()
    {
        hexcoordinates = GetComponent<HexCoordinates>();
        highlight = GetComponent<Highlight>();
    }


    public int GetCost()
    => hextype switch
    {
        Hextype.Difficult => 20,
        Hextype.Default => 10,
        Hextype.Road => 5,
        _ => throw new Exception($"Hex of type {hextype} not supported")
    };

    public bool IsObstacle()
    {
        return this.hextype == Hextype.Obstacle;
    }

}


public enum Hextype
{
    None,
    Default,
    Difficult,
    Road,
    Water,
    Obstacle
}
