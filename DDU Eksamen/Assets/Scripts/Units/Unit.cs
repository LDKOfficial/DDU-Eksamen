using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SelectionBase]
public class Unit : MonoBehaviour
{

    public int MaxMovementPoints = 20;

    
    public int currentMovementPoints;


    [SerializeField]
    private float movementDuration = 1, rotationDuration = 0.3f; //rotation Duration might not be useful for us as it rotates the 3D objekt in tutorial

    [SerializeField]
    public Highlight highlight;

    private Queue<Vector3> pathPositions = new Queue<Vector3>();

    public event Action<Unit> MovementFinished;

    public HexGrid hexGrid;

    public void Start()
    {
        currentMovementPoints = MaxMovementPoints;
        hexGrid.GetTileAt(hexGrid.GetClosestHex(transform.position)).isOccupied = true;
    }

    public void MoveThroughPath(List<Vector3> CurrentPath)
    {
        hexGrid.GetTileAt(hexGrid.GetClosestHex(transform.position)).isOccupied = false;
        pathPositions = new Queue<Vector3>(CurrentPath);
        Vector3 firstTarget = pathPositions.Dequeue();
        //StartCoroutine(RotationCoroutine(firstTarget, rotationDuration));
        StartCoroutine(MovementCoroutine(firstTarget));
    }

    private IEnumerator RotationCoroutine(Vector3 endPositione, float rotationDuration)
    {
        Quaternion startRotation = transform.rotation;
        endPositione.y = transform.position.y;
        Vector3 direction = endPositione - transform.position;
        Quaternion endRotation = Quaternion.LookRotation(direction, Vector3.up);

        if (Mathf.Approximately(Mathf.Abs(Quaternion.Dot(startRotation, endRotation)), 1.0f) == false)
        {
            float timeElapsed = 0;
            while (timeElapsed < rotationDuration)
            {
                timeElapsed += Time.deltaTime;
                float lerpStep = timeElapsed / rotationDuration;
                transform.rotation = Quaternion.Lerp(startRotation, endRotation, lerpStep);
                yield return null;
            }
            transform.rotation = endRotation;
        }
        StartCoroutine(MovementCoroutine(endPositione));
    }

    private IEnumerator MovementCoroutine(Vector3 endPositione)
    {
        Vector3 startPosition = transform.position;
        endPositione.z = startPosition.z;
        float timeElapsed = 0;

        while (timeElapsed < movementDuration)
        {
            timeElapsed += Time.deltaTime;
            float lerStep = timeElapsed / movementDuration;
            transform.position = Vector3.Lerp(startPosition, endPositione, lerStep);
            yield return null;
        }
        transform.position = endPositione;

        

        if (pathPositions.Count > 0)
        {
            Debug.Log("Selecting the Next position!");
            //StartCoroutine(RotationCoroutine(pathPositions.Dequeue(), rotationDuration));
            StartCoroutine(MovementCoroutine(pathPositions.Dequeue()));
        }
        else
        {
            Debug.Log("Movement finished!");
            hexGrid.GetTileAt(hexGrid.GetClosestHex(endPositione)).isOccupied = true;
            MovementFinished?.Invoke(this);
        }
    }
}
