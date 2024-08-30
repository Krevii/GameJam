using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class UIController : MonoBehaviour
{
    
    public TextMeshProUGUI MoneyText;

    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        
    }

    // Update is called once per frame
    void Update()
    {
        MoneyUpdate();
    }

    public void MoneyUpdate()
    {
        MoneyText.text = player.money.ToString();
    }
}
