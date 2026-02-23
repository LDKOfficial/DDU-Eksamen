using UnityEngine;


[SelectionBase]
public class Hex : MonoBehaviour
{
    private HexCoordinates hexcoordinates;

    public Vector3Int HexCoords => hexcoordinates.GetHexCoords();

    private void Awake()
    {
        hexcoordinates = GetComponent<HexCoordinates>();
    }

}
