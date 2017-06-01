using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusicLevel : MonoBehaviour {

    public AudioClip m_clip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (AudioManager.Instance())
            {
                AudioManager.Instance().PlayMusic(m_clip, 4.0f);
            }
        }
    }
}
