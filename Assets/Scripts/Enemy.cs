using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int healthPoint = 100;
    public int damage;
    public GameObject Drop;
    //public GameObject Puf;
    public SpriteRenderer spriteRenderer;
    //public float speed = 3f;
    public bool isDead = false;
    public GameObject DustParticle;
    public bool isRock = false;

    private Animator _animatorChildren;
    private Animator _animator;
    private Transform _player;
    private Rigidbody2D _rb;
    private SpriteRenderer _sp;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _animatorChildren = GetComponentsInChildren<Animator>()[1];
        _animator = GetComponentsInChildren<Animator>()[0];
        _sp = GetComponentsInChildren<SpriteRenderer>()[0];
        _rb = GetComponent<Rigidbody2D>();
        StartCoroutine("DustParticleDelay");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            Dead();

            if (!isRock)
            {
                FlipByPlayerPosition();
            }

        }
    }
    private void FixedUpdate()
    {
        HpbarClaculate();
        if (!isDead)
        {
            //FollowPlayer();
        }
    }
    public void GetDamage(int damage)
    {
        if (isDead) return;

        if ((healthPoint - damage) <= 0)
        {
            healthPoint = 0;
        }
        else
        {
            healthPoint -= damage;
        }

        _animatorChildren.Play("HpBar");
        _animator.Play("Hurt");
        
    }
    public void Dead()
    {
        if (healthPoint <= 0)
        {
            isDead = true;
            _animator.Play("Dead");

            //Destroy(gameObject);
            //_rb.velocity = Vector3.zero;
            if (isRock)
            {
                Instantiate(Drop, gameObject.transform.position, Quaternion.identity);
                Instantiate(Drop, gameObject.transform.position, Quaternion.identity);
                Instantiate(Drop, gameObject.transform.position, Quaternion.identity);
                Instantiate(Drop, gameObject.transform.position, Quaternion.identity);

            }
            else
            {
                Instantiate(Drop, gameObject.transform.position, Quaternion.identity);
            }
            //Instantiate(Puf, gameObject.transform.position, Quaternion.identity);
            Destroy(GetComponent<CapsuleCollider2D>());
            Destroy(GetComponent<FlockingController>());
            Destroy(gameObject, 10f);
        }
    }
    private void HpbarClaculate()
    {
        Vector3 healthBarSize = new Vector3(healthPoint * 0.01f, 0.15f, 1);

        spriteRenderer.size = healthBarSize;//Vector3.Lerp(spriteRenderer.transform.localScale, healthBarSize, Time.deltaTime * 10f);
    }
    private void FlipByPlayerPosition()
    {
        var worldCursor = _player.transform.position;

        if (worldCursor.x > transform.position.x)
        {
            _sp.flipX = false;
        }
        if (worldCursor.x < transform.position.x)
        {
            _sp.flipX = true;
        }
    }
    IEnumerator DustParticleDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            if (_rb != null)
            {
                if (_rb.velocity != Vector2.zero)
                {
                    Instantiate(DustParticle, new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z), Quaternion.identity);
                }
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().HitPointRemove(damage);
        }
    }
}
