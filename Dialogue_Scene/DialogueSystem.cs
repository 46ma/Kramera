using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    //UI
    [Header("Main Dialogue UI")]
    public TextMeshProUGUI dialogueText;
    public CanvasGroup dialogueCanvasGroup;

    //Dialogue
    [Header("Dialogue Settings")]
    [TextArea(3, 10)]
    public List<string> dialogueLines;
    public string nextSceneName;
    public float typingSpeed = 0.05f;

    //Audio
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip typingSound;
    [Range(0.8f, 1.2f)] public float minPitch = 0.9f;
    [Range(0.8f, 1.2f)] public float maxPitch = 1.1f;

    //Final Transition
    [Header("Final Transition UI")]
    public CanvasGroup finalBlackScreenGroup;
    public TextMeshProUGUI finalTextDisplay;
    [TextArea(2, 5)]
    public string lastMessageBeforeSceneChange;

    //Text and dialogue setting
    private int currentIndex = 0;
    private bool isTyping = false;
    private string currentFullText;
    //Transition
    private bool isTransitioning = false;
    
    void Start()
    {
        if (dialogueCanvasGroup != null) dialogueCanvasGroup.alpha = 0; //Close the dialoue UI when start the game
        if (finalBlackScreenGroup != null)
        {
            finalBlackScreenGroup.alpha = 0; //Close the transition screen when start the game
            finalBlackScreenGroup.gameObject.SetActive(true);
        }

        StartCoroutine(StartScene());
    }

    //Call fade effect
    IEnumerator StartScene()
    {
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(FadeCanvas(1));
        if (dialogueLines.Count > 0)
        {
            StartCoroutine(TypeText(dialogueLines[currentIndex]));
        }
    }

    void Update()
    {
        if (isTransitioning) return;

        //When player clicked or press spacebar
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                StopAllCoroutines(); //Stop current typing effect
                dialogueText.text = currentFullText; //Make full text
                isTyping = false;
            }
            else
            {
                AdvanceDialogue();
            }
        }
    }

    void AdvanceDialogue()
    {
        currentIndex++;
        if (currentIndex < dialogueLines.Count)
        {
            StartCoroutine(TypeText(dialogueLines[currentIndex]));
        }
        else
        {
            StartCoroutine(FinalSequence());
        }
    }
    IEnumerator FinalSequence()
    {
        isTransitioning = true;

        //Hide dilogue UI
        if (dialogueCanvasGroup != null)
            yield return StartCoroutine(FadeCanvas(dialogueCanvasGroup, 0, 1f));

        //Show black screen
        if (finalBlackScreenGroup != null)
        {
            //Clear old text
            finalTextDisplay.text = "";
            finalBlackScreenGroup.transform.SetAsLastSibling();

            //After black screen fade in start typing effect
            yield return StartCoroutine(FadeCanvas(finalBlackScreenGroup, 1, 1.5f));
            if (!string.IsNullOrEmpty(lastMessageBeforeSceneChange))
            {
                bool isTag = false;
                foreach (char letter in lastMessageBeforeSceneChange.ToCharArray())
                {
                    //Skip color rich text
                    if (letter == '<') isTag = true;
                    finalTextDisplay.text += letter;
                    if (letter == '>') isTag = false;

                    if (!isTag && letter != ' ')
                    {
                        if (audioSource != null && typingSound != null)
                        {
                            //Random pitch for nature typing sound
                            audioSource.pitch = Random.Range(minPitch, maxPitch);
                            audioSource.PlayOneShot(typingSound);
                        }
                        yield return new WaitForSeconds(typingSpeed);
                    }
                }
            }
        }

        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(nextSceneName);
    }

    //Fade effect
    IEnumerator FadeCanvas(CanvasGroup cg, float targetAlpha, float duration)
    {
        if (cg == null) yield break;
        float startAlpha = cg.alpha;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }
        cg.alpha = targetAlpha;
    }

    //Typing effect
    IEnumerator TypeText(string text)
    {
        isTyping = true;
        currentFullText = text;
        dialogueText.text = "";

        bool isTag = false;
        foreach (char letter in text.ToCharArray())
        {
            if (letter == '<') isTag = true;
            dialogueText.text += letter;
            if (letter == '>') isTag = false;

            if (!isTag && letter != ' ')
            {
                if (audioSource != null && typingSound != null)
                {
                    audioSource.PlayOneShot(typingSound);
                }
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        isTyping = false;
    }

    IEnumerator FadeCanvas(float targetAlpha)
    {
        float speed = 2f;
        while (!Mathf.Approximately(dialogueCanvasGroup.alpha, targetAlpha))
        {
            dialogueCanvasGroup.alpha = Mathf.MoveTowards(dialogueCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }
    }

    /*
    IEnumerator EndDialogue()
    {
        isTransitioning = true;
        yield return StartCoroutine(FadeCanvas(0));
        SceneManager.LoadScene(nextSceneName);
    }
    */
}