using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int hitPoint;
    public int money;
    public int maxhitPoint = 250;
    public GameObject Puf;
    public int teleportPoint = 1;
    public GameObject weapon;
    public SpriteRenderer spriteRenderer;
    private PlayerController _controller;
    private SpriteRenderer _sp;
    public GameObject DiedText;

    
    private Animator _animatorChildren;
    private Camera _cam;
    private bool isdead = false;
    // Start is called before the first frame update
    void Start()
    {
        hitPoint = maxhitPoint;
        _cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _sp = GetComponent<SpriteRenderer>();
        _animatorChildren = GetComponentsInChildren<Animator>()[2];
        _controller = GetComponent<PlayerController>();
        money = Random.Range(100, 200);
    }

    // Update is called once per frame
    void Update()
    {
        FlipByCursorPosition();

        if (hitPoint <= 0)
        {
            isdead = true;
        }
        Dead();
    }
    private void FixedUpdate()
    {
        _controller.Run();
    }
    private void FlipByCursorPosition()
    {
        var worldCursor = _cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward);
        if (worldCursor.x > transform.position.x)
        {
            _sp.flipX = false;
        }
        if (worldCursor.x < transform.position.x)
        {
            _sp.flipX = true;
        }
    }
    public void AddMoney(int value)
    {
        money += value;
    }
    public void TakeMoney(int amount)
    {
        if (money - amount < 0)
        {
            return;
        }
        else
        {
            money -= amount;
        }
    }
    public void HitPointRemove(int amount)
    {
        hitPoint -= amount;
        HpbarClaculate();
        _animatorChildren.Play("HpBar");
    }
    public void FullHitPoint()
    {
        hitPoint = maxhitPoint;
    }
    public void Dead()
    {
        if (isdead)
        {
            gameObject.SetActive(false);
            DiedText.SetActive(true);
            Invoke("ReloadScene", 5f);
            
        }
    }
    private void HpbarClaculate()
    {

        Vector3 healthBarSize = new Vector3(hitPoint * 0.01f, 0.15f, 1);
        spriteRenderer.size = healthBarSize; //Vector3.Lerp(spriteRenderer.transform.localScale, healthBarSize, Time.deltaTime * 10f);
    }
    private void ReloadScene()
    {   
        SceneManager.LoadScene(0);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        

        if (collision.gameObject.layer == 8)
        {
            AddMoney(1);
            Instantiate(Puf, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }
    }
}
