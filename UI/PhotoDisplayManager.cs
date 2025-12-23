using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhotoDisplayManager : MonoBehaviour
{
    public static PhotoDisplayManager instance;

    //UI Panels
    [Header("UI Panels")]
    public GameObject photoDisplayPanel;
    public GameObject dialogueBoxPanel;

    //UI Elements
    [Header("UI Elements")]
    public Image photoImage;
    public TextMeshProUGUI dialogueText;

    //Audio
    [Header("Audio")]
    public AudioSource typingAudioSource;
    [SerializeField] AudioClip typingSound;

    //Testing photo display time and typing speed for typing effect
    [Header("Settings")]
    [SerializeField] float photoDisplayTime = 2f;
    [SerializeField] float typingSpeed = 0.05f;

    public bool isTyping { get; private set; }

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        if (photoDisplayPanel != null) photoDisplayPanel.SetActive(false);
        //if (dialogueBoxPanel != null) dialogueBoxPanel.SetActive(false);
    }

    //Show  photo in the targetPonit afther shoot
    public void ShowPhoto(Sprite photo, string message)
    {
        if (photoImage != null)
        {
            photoImage.sprite = photo;
        }

        //Show photo and text
        photoDisplayPanel.SetActive(true);
        dialogueBoxPanel.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(ProcessPhotoAndText(message));
    }

    private IEnumerator ProcessPhotoAndText(string message)
    {
        isTyping = true;
        bool isTag = false;
        dialogueText.text = "";

        foreach (char letter in message.ToCharArray())
        {
            //Skip color rich text
            if (letter == '<') isTag = true;
            dialogueText.text += letter;
            if (letter == '>') isTag = false;
            if (isTag) continue;

            //Audio
            if (typingAudioSource != null && typingSound != null && letter != ' ')
            {
                typingAudioSource.pitch = Random.Range(0.9f, 1.2f); //Random pitch for natrue typing sound
                typingAudioSource.PlayOneShot(typingSound, 0.5f);
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        yield return new WaitForSeconds(photoDisplayTime);
        photoDisplayPanel.SetActive(false);
    }
}