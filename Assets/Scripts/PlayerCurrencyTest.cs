using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCurrencyTest : MonoBehaviour
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
            TestCurrencyManagement();
        }
    }
    void TestCurrencyManagement()
    {
        int initialMoney = player.money;

        // Тест на добавление валюты
        int addedMoney = 50;
        player.AddMoney(addedMoney);
        IsPassed = player.money == (initialMoney + addedMoney);
    }
}
