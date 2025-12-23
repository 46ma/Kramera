using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WarningScreen : MonoBehaviour
{
    public float displayTime = 3.0f; //Display time
    public string nextSceneName = "MainMenu";
    public CanvasGroup warningGroup;

    void Start()
    {
        StartCoroutine(WarningSequence());
    }

    //Fade in effect
    IEnumerator WarningSequence()
    {
        yield return StartCoroutine(Fade(0, 1, 1f));

        float timer = 0;
        while (timer < displayTime)
        {
            //If player clicked then skip warning screen
            if (Input.GetMouseButtonDown(0)) break;
            timer += Time.deltaTime;
            yield return null;
        }

        //Fade out
        yield return StartCoroutine(Fade(1, 0, 1f));

        //Load next scene
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator Fade(float start, float end, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            warningGroup.alpha = Mathf.Lerp(start, end, time / duration);
            yield return null;
        }
        warningGroup.alpha = end;
    }
}