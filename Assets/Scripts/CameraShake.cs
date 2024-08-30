using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    public void Shake()
    {
        animator.Play("CameraShake");
    }
}
