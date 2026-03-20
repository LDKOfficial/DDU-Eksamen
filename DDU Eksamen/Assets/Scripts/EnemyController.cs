using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    private List<Enemy> Enemies;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None).ToList();
    }

    public void EnemyTurn()
    {
        List<Enemy> enemiesToRemove = new List<Enemy>();

        Debug.Log("Enemy turn in enemy controller");
        foreach (Enemy enemy in Enemies)
        {   
            if (!enemy.enabled)
            {
                enemiesToRemove.Add(enemy);
            }
            else
            {
                Debug.Log("Enemy Turn");
                enemy.enemyTurn();
            }
        }

        foreach (Enemy enemy in enemiesToRemove)
        {
            Enemies.Remove(enemy);
        }

    }

}
