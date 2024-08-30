using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float dashSpeed; // Скорость рывка
    public float dashDuration; // Длительность рывка
    public GameObject DustParticle;
    public GameObject DashDustParticle;

    private Animator _animator;
    private bool isDashing; // Переменная, указывающая, находится ли игрок в состоянии рывка
    private float dashTime; // Время, оставшееся до конца рывка
    private Rigidbody2D rb;
    private float moveX;
    private float moveY;
    private Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        StartCoroutine("DustParticleDelay");
        StartCoroutine("DashDustParticleDelay");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.velocity = movement * Time.fixedDeltaTime * dashSpeed;
            Debug.Log("Dassssssh");
        }
        Dash();
    }
    void Update()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        movement = new Vector2 (moveX, moveY);


        if(movement != Vector2.zero)
        {
            _animator.Play("PlayerRun");
        }
        else
        {
            _animator.Play("PlayerIdle");
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
        {
            isDashing = true;
            dashTime = dashDuration;
            //rb.velocity = movement * Time.fixedDeltaTime * dashSpeed;
        }
        
    }
    public void Dash()
    {
        if (isDashing)
        {
            if (dashTime <= 0)
            {
                isDashing = false;
                rb.velocity = Vector2.zero;
            }
            else
            {
                dashTime -= Time.fixedDeltaTime;
            }
        }

    }
    public void Run()
    {
        
        if (rb != null && !isDashing)
        {

            rb.velocity = movement * Time.fixedDeltaTime * speed;
            
            
        }
    }
    IEnumerator DustParticleDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
         
            if (movement != Vector2.zero)
            {
                Instantiate(DustParticle, new Vector3(transform.position.x,transform.position.y - 0.2f,transform.position.z) ,Quaternion.identity);
            }
        }
    }
    IEnumerator DashDustParticleDelay()
    {
        while (true)
        {
            //yield return new WaitForSeconds(0.1f);
            if (isDashing && movement != Vector2.zero)
            {
                Instantiate(DashDustParticle, new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z), Quaternion.identity);
            }
            yield return null;
        }
    }
}
