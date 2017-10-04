using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Text))]
public class Dialogue : MonoBehaviour
{
    private Text _textComponent;

	//public int bossEvent = -1; //if -1 means no event
    //public int kingEvent = -1;

    public enum eventType { BOSS, SNOWMAN, ENABLEDISABLE};
    [System.Serializable]
    public struct Event
    {
        public eventType etype;
        public string name;
        public GameObject GOtoEnable;
        public string[] animationVariable;
        public int[] index;
        public int counter;
    }

    public Event[] dialogueEvent;
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

	public bool spanish = false;

    // Use this for initialization
	public void Start()
	{
		_textComponent = GetComponent<Text>();
		_textComponent.text = "";

		HideIcons();

		playerManager = GameObject.Find("Player").GetComponent<Player>();
		_isDialoguePlaying = true;
        if (GameObject.Find("Data"))
        {
            spanish = GameObject.Find("Data").GetComponent<Data>().spanish;
        }

        if (dialogueEvent.Length > 0)
        {
            for (int i = 0; i < dialogueEvent.Length; i++)
            {
                if (dialogueEvent[i].index[0] == 0)
                {
                    switch (dialogueEvent[i].etype)
                    {
                        case eventType.BOSS:
                            GameObject.Find("Boss").GetComponent<BossScene>().enabled = true;
                            break;

                        case eventType.SNOWMAN:
                            Animator anim = GameObject.Find(dialogueEvent[i].name).GetComponent<Animator>();
                            anim.SetBool(dialogueEvent[i].animationVariable[dialogueEvent[i].counter], !anim.GetBool(dialogueEvent[i].animationVariable[dialogueEvent[i].counter]));
                            break;

                        case eventType.ENABLEDISABLE:
                            dialogueEvent[i].GOtoEnable.SetActive(!dialogueEvent[i].GOtoEnable.activeInHierarchy);
                            break;
                        default:
                            break;
                    }
                    if (dialogueEvent[i].counter != dialogueEvent[i].index.Length - 1)
                        dialogueEvent[i].counter++;
                }
            }
        }

        StartCoroutine(StartDialogue());

		if (DialogueCamera.Length != 0)
			activeCameras = true;

		if (startEvent)
			startEvent.SetActive (true);
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
                    DialogueCamera[indexCameras[currentDialogueIndex - 1]].gameObject.SetActive(false);
                    DialogueCamera[indexCameras[currentDialogueIndex]].gameObject.SetActive(true);
                    for (int i = 0; i < dialogueEvent.Length; i++)
                    {
                        if (dialogueEvent[i].index[dialogueEvent[i].counter] == currentDialogueIndex)
                        {
                            switch (dialogueEvent[i].etype)
                            {
                                case eventType.BOSS:
                                    GameObject.Find("Boss").GetComponent<BossScene>().enabled = true;
                                    break;

                                case eventType.SNOWMAN:
                                    Animator anim = GameObject.Find(dialogueEvent[i].name).GetComponent<Animator>();
                                    anim.SetBool(dialogueEvent[i].animationVariable[dialogueEvent[i].counter], !anim.GetBool(dialogueEvent[i].animationVariable[dialogueEvent[i].counter]));
                                    break;

                                case eventType.ENABLEDISABLE:
                                    dialogueEvent[i].GOtoEnable.SetActive(!dialogueEvent[i].GOtoEnable.activeInHierarchy);
                                    break;

                                default:
                                    break;
                            }
                            if (dialogueEvent[i].counter != dialogueEvent[i].index.Length - 1)
                               dialogueEvent[i].counter++;
                        }
                        //    if (dialogueEvent[i].counter != dialogueEvent[i].index.Length - 1)
                        // if (dialogueEvent[i].counter != dialogueEvent[i].index.Length - 1)
                        //   dialogueEvent[i].counter++;
                    }
                }


                if (!spanish)
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

        playerManager.m_negatePlayerInput = false;

        if (dialogueEvent.Length > 0)
        {
            for (int i = 0; i < dialogueEvent.Length; i++)
            {
                //if(dialogueEvent[i].counter == 0 && dialogueEvent[i].etype != eventType.BOSS)
                if (dialogueEvent[i].counter == 0 && dialogueEvent[i].index.Length != 1)
                    dialogueEvent[i].counter++;
                if (dialogueEvent[i].index[dialogueEvent[i].counter] == currentDialogueIndex)
                {
                    switch (dialogueEvent[i].etype)
                    {
                        case eventType.BOSS:
                            GameObject.Find("Boss").GetComponent<BossScene>().enabled = true;
                            break;

                        case eventType.SNOWMAN:
                            Animator anim = GameObject.Find(dialogueEvent[i].name).GetComponent<Animator>();
                            anim.SetBool(dialogueEvent[i].animationVariable[dialogueEvent[i].counter], !anim.GetBool(dialogueEvent[i].animationVariable[dialogueEvent[i].counter]));
                            break;

                        case eventType.ENABLEDISABLE:
                            dialogueEvent[i].GOtoEnable.SetActive(!dialogueEvent[i].GOtoEnable.activeInHierarchy);
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        for (int i = 0; i < dialogueEvent.Length; ++i)
        {
            //if (bossEvent != -1)
            if (dialogueEvent[i].etype == eventType.BOSS)
            {
                GameObject.Find("Boss").GetComponent<BossScene>().dialogueFinished = true;
                playerManager.m_negatePlayerInput = true;
            }
                //SceneManager.LoadScene (2);
        }
        HideIcons();
        _isEndOfDialogue = false;
        _isDialoguePlaying = false;
		_isStringBeingRevealed = false;

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
                if (Input.GetButton(m_inputNameButton))
                {
              //      Debug.Log("YOLOOOOO");
                    // yield return new WaitForSeconds(SecondsBetweenCharacters*CharacterRateMultiplier);
                    yield return 0;
                }
                else
                {
                    yield return new WaitForSeconds(SecondsBetweenCharacters);
                }
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
