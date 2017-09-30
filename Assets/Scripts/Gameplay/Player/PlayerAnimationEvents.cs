using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    Player m_player;
    public GameObject m_prefabSnowOnFeet;
    public Transform m_leftFootTransform;
    public Transform m_rightFootTransform;
    public GameObject m_leftFootprint;
    public GameObject m_rightFootprint;

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

    public void AnimationLeftFootOnGround()
    {
        m_player.m_soundEffects.PlayFootStep();
        if (m_player.m_inputSpeed > 0.5)
            EffectsManager.Instance.GetEffect(m_prefabSnowOnFeet, m_leftFootTransform);
    }

    public void AnimationRightFootOnGround()
    {
        m_player.m_soundEffects.PlayFootStep();
        if (m_player.m_inputSpeed > 0.5)
            EffectsManager.Instance.GetEffect(m_prefabSnowOnFeet, m_rightFootTransform);
    }
}
