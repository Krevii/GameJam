using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerControllerTest : MonoBehaviour
{
    private Player player;
    private PlayerController playerController;
    [SerializeField]
    private bool IsPassed = false;
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TestPlayerControllerInteraction();
        }
    }

    void TestPlayerControllerInteraction()
    {
        // Тест на запуск рывка
        Vector2 startPosition = player.transform.position;

        playerController.Dash();

        IsPassed = startPosition.x != player.transform.position.x;
    }
}
