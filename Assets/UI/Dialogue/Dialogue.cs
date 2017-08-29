﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Dialogue : MonoBehaviour
{
    private Text _textComponent;

    public string[] DialogueStrings;
	public int[] indexCameras;
	public Camera[] DialogueCamera;
	bool activeCameras = false;

    public float SecondsBetweenCharacters = 0.15f;
    public float CharacterRateMultiplier = 0.5f;

    //public KeyCode DialogueInput = KeyCode.Return;
    public string m_inputNameButton = "Activate";

    private bool _isStringBeingRevealed = false;
    private bool _isDialoguePlaying = false;
    private bool _isEndOfDialogue = false;

    public GameObject ContinueIcon;
    public GameObject StopIcon;

    Player playerManager;

    //this.gameObject.transform.parent.gameObject.SetActive(false);
    // playerManager.m_negatePlayerInput = false;
    // Use this for initialization
	public void Start()
	{
		_textComponent = GetComponent<Text>();
		_textComponent.text = "";

		HideIcons();

		playerManager = GameObject.Find("Player").GetComponent<Player>();
		_isDialoguePlaying = true;
		StartCoroutine(StartDialogue());

		if (DialogueCamera.Length != 0)
			activeCameras = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
	   /* if (Input.GetKeyDown(KeyCode.Return))
	    //{
	        if (!_isDialoguePlaying)
	        {
                _isDialoguePlaying = true;
                StartCoroutine(StartDialogue());
            }
	        
	    }*/
	}

    private IEnumerator StartDialogue()
    {
        int dialogueLength = DialogueStrings.Length;
        int currentDialogueIndex = 0;

        while (currentDialogueIndex < dialogueLength || !_isStringBeingRevealed)
        {
            if (!_isStringBeingRevealed)
            {
                _isStringBeingRevealed = true;
				if (activeCameras) 
				{
					DialogueCamera [indexCameras [currentDialogueIndex - 1]].gameObject.SetActive (false);
					DialogueCamera [indexCameras [currentDialogueIndex]].gameObject.SetActive (true);
				}

                StartCoroutine(DisplayString(DialogueStrings[currentDialogueIndex++]));

                if (currentDialogueIndex >= dialogueLength)
                {
                    _isEndOfDialogue = true;
                }
            }

            yield return 0;
        }

        while (true)
        {
            //if (Input.GetKeyDown(DialogueInput))
            if (!playerManager.m_paused && Input.GetButtonDown(m_inputNameButton))
            {
				
                break;
            }

            yield return 0;
        }

		DialogueCamera [indexCameras [currentDialogueIndex - 1]].gameObject.SetActive (false);
        HideIcons();
        _isEndOfDialogue = false;
        _isDialoguePlaying = false;
        this.gameObject.transform.parent.gameObject.SetActive(false);

        playerManager.m_negatePlayerInput = false;
        //playerManager.m_negateJump = false;
    }

    private IEnumerator DisplayString(string stringToDisplay)
    {
        int stringLength = stringToDisplay.Length;
        int currentCharacterIndex = 0;

        HideIcons();

        _textComponent.text = "";

        while (currentCharacterIndex < stringLength)
        {
            _textComponent.text += stringToDisplay[currentCharacterIndex];
            currentCharacterIndex++;

            if (currentCharacterIndex < stringLength)
            {
               /* if (Input.GetKey(DialogueInput))
                {
                    yield return new WaitForSeconds(SecondsBetweenCharacters*CharacterRateMultiplier);
                }
                else*
                {*/
                    yield return new WaitForSeconds(SecondsBetweenCharacters);
               // }
            }
            else
            {
                break;
            }
        }

        ShowIcon();

        while (true)
        {
            //if (Input.GetKeyDown(DialogueInput))
            if (!playerManager.m_paused && Input.GetButtonDown(m_inputNameButton))
            {
                break;
            }

            yield return 0;
        }

        HideIcons();

        _isStringBeingRevealed = false;
        _textComponent.text = "";
    }

    private void HideIcons()
    {
        ContinueIcon.SetActive(false);
        StopIcon.SetActive(false);
    }

    private void ShowIcon()
    {
        if (_isEndOfDialogue)
        {
            StopIcon.SetActive(true);
            return;
        }

        ContinueIcon.SetActive(true);
    }

    public bool DialogueHasEnded()
    {
        return _isEndOfDialogue;
    }
}
