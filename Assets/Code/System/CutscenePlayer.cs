using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CutscenePlayer : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private float percentageBeforeTransition;

    private bool _finished;

    private void Awake()
    {
        videoPlayer.time = 0;
        videoPlayer.Play();
    }

    private void Update()
    {
        if (_finished) return;
        if (videoPlayer.time >= videoPlayer.clip.length - (percentageBeforeTransition / 100f))
        {
            _finished = true;
            TransitionManager.Instance.Fade(()=>{SceneManager.LoadScene(nextSceneName);},()=>{});
        }
    }
}
