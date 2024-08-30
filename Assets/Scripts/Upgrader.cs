using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Upgrader : MonoBehaviour
{
    // Start is called before the first frame update
    public int price;
    public TextMeshPro priceText;
    public List<GameObject> weaponLevel = new List<GameObject>();
    private Weapon weapon;
    private GameObject Hand;

    private GameObject player;

    void Start()
    {
        priceText.text = "Price:" + price.ToString();
        player = GameObject.FindGameObjectWithTag("Player");
        Hand = GameObject.Find("Hand");
        weapon = player.GetComponent<Player>().weapon.GetComponent<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= 5f)
        {
            if (Input.GetKeyDown(KeyCode.E) && player.GetComponent<Player>().money >= price)
            {
                player.GetComponent<Player>().TakeMoney(price);

                weapon = player.GetComponent<Player>().weapon.GetComponent<Weapon>();

                GameObject weaponObj = Instantiate(weaponLevel[weapon.level+1], player.GetComponent<Player>().weapon.transform.position, player.GetComponent<Player>().weapon.transform.rotation, Hand.transform);
                Destroy(player.GetComponent<Player>().weapon);
                player.GetComponent<Player>().weapon = weaponObj;
                


            }
            transform.localScale = Vector3.one * 1.2f;
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }

    
}
