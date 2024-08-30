using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [HideInInspector] public Vector3 worldCursor;
    private bool _flag = true;
    private float _timer;
    private Animator _anim;
    //private ShakeCameraEffect shakeCamera;

    public GameObject trail;
    public LayerMask target;
    public Transform damagePoition;
    public float delay;
    public int damage;
    public float range;
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //shakeCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ShakeCameraEffect>();
        AttackAnim();
        _timer -= Time.fixedDeltaTime;
    }
    public void OnDamage()
    {
        Collider2D[] Resources = Physics2D.OverlapCircleAll(damagePoition.position, range, target);
        for (int i = 0; i < Resources.Length && Resources[i] != null; i++)
        {
            //shakeCamera.Shake();
            //Resources[i].GetComponent<RockManagaer>().TakeStrength(damage);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(damagePoition.position, range);
    }
    private void AttackAnim()
    {

        if (Input.GetKey(KeyCode.Mouse0) && _timer < 0f)
        {
            if (_flag)
            {
                _anim.Play("AttackPong");
            }
            else
            {
                _anim.Play("AttackPing");
            }
            _flag = !_flag;
            _timer = delay;
        }
    }
    public void ParticleTral()
    {
        if (trail.activeSelf)
        {
            trail.SetActive(false);
        }
        else
        {
            trail.SetActive(true);
        }
    }
}
