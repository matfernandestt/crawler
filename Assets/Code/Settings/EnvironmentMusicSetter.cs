using System;
using UnityEngine;

public class EnvironmentMusicSetter : MonoBehaviour
{
    [SerializeField] private AudioClip music;
    
    private void OnEnable()
    {
        AudioManager.Instance.PlayMusic(music);
    }
}
