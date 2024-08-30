using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static UnityEngine.GraphicsBuffer;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage = 0;
    public float speed = 0;
    public float range = 0;
    public int level;
    public GameObject projectTile = null;
    public LayerMask enemyLayer;
    public Transform damagePosition;

    private CameraShake _cameraShake;
    private bool _flag = true;
    private float _timer;
    private Animator _anim;
    
    void Start()
    {
        _anim = GetComponent<Animator>();
        _cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        AttackAnim();
        _timer -= Time.fixedDeltaTime;
    }

    public void OnAttack()
    {
        Collider2D[] Enemy = Physics2D.OverlapCircleAll(damagePosition.position, range, enemyLayer);
        for (int i = 0; i < Enemy.Length && Enemy[i] != null; i++)
        {
            //shakeCamera.Shake();
            Enemy[i].GetComponent<Enemy>().GetDamage(damage);
            _cameraShake.Shake();
        }
    }
    private void AttackAnim()
    {
        //float temp = 1 * ((speed*10) / 100);
        //Debug.Log($"asd{temp}");
        //_anim.speed = 0.2f;

        if (Input.GetKey(KeyCode.Mouse0) && _timer < 0f)
        {
            if (_flag)
            {
                _anim.Play("AttackPing");
            }
            else
            {
                _anim.Play("AttackPong");
            }
            _flag = !_flag;
            _timer = speed;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(damagePosition.position, range);
    }
}
