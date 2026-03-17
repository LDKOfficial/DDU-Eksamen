using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SelectionBase]
public class Unit : MonoBehaviour
{

    public int MaxMovementPoints = 20;
    public int currentMovementPoints;


    public int maxHitPoints = 100;
    public int hitPoints;

    public GameObject UI;

    public float movementDuration = 1, rotationDuration = 0.3f; //rotation Duration might not be useful for us as it rotates the 3D objekt in tutorial

    public Highlight highlight;

    private Queue<Vector3> pathPositions = new Queue<Vector3>();

    public event Action<Unit> MovementFinished;

    public HexGrid hexGrid;

    [SerializeField]
    private List<GameObject> healthBarList = new List<GameObject>();

    [SerializeField]
    private Animator animator;

    private bool isAlive = true;

    public void Start()
    {
        currentMovementPoints = MaxMovementPoints;
        hitPoints = maxHitPoints;
        hexGrid.GetTileAt(hexGrid.GetClosestHex(transform.position)).isOccupied = true;
    }

    public void TakeDamage(int damage)
    {
        hitPoints -= damage;

        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        
        double hitPointPercent = Convert.ToDouble(hitPoints) / Convert.ToDouble(maxHitPoints);
        Debug.Log(hitPointPercent);
        foreach (GameObject health in healthBarList)
        {
            health.SetActive(true);
        }

        if (isAlive)
        {
            if (hitPointPercent <= 0f) // 0 = 0/20
            {
                Die();
            }
            else if (hitPointPercent <= 0.05f) // 0.05 = 1/20
            {
                // disable all
                healthBarList[19].SetActive(false);
                healthBarList[18].SetActive(false);
                healthBarList[17].SetActive(false);
                healthBarList[16].SetActive(false);
                healthBarList[15].SetActive(false);
                healthBarList[14].SetActive(false);
                healthBarList[13].SetActive(false);
                healthBarList[12].SetActive(false);
                healthBarList[11].SetActive(false);
                healthBarList[10].SetActive(false);
                healthBarList[9].SetActive(false);
                healthBarList[8].SetActive(false);
                healthBarList[7].SetActive(false);
                healthBarList[6].SetActive(false);
                healthBarList[5].SetActive(false);
                healthBarList[4].SetActive(false);
                healthBarList[3].SetActive(false);
                healthBarList[2].SetActive(false);
                healthBarList[1].SetActive(false);
                healthBarList[0].SetActive(false);

            }
            else if (hitPointPercent <= 0.1f) // 0.1 = 2/20
            {
                // disable first 19
                healthBarList[19].SetActive(false);
                healthBarList[18].SetActive(false);
                healthBarList[17].SetActive(false);
                healthBarList[16].SetActive(false);
                healthBarList[15].SetActive(false);
                healthBarList[14].SetActive(false);
                healthBarList[13].SetActive(false);
                healthBarList[12].SetActive(false);
                healthBarList[11].SetActive(false);
                healthBarList[10].SetActive(false);
                healthBarList[9].SetActive(false);
                healthBarList[8].SetActive(false);
                healthBarList[7].SetActive(false);
                healthBarList[6].SetActive(false);
                healthBarList[5].SetActive(false);
                healthBarList[4].SetActive(false);
                healthBarList[3].SetActive(false);
                healthBarList[2].SetActive(false);
                healthBarList[1].SetActive(false);
            }
            else if (hitPointPercent <= 0.15f) // 0.15 = 3/20
            {
                // disable first 18
                healthBarList[19].SetActive(false);
                healthBarList[18].SetActive(false);
                healthBarList[17].SetActive(false);
                healthBarList[16].SetActive(false);
                healthBarList[15].SetActive(false);
                healthBarList[14].SetActive(false);
                healthBarList[13].SetActive(false);
                healthBarList[12].SetActive(false);
                healthBarList[11].SetActive(false);
                healthBarList[10].SetActive(false);
                healthBarList[9].SetActive(false);
                healthBarList[8].SetActive(false);
                healthBarList[7].SetActive(false);
                healthBarList[6].SetActive(false);
                healthBarList[5].SetActive(false);
                healthBarList[4].SetActive(false);
                healthBarList[3].SetActive(false);
                healthBarList[2].SetActive(false);
            }
            else if (hitPointPercent <= 0.2f) // 0.2 = 4/20
            {
                // disable first 17
                healthBarList[19].SetActive(false);
                healthBarList[18].SetActive(false);
                healthBarList[17].SetActive(false);
                healthBarList[16].SetActive(false);
                healthBarList[15].SetActive(false);
                healthBarList[14].SetActive(false);
                healthBarList[13].SetActive(false);
                healthBarList[12].SetActive(false);
                healthBarList[11].SetActive(false);
                healthBarList[10].SetActive(false);
                healthBarList[9].SetActive(false);
                healthBarList[8].SetActive(false);
                healthBarList[7].SetActive(false);
                healthBarList[6].SetActive(false);
                healthBarList[5].SetActive(false);
                healthBarList[4].SetActive(false);
                healthBarList[3].SetActive(false);
            }
            else if (hitPointPercent <= 0.25f) // 0.25 = 5/20
            {
                // disable first 16
                healthBarList[19].SetActive(false);
                healthBarList[18].SetActive(false);
                healthBarList[17].SetActive(false);
                healthBarList[16].SetActive(false);
                healthBarList[15].SetActive(false);
                healthBarList[14].SetActive(false);
                healthBarList[13].SetActive(false);
                healthBarList[12].SetActive(false);
                healthBarList[11].SetActive(false);
                healthBarList[10].SetActive(false);
                healthBarList[9].SetActive(false);
                healthBarList[8].SetActive(false);
                healthBarList[7].SetActive(false);
                healthBarList[6].SetActive(false);
                healthBarList[5].SetActive(false);
                healthBarList[4].SetActive(false);
            }
            else if (hitPointPercent <= 0.3f) // 0.3 = 6/20
            {
                // disable first 15
                healthBarList[19].SetActive(false);
                healthBarList[18].SetActive(false);
                healthBarList[17].SetActive(false);
                healthBarList[16].SetActive(false);
                healthBarList[15].SetActive(false);
                healthBarList[14].SetActive(false);
                healthBarList[13].SetActive(false);
                healthBarList[12].SetActive(false);
                healthBarList[11].SetActive(false);
                healthBarList[10].SetActive(false);
                healthBarList[9].SetActive(false);
                healthBarList[8].SetActive(false);
                healthBarList[7].SetActive(false);
                healthBarList[6].SetActive(false);
                healthBarList[5].SetActive(false);
            }
            else if (hitPointPercent <= 0.35f) // 0.35 = 7/20
            {
                // disable first 14
                healthBarList[19].SetActive(false);
                healthBarList[18].SetActive(false);
                healthBarList[17].SetActive(false);
                healthBarList[16].SetActive(false);
                healthBarList[15].SetActive(false);
                healthBarList[14].SetActive(false);
                healthBarList[13].SetActive(false);
                healthBarList[12].SetActive(false);
                healthBarList[11].SetActive(false);
                healthBarList[10].SetActive(false);
                healthBarList[9].SetActive(false);
                healthBarList[8].SetActive(false);
                healthBarList[7].SetActive(false);
                healthBarList[6].SetActive(false);
            }
            else if (hitPointPercent <= 0.4f) // 0.4 = 8/20
            {
                // disable first 13
                healthBarList[19].SetActive(false);
                healthBarList[18].SetActive(false);
                healthBarList[17].SetActive(false);
                healthBarList[16].SetActive(false);
                healthBarList[15].SetActive(false);
                healthBarList[14].SetActive(false);
                healthBarList[13].SetActive(false);
                healthBarList[12].SetActive(false);
                healthBarList[11].SetActive(false);
                healthBarList[10].SetActive(false);
                healthBarList[9].SetActive(false);
                healthBarList[8].SetActive(false);
                healthBarList[7].SetActive(false);
            }
            else if (hitPointPercent <= 0.45f) // 0.45 = 9/20
            {
                // disable first 12
                healthBarList[19].SetActive(false);
                healthBarList[18].SetActive(false);
                healthBarList[17].SetActive(false);
                healthBarList[16].SetActive(false);
                healthBarList[15].SetActive(false);
                healthBarList[14].SetActive(false);
                healthBarList[13].SetActive(false);
                healthBarList[12].SetActive(false);
                healthBarList[11].SetActive(false);
                healthBarList[10].SetActive(false);
                healthBarList[9].SetActive(false);
                healthBarList[8].SetActive(false);
            }
            else if (hitPointPercent <= 0.5f) // 0.5 = 10/20
            {
                //  disable first 11
                healthBarList[19].SetActive(false);
                healthBarList[18].SetActive(false);
                healthBarList[17].SetActive(false);
                healthBarList[16].SetActive(false);
                healthBarList[15].SetActive(false);
                healthBarList[14].SetActive(false);
                healthBarList[13].SetActive(false);
                healthBarList[12].SetActive(false);
                healthBarList[11].SetActive(false);
                healthBarList[10].SetActive(false);
                healthBarList[9].SetActive(false);
            }
            else if (hitPointPercent <= 0.55f) // 0.55 = 11/20
            {
                // disable first 10
                healthBarList[19].SetActive(false);
                healthBarList[18].SetActive(false);
                healthBarList[17].SetActive(false);
                healthBarList[16].SetActive(false);
                healthBarList[15].SetActive(false);
                healthBarList[14].SetActive(false);
                healthBarList[13].SetActive(false);
                healthBarList[12].SetActive(false);
                healthBarList[11].SetActive(false);
                healthBarList[10].SetActive(false);
            }
            else if (hitPointPercent <= 0.6f) // 0.6 = 12/20
            {
                // disable first nine
                healthBarList[19].SetActive(false);
                healthBarList[18].SetActive(false);
                healthBarList[17].SetActive(false);
                healthBarList[16].SetActive(false);
                healthBarList[15].SetActive(false);
                healthBarList[14].SetActive(false);
                healthBarList[13].SetActive(false);
                healthBarList[12].SetActive(false);
                healthBarList[11].SetActive(false);
            }
            else if (hitPointPercent <= 0.65f) // 0.65 = 13/20
            {
                // disable first eight
                healthBarList[19].SetActive(false);
                healthBarList[18].SetActive(false);
                healthBarList[17].SetActive(false);
                healthBarList[16].SetActive(false);
                healthBarList[15].SetActive(false);
                healthBarList[14].SetActive(false);
                healthBarList[13].SetActive(false);
                healthBarList[12].SetActive(false);
            }
            else if (hitPointPercent <= 0.7f) // 0.7 = 14/20
            {
                // disable first seven
                healthBarList[19].SetActive(false);
                healthBarList[18].SetActive(false);
                healthBarList[17].SetActive(false);
                healthBarList[16].SetActive(false);
                healthBarList[15].SetActive(false);
                healthBarList[14].SetActive(false);
                healthBarList[13].SetActive(false);
            }
            else if (hitPointPercent <= 0.75f) // 0.75 = 15/20
            {
                // disable first six
                healthBarList[19].SetActive(false);
                healthBarList[18].SetActive(false);
                healthBarList[17].SetActive(false);
                healthBarList[16].SetActive(false);
                healthBarList[15].SetActive(false);
                healthBarList[14].SetActive(false);
            }
            else if (hitPointPercent <= 0.8f) // 0.8 = 16/20
            {
                // disable first five
                healthBarList[19].SetActive(false);
                healthBarList[18].SetActive(false);
                healthBarList[17].SetActive(false);
                healthBarList[16].SetActive(false);
                healthBarList[15].SetActive(false);
            }
            else if (hitPointPercent <= 0.85f) // 0.85 = 17/20
            {
                // disable first four
                healthBarList[19].SetActive(false);
                healthBarList[18].SetActive(false);
                healthBarList[17].SetActive(false);
                healthBarList[16].SetActive(false);
            }
            else if (hitPointPercent <= 0.9f) // 0.9 = 18/18
            {
                // disable first three
                healthBarList[19].SetActive(false);
                healthBarList[18].SetActive(false);
                healthBarList[17].SetActive(false);
            }
            else if (hitPointPercent <= 0.95f) // 0.95 = 19/20
            {
                // disable first two
                healthBarList[19].SetActive(false);
                healthBarList[18].SetActive(false);
            }
            else if (hitPointPercent < 1) // 1 = 20/20
            {
                // disable first one
                healthBarList[19].SetActive(false);
            }
            else
            {
                // alle er tænt
            }
        }
    }

    private void Die()
    {
        isAlive = false;
        highlight.enabled = false;
        this.enabled = false;
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        animator.SetBool("Dead", true);
        
    }

    public void MoveThroughPath(List<Vector3> CurrentPath)
    {
        hexGrid.GetTileAt(hexGrid.GetClosestHex(transform.position)).isOccupied = false;
        hexGrid.GetTileAt(hexGrid.GetClosestHex(CurrentPath[CurrentPath.Count-1])).isOccupied = true;
        pathPositions = new Queue<Vector3>(CurrentPath);
        Vector3 firstTarget = pathPositions.Dequeue();
        //StartCoroutine(RotationCoroutine(firstTarget, rotationDuration))
        this.gameObject.GetComponent<Collider2D>().enabled = false;
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
            //Debug.Log("Selecting the Next position!");
            //StartCoroutine(RotationCoroutine(pathPositions.Dequeue(), rotationDuration));
            StartCoroutine(MovementCoroutine(pathPositions.Dequeue()));
        }
        else
        {
            //Debug.Log("Movement finished!");
            //hexGrid.GetTileAt(hexGrid.GetClosestHex(endPositione)).isOccupied = true;
            this.gameObject.GetComponent<Collider2D>().enabled = true;
            MovementFinished?.Invoke(this);
        }
    }



    public void Attack(Unit enemy)
    {

    }
}
