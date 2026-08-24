using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSrc;
    [SerializeField] private AudioSource sfxSrc;
    
    public static AudioManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSrc.clip = clip;
        musicSrc.Play();
    }

    public void PlaySfx(AudioClip clip)
    {
        sfxSrc.clip = clip;
        sfxSrc.Play();
    }

    public void StopMusic()
    {
        musicSrc.Stop();
    }
}
