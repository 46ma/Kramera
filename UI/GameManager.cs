using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int targetsRemaining;

    //Scene setting
    [Header("Scene Settings")]
    public string nextSceneName = "";
    [SerializeField] float fadeDuration = 1.0f;
    [SerializeField] float typingSpeedEnd = 0.05f;

    //End game dialogue for chapther 4
    [Header("End Game Dialogue")]
    public CanvasGroup endScreenGroup;
    public TextMeshProUGUI endText;
    [TextArea(2, 5)]
    public List<string> finalDialogues;

    //For random typing sound pitch in game
    [Header("Audio Customization")]
    [Range(0f, 1f)] public float volume = 0.8f;
    [Range(0.5f, 1.5f)] public float minPitch = 0.9f;
    [Range(0.5f, 1.5f)] public float maxPitch = 1.1f;
    public AudioSource endTypingAudio;
    public AudioClip endTypingSound;

    //Check game state
    private bool isLevelEnding = false;

    void Awake()
    {
        if (instance == null) instance = this;
        if (endScreenGroup != null) endScreenGroup.alpha = 0; //Close end screen when start the game
    }

    private void Start()
    {
        targetsRemaining = FindObjectsOfType<TargetPoint>().Length;
    }

    //Check target in that scene
    public void TargetShootCompleted()
    {
        if (isLevelEnding) return;
        targetsRemaining--;

        if (targetsRemaining <= 0)
        {
            isLevelEnding = true;
            StartCoroutine(EndLevelSequence());
        }
    }

    //When level end
    private IEnumerator EndLevelSequence()
    {
        // wait for image and text
        if (PhotoDisplayManager.instance != null)
        {
            while (PhotoDisplayManager.instance.isTyping) yield return null;
        }
        yield return new WaitForSeconds(1.5f);

        // Fade in 
        if (endScreenGroup != null)
        {
            float timer = 0;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                endScreenGroup.alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
                yield return null;
            }
            endScreenGroup.alpha = 1;
        }

        //Sequential text delivery from a string list.
        foreach (string message in finalDialogues)
        {
            yield return StartCoroutine(TypeMessage(message));

            //Wait for player clicked
            bool hasClicked = false;
            while (!hasClicked)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                {
                    hasClicked = true;
                }
                yield return null;
            }
        }

        //Change scene if read all text
        yield return new WaitForSeconds(0.5f);

        //When finsihed use in MainMenu
        PlayerPrefs.SetInt("GameFinished", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(nextSceneName);
    }

    //Typing effect and sound
    private IEnumerator TypeMessage(string message)
    {
        endText.text = "";
        bool isTag = false;

        foreach (char letter in message.ToCharArray())
        {
            //For skip color rich text
            if (letter == '<') isTag = true;
            endText.text += letter;
            if (letter == '>') isTag = false;

            if (!isTag && letter != ' ')
            {
                //Random pich for natrue sound
                if (endTypingAudio != null && endTypingSound != null)
                {
                    endTypingAudio.pitch = Random.Range(minPitch, maxPitch);
                    endTypingAudio.PlayOneShot(endTypingSound);
                }
                yield return new WaitForSeconds(typingSpeedEnd);
            }
        }
    }
}