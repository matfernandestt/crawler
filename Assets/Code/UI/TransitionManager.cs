using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;
    
    [SerializeField] private Image fadeImage;

    private Coroutine _transitionRoutine;

    private void Awake()
    {
        if (Instance == null)
        {
            transform.SetParent(null);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Fade(Action onFaded, Action onCompleted)
    {
        StartCoroutine(FadeInAndOut(onFaded, onCompleted));
    }

    private IEnumerator FadeInAndOut(Action onFaded,  Action onCompleted)
    {
        yield return StartCoroutine(TransitionRoutine(true));
        onFaded?.Invoke();
        yield return StartCoroutine(TransitionRoutine(false));
        onCompleted?.Invoke();
    }

    private IEnumerator TransitionRoutine(bool fadeToBlack)
    {
        fadeImage.fillClockwise = fadeToBlack;
        var start = fadeToBlack ? 0f : 1f;
        var end = fadeToBlack ? 1f : 0f;
        
        var progress = 0f;
        while (progress < 1f)
        {
            progress += Time.deltaTime * 2f;
            yield return null;
            fadeImage.fillAmount = Mathf.Lerp(start, end, progress);
        }
        fadeImage.fillAmount = end;
    }
}
