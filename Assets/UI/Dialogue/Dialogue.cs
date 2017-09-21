using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Text))]
public class Dialogue : MonoBehaviour
{
    private Text _textComponent;

	public int bossEvent = -1; //if -1 means no event
    public string[] DialogueStrings;
	public string[] DialogueStringsES;
	public int[] indexCameras;
	public Camera[] DialogueCamera;
	public bool activeCameras = false;

    public float SecondsBetweenCharacters = 0.15f;
	public float CharacterRateMultiplier = 0.5f;

    //public KeyCode DialogueInput = KeyCode.Return;
    public string m_inputNameButton = "Activate";

    private bool _isStringBeingRevealed = false;
	public bool _isDialoguePlaying = false;
    private bool _isEndOfDialogue = false;

    public GameObject ContinueIcon;
    public GameObject StopIcon;

    Player playerManager;
	public GameObject startEvent;
	public GameObject endEvent;

	bool spanish;

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

		if (startEvent)
			startEvent.SetActive (true);

		spanish = GameObject.Find ("Data").GetComponent<Data> ().spanish;
	}
		
	// Update is called once per frame
	void Update () 
	{
		/*if (Input.GetButtonDown(m_inputNameButton))
	    {
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
		_isDialoguePlaying = true;
        while (currentDialogueIndex < dialogueLength || !_isStringBeingRevealed)
        {
            if (!_isStringBeingRevealed)
            {
                _isStringBeingRevealed = true;
				if (activeCameras) 
				{
					DialogueCamera [indexCameras [currentDialogueIndex - 1]].gameObject.SetActive (false);
					DialogueCamera [indexCameras [currentDialogueIndex]].gameObject.SetActive (true);
					if (bossEvent == currentDialogueIndex)
						GameObject.Find ("Boss").GetComponent<BossScene> ().enabled = true;
				}

				if(!spanish)
                	StartCoroutine(DisplayString(DialogueStrings[currentDialogueIndex++]));
				else
					StartCoroutine(DisplayString(DialogueStringsES[currentDialogueIndex++]));

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
			//if (!playerManager.m_paused && Input.GetButtonDown(m_inputNameButton) && !_isStringBeingRevealed)
			if(!_isStringBeingRevealed)
            {
				
                break;
            }

            yield return 0;
        }
		if(activeCameras)
			DialogueCamera [indexCameras [currentDialogueIndex - 1]].gameObject.SetActive (false);
		if (bossEvent != -1)
			SceneManager.LoadScene (2);
        HideIcons();
        _isEndOfDialogue = false;
        _isDialoguePlaying = false;
		_isStringBeingRevealed = false;

        playerManager.m_negatePlayerInput = false;
		this.gameObject.transform.parent.gameObject.SetActive(false);
		if (endEvent)
			endEvent.SetActive (true);
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
				/*if (Input.GetButton(m_inputNameButton))
                {
                    yield return new WaitForSeconds(SecondsBetweenCharacters*CharacterRateMultiplier);
                }
                else
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
