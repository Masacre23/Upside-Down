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
		h = Mathf.Abs(Input.GetAxis("Horizontal"));
		v = Mathf.Abs(Input.GetAxis("Vertical"));

      /*  if(Input.GetButtonDown("Jump"))
        {
            run = true;
        }*/
    
		float vel = h + v;
		animator.SetFloat ("Vel", vel);

        //animator.SetBool("Sprint", run);
    }
}
