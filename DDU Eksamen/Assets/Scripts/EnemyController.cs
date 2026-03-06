using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemyController : MonoBehaviour
{
    List<Enemy> Enemies;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None).ToList();
    }


}
