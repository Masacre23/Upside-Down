using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBehaviour : MonoBehaviour {

    enum RobotState
    {
        STOP,
        MOVING
    }

    [System.Serializable]
    struct RobotPositionsInfo
    {
        public Transform m_position;
        public string m_showingButton;
        public GameObject m_dialog;
        public TriggerPlatform m_changeTrigger;
        public float m_transitionTime;
        public bool m_autoExitState;
    }

    [SerializeField] RobotPositionsInfo[] m_behaviour;

    ButtonControlHelper m_buttons;
    DialogueZone m_dialogZone;
    int m_currentBehaviour = 0;
    int m_finalBehaviour = 0;

    RobotState m_state = RobotState.STOP;

    Vector3 m_initialPosition;
    Vector3 m_finalPosition;
    Quaternion m_initialRotation;
    Quaternion m_finalRotation;

    float m_changeTime = 0.0f;
    float m_maxChangeTime;

    Collider m_objectCollider;

    void Awake()
    {
        m_buttons = GetComponentInChildren<ButtonControlHelper>();
        m_dialogZone = GetComponentInChildren<DialogueZone>();
        m_dialogZone.enabled = false;

        m_objectCollider = GetComponent<SphereCollider>();  
    }

    // Use this for initialization
    void Start ()
    {
        SetBehaviour(m_currentBehaviour);
	}
	
	// Update is called once per frame
	void Update ()
    {
        switch (m_state)
        {
            case RobotState.STOP:
                if (m_behaviour[m_currentBehaviour].m_autoExitState)
                {
                    SetChanging(m_currentBehaviour + 1);
                }
                else
                {
                    //Change location if dialog (if any) is over
                    if (m_behaviour[m_currentBehaviour].m_dialog)
                    {
                        if (!m_dialogZone.enabled)
                            SetChanging(m_currentBehaviour + 1);
                    }

                    //See if any platform for change at higher step has been reached
                    int changeToStep = m_currentBehaviour;
                    bool change = false;
                    for (int i = m_currentBehaviour; i < m_behaviour.Length; i++)
                    {
                        if (m_behaviour[i].m_changeTrigger && m_behaviour[i].m_changeTrigger.m_playerDetected)
                        {
                            changeToStep = i + 1;
                            change = true;
                        }
                    }
                    if (change)
                        SetChanging(changeToStep);
                }
                break;
            case RobotState.MOVING:
                m_changeTime += Time.deltaTime;
                if (m_changeTime >= m_maxChangeTime)
                {
                    if (m_finalBehaviour == m_currentBehaviour)
                        EndChanging(m_currentBehaviour);
                    else
                        SetChanging(m_finalBehaviour); 
                }
                else
                {
                    float perc = m_changeTime / m_maxChangeTime;
                    transform.position = Vector3.Lerp(m_initialPosition, m_finalPosition, perc);
                    transform.rotation = Quaternion.Lerp(m_initialRotation, m_finalRotation, perc);
                }
                break;
            default:
                break;
        }
        
	}

    public void SetChanging(int step)
    {
        m_finalBehaviour = step;
        ++m_currentBehaviour;

        m_changeTime = 0.0f;
        m_maxChangeTime = m_behaviour[m_currentBehaviour].m_transitionTime;

        m_initialPosition = transform.position;
        m_initialRotation = transform.rotation;

        m_finalPosition = m_behaviour[m_currentBehaviour].m_position.position;
        m_finalRotation = m_behaviour[m_currentBehaviour].m_position.rotation;

        m_buttons.UnsetImage();

        m_objectCollider.enabled = false;

        m_state = RobotState.MOVING;
    }

    public void EndChanging(int step)
    {
        SetBehaviour(step);

        m_objectCollider.enabled = true;

        m_state = RobotState.STOP;
    }

    public void SetBehaviour(int step)
    {
        transform.position = m_behaviour[step].m_position.position;
        transform.rotation = m_behaviour[step].m_position.rotation;

        m_buttons.SetImage(m_behaviour[step].m_showingButton);
        if (m_behaviour[step].m_dialog)
        {
            m_dialogZone.enabled = true;
            m_dialogZone.SetNewDialog(m_behaviour[step].m_dialog);
        }
        else
            m_dialogZone.enabled = false;

    }
}
