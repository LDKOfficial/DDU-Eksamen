using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    private List<Enemy> Enemies;

    private float time = 0f;

    private bool activateTimer = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None).ToList();
    }

    public void EnemyTurn()
    {
        activateTimer = true;

        Debug.Log("Enemy turn in enemy controller");
        foreach (Enemy enemy in Enemies)
        {
            time = 0f;
            StartCoroutine(Timer());

            Debug.Log("Coroutine started");

            Debug.Log(enemy.gameObject.GetComponent<Unit>().movementDuration);
            /*
            while (time < enemy.gameObject.GetComponent<Unit>().movementDuration)
            {
                Debug.Log("Inside While loop");
                Debug.Log($"Time until next enemy turn: {time}/{enemy.gameObject.GetComponent<Unit>().movementDuration}");
            }
            */
            
            
            Debug.Log("Enemy Turn");
            
            enemy.enemyTurn();
        }
        activateTimer = false;
    }

    private IEnumerator Timer()
    {
        
        while (time < 10f)
        {
            time += Time.deltaTime;
            //Debug.Log(time);
            yield return null;
        }
        yield return null;
    }

}
