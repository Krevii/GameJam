using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    private GameObject player;
    public List<GameObject> bonfires = new List<GameObject>();
    private int teleportPoint;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bonfires = FindObjectOfType<PerlinNoiseGenerator>().bonfires;

        teleportPoint = player.GetComponent<Player>().teleportPoint;
    }

    // Update is called once per frame
    void Update()
    {
        teleportPoint = player.GetComponent<Player>().teleportPoint;
        BonefireTeleport();
    }
    private void BonefireTeleport()
    {
        if (Vector3.Distance(player.transform.position, bonfires[0].transform.position) <= 5f && Input.GetKeyDown(KeyCode.E) && teleportPoint >= 1)
        {
            player.transform.position = bonfires[1].transform.position;
            Debug.Log("bonfires[1]");
            return;
        }
        if (Vector3.Distance(player.transform.position, bonfires[1].transform.position) <= 5f && Input.GetKeyDown(KeyCode.E) && teleportPoint >= 2)
        {
            player.transform.position = bonfires[2].transform.position;
            Debug.Log("bonfires[2]");
            return;
        }
        if (Vector3.Distance(player.transform.position, bonfires[2].transform.position) <= 5f && Input.GetKeyDown(KeyCode.E) && teleportPoint >= 3)
        {
            player.transform.position = bonfires[3].transform.position;
            Debug.Log("bonfires[3]");
            return;
        }
        if (Vector3.Distance(player.transform.position, bonfires[3].transform.position) <= 5f && Input.GetKeyDown(KeyCode.E) && teleportPoint >= 4)
        {
            player.transform.position = bonfires[4].transform.position;
            Debug.Log("bonfires[4]");
            return;
        }
        if (Vector3.Distance(player.transform.position, bonfires[4].transform.position) <= 5f && Input.GetKeyDown(KeyCode.E) && teleportPoint >= 10)
        {
            player.transform.position = bonfires[5].transform.position;
            Debug.Log("bonfires[5]");
            return;
        }

    }
}
