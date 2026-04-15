using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnControler : MonoBehaviour
{

    // Liste af spiller karakterer
    private List<GameObject> playerUnits = new List<GameObject>();

    [SerializeField]
    private GameObject deathScreen;

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

    private void EnableDeathScreen()
    {
        int counter = 0;
        foreach (GameObject player in playerUnits)
        {
            if (!player.GetComponent<Unit>().isActiveAndEnabled)
            {
                counter++;
            }
        }

        if (counter == playerUnits.Count)
        {
            deathScreen.SetActive(true);
        }
    }

    private void PlayerTurnStart()
    {        
        // Enabel at spilleren kan gøre ting, og reset stats som movement og actions
        EnableDeathScreen();
        isPlayerTurn = true;
        turnCounter++;
        turnCounterDisplay.text = $"Turn: {turnCounter}";

        Debug.Log($"Is player turn {isPlayerTurn}");

        foreach (GameObject unit in playerUnits)
        {
            unit.GetComponent<Unit>().actionPoints = unit.GetComponent<Unit>().maxActionPoints;
            unit.GetComponent<Unit>().UpdateActionPoints(0);
        }
    }

    public void PlayerTurnEnd()
    {
        // Called by UI Ends player turn
        isPlayerTurn = false;

        EnemyTurnStart();
    }

    private void EnemyTurnStart()
    {
        Debug.Log("Enemy turn started");
        enemyController.EnemyTurn();

        PlayerTurnStart(); 
    }
}
