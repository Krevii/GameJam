using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerHealthTest : MonoBehaviour
{
    private Player player;
    [SerializeField]
    private bool IsPassed = false;
    
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TestPlayerHealth();
        }
    }
    void TestPlayerHealth()
    {
        int initialHealth = player.hitPoint;

        // “ест на уменьшение жизненных показателей
        int damage = 50;
        player.HitPointRemove(damage);
        IsPassed = player.hitPoint == (initialHealth - damage);
    }
}
