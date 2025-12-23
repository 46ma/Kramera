using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PhotoInventoryManager : MonoBehaviour
{
    public static PhotoInventoryManager instance;

    //Photo slots
    [Header("Photo Slots")]
    public List<Image> photoSlots;
    private int currentPhotoCount = 0;

    void Awake()
    {
        if (instance == null) instance = this;

        /*
        foreach (var slot in photoSlots)
        {
            if (slot != null) slot.enabled = false;
        }
        */
    }

    //Add photo to inventory
    public void AddPhotoToInventory(Sprite photoSprite)
    {
        if (currentPhotoCount < photoSlots.Count)
        { 
            photoSlots[currentPhotoCount].sprite = photoSprite; //Add sprite image in photoSlots
            photoSlots[currentPhotoCount].enabled = true; //Show image

            currentPhotoCount++;
        }
    }
}