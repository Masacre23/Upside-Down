using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    Player m_player;

	// Use this for initialization
	void Start ()
    {
        m_player = GetComponent<Player>();
	}

    public void AnimationClearThrow()
    {
        m_player.m_throwAnimation = false;
    }

    public void AnimationThrowObject()
    {
        m_player.m_pickedObject.ThrowObjectNow();
    }

    public void AnimationFeetOnGround()
    {
        m_player.m_soundEffects.PlayFootStep();
    }
}
