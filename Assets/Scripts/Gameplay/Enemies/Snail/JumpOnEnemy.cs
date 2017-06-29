using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpOnEnemy : MonoBehaviour {
	public Enemy enemy;
	void OnCollisionEnter(Collision col)
	{
		/*if (col.gameObject.tag == "Player" && enemy.m_animator.GetCurrentAnimatorStateInfo (0).IsName ("Walk")) 
		{
			transform.parent.parent.GetComponent<Enemy> ().m_animator.SetBool ("Stunned", true);
			Debug.Log ("Yolo");
		}*/
		//if (col.gameObject.tag == "Player") 
		//{
		//	enemy.m_animator.SetBool ("Stunned", true);
			Debug.Log ("Yolo");
		//}
	}
}
