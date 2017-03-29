using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    Animator animator;
    public float h;
    public float v;
    public bool run = false;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if(Input.GetButtonDown("Jump"))
        {
            run = true;
        }
        if(h == 0 && v == 0)
            animator.SetFloat("Vel", 0);
        else
            animator.SetFloat("Vel", 1);
        animator.SetBool("Sprint", run);
    }
}
