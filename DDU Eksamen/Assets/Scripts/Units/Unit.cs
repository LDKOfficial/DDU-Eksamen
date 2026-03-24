using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;


[SelectionBase]
public class Unit : MonoBehaviour
{
    [Header("Stats")]
    public int maxActionPoints = 6;
    public int actionPoints;

    [Header("")]
    public int maxHitPoints = 100;
    public int hitPoints;

    [Header("")]
    public int damage = 20;

    [Header("Action Costs")]
    public int attackCost = 3;

    [Header("Unity Shit")]
    public HexGrid hexGrid;
    public float movementDuration = 1, rotationDuration = 0.3f; //rotation Duration might not be useful for us as it rotates the 3D objekt in tutorial

    

    [Header("UI")]
    public GameObject UI;
    [SerializeField]
    private TextMeshProUGUI attackCostDisplay;  

    [SerializeField]
    private List<GameObject> actionPointBarList = new List<GameObject>();

    public Highlight highlight;

    [SerializeField]
    private List<GameObject> healthBarList = new List<GameObject>();



    private Queue<Vector3> pathPositions = new Queue<Vector3>();

    public event Action<Unit> MovementFinished;

    



    [SerializeField]
    private Animator animator;

    private bool isAlive = true;

    [HideInInspector]
    public bool haveMoved = false;

    public void Start()
    {
        hitPoints = maxHitPoints;
        actionPoints = maxActionPoints;
        hexGrid = FindFirstObjectByType<HexGrid>();
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


        if (isAlive)
        {
            if (hitPoints <= 0)
            {
                Die();
            }
            else
            {
                float counter = 0.05f;
                foreach (GameObject ui in healthBarList)
                {
                    ui.SetActive(true);
                    
                    if (hitPointPercent <= counter)
                    {
                        ui.SetActive(false);
                    }
                    counter += 0.05f;
                }
            }


        }
    }

    private void Die()
    {
        isAlive = false;
        highlight.enabled = false;
        this.enabled = false;
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        if (this.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.enabled = false;
        }
        
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
        if (actionPoints >= attackCost)
        {
            UpdateActionPoints(attackCost);
            enemy.TakeDamage(damage);
        }

    }

    public void UpdateActionPoints(int actionPointCost)
    {
        actionPoints -= actionPointCost;

        // update ui element with new actionpoints
        int counter = 1;
        foreach (GameObject ui in actionPointBarList)
        {
            ui.SetActive(false);

            if (actionPoints >= counter)
            {
                ui.SetActive(true);
            }

            counter++;
        }
    }
}
