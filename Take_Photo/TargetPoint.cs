using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPoint : MonoBehaviour, IClickable
{
    //Collider
    private Collider2D targetCollider;
    private SpriteRenderer targetRenderer;

    //For showing photo after player shoot
    public Sprite photoResult;

    //For showing dialogue after player shoot photo
    [TextArea(3, 10)]
    [SerializeField] string dialogueAfterShoot;

    private void Start()
    {
        targetRenderer = GetComponent<SpriteRenderer>();
        targetCollider = GetComponent<Collider2D>();
    }

    public void OnClick()
    {
        OnTargetShoot();
    }
    public void OnTargetShoot()
    {
        //Close target collider and sprite after clicked
        if (targetRenderer != null) targetRenderer.enabled = false;
        if (targetCollider != null) targetCollider.enabled = false;

        //Call FlashController
        if (FlashController.Instance != null && photoResult != null)
        {
            //Send photo and image to FlashController
            FlashController.Instance.StartFlashSequence(photoResult, dialogueAfterShoot);
        }

        //Update to GameManager when shoot picture
        if (GameManager.instance != null)
        {
            GameManager.instance.TargetShootCompleted();

        }
    }
}
