using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashController : MonoBehaviour
{
    public CanvasGroup flashGroup;
    public AudioSource cameraShutter;
    [SerializeField] float flashDuration = 0.2f;

    public static FlashController Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void StartFlashSequence(Sprite photo, string message)
    {
        if (cameraShutter != null) cameraShutter.Play();
        StopAllCoroutines();
        StartCoroutine(FlashAndThenShow(photo, message));
    }

    //Make flash effect and show photo and message on display
    private IEnumerator FlashAndThenShow(Sprite photo, string message)
    {
        //Send photo to PhotoDisplay script
        if (PhotoDisplayManager.instance != null)
            PhotoDisplayManager.instance.ShowPhoto(photo, message);

        //Send photo to PhotoInventoryManager script
        if (PhotoInventoryManager.instance != null)
            PhotoInventoryManager.instance.AddPhotoToInventory(photo);

        //Flash Effect
        flashGroup.alpha = 1f;
        float timer = 0f;
        while (timer < flashDuration)
        {
            flashGroup.alpha = Mathf.Lerp(1f, 0f, timer / flashDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        flashGroup.alpha = 0f;
    }
}