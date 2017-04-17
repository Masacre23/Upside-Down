using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSpawnPlayer : MonoBehaviour {
	public Transform p1;
	public Transform p2;
	public Transform p3;
	public Transform p4;

	void Start () {
		
	}

	void Update () {
		if (Input.GetKey (KeyCode.Alpha1)) 
		{
			Spawn (p1);
		}
		if (Input.GetKey (KeyCode.Alpha2)) 
		{
			Spawn (p2);
		}
		if (Input.GetKey (KeyCode.Alpha3)) 
		{
			Spawn (p3);
		}
		if (Input.GetKey (KeyCode.Alpha4)) 
		{
			Spawn (p4);
		}
        /*if (Input.GetKey(KeyCode.Alpha5))
        {*/
            if (!GetComponent<Player>().m_alive)
            {
                Player player = GetComponent<Player>();
                Spawn(player.m_checkPoint);
                player.m_alive = true;
                player.m_health = player.m_maxHealth;
            }
        //}
        
    }

	public void Spawn(Transform tr)
	{
		transform.position = tr.position;
		transform.rotation = tr.rotation;
		this.GetComponent<Player> ().Start ();
	}
}
