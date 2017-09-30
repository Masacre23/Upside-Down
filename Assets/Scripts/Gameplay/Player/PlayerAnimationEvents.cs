using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    Player m_player;

    [Header("Snow on foot")]
    public GameObject m_prefabSnowOnFeet;
    public Transform m_leftFootTransform;
    public Transform m_rightFootTransform;

    [Header("Footprint Decal")]
    public GameObject m_leftFootprint;
    public GameObject m_rightFootprint;
    public Transform m_leftFootFrontTransform;
    public Transform m_leftFootBackTransform;
    public Transform m_rightFootFrontTransform;
    public Transform m_rightFootBackTransform;
    public LayerMask m_layersToFootprint;
    public float m_checkFloorDistance = 0.3f;
    public float m_distanceFromFloor = 0.025f;

    [Header("JumpCloud")]
    public GameObject m_smokeCloud;
    public Transform m_smokePosition;

    [Header("Recieve hit")]
    public GameObject m_hit;
    public Transform m_hitTransform;

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
        if (SetFootprint(m_leftFootprint, m_leftFootFrontTransform, m_leftFootBackTransform) && m_player.m_inputSpeed > 0.5)
        {
            EffectsManager.Instance.GetEffect(m_prefabSnowOnFeet, m_leftFootTransform);
        }
    }

    public void AnimationRightFootOnGround()
    {
        m_player.m_soundEffects.PlayFootStep(); 
        if (SetFootprint(m_rightFootprint, m_rightFootFrontTransform, m_rightFootBackTransform) && m_player.m_inputSpeed > 0.5)
        {
            EffectsManager.Instance.GetEffect(m_prefabSnowOnFeet, m_rightFootTransform);
        }
    }

    public void AnimationOnFalling()
    {
        EffectsManager.Instance.GetEffect(m_smokeCloud, m_smokePosition);
    }

    public void AnimationHit()
    {
        EffectsManager.Instance.GetEffect(m_hit, m_hitTransform);
    }

    private bool SetFootprint(GameObject prefab, Transform front, Transform back)
    {
        RaycastHit frontHit;
        RaycastHit backHit;
        bool frontIsHit = Physics.Raycast(front.position, -transform.up, out frontHit, m_checkFloorDistance, m_layersToFootprint);
        bool backIsHit = Physics.Raycast(back.position, -transform.up, out backHit, m_checkFloorDistance, m_layersToFootprint);

        if (frontIsHit && backIsHit)
        {
            Vector3 forward = frontHit.point - backHit.point;
            Vector3 middlePoint = backHit.point + forward / 2.0f;
            EffectsManager.Instance.GetEffect(prefab, middlePoint + transform.up * m_distanceFromFloor, transform.up, forward.normalized);
            return true;
        }

        return false;
    }
}
