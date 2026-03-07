using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnControler : MonoBehaviour
{

    // Liste af spiller karakterer
    private List<GameObject> playerUnits = new List<GameObject>();

    [SerializeField]
    private EnemyController enemyController;

    public int turnCounter = 1;

    public bool isPlayerTurn = true;

    [SerializeField]
    private TextMeshProUGUI turnCounterDisplay;
    void Awake()
    {
        playerUnits.AddRange(GameObject.FindGameObjectsWithTag("Player"));
    }



    private void PlayerTurnStart()
    {
        isPlayerTurn = true;
        turnCounter++;
        turnCounterDisplay.text = $"Turn: {turnCounter}";

        foreach (GameObject unit in playerUnits)
        {
            unit.GetComponent<Unit>().currentMovementPoints = unit.GetComponent<Unit>().MaxMovementPoints;
        }
        
        // Enabel at spilleren kan gøre ting, og reset stats som movement og actions
    }

    public void PlayerTurnEnd()
    {
        // Called by UI Ends player turn
        isPlayerTurn = false;


        //PlayerTurnStart(); // temp
        EnemyTurnStart(); // at end of method temp disabled
    }

    private void EnemyTurnStart()
    {
        // Disable at spilleren kan gøre ting, og flytte på enemies, angribe med enemies og tager ande actions.

        enemyController.EnemyMovement();

        PlayerTurnStart(); // temp
    }


}
