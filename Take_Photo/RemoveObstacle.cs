using UnityEngine;

public class RemoveObstacle : MonoBehaviour,IClickable
{
    //Audio setting
    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip removeSound;
    [Range(0f, 1f)]
    [SerializeField] private float volume = 1.0f;

    public void OnClick()
    {
        if (removeSound != null && audioSource != null)
        {
            //Play audio
            audioSource.PlayOneShot(removeSound, volume);

            //Close sprite in obstacle and close obstacle collider
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            //Destroy this obstacle after sound
            Destroy(gameObject, removeSound.length);
        }
        else
        {
            //Destroy if no audio
            Destroy(gameObject);
        }
    }
}
