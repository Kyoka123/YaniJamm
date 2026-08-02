using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Serializable]
    public struct Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume;
        [Range(0.1f, 3f)] public float pitch;
    }

    public Sound[] sounds;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySound(string soundName)
    {
        Sound s = Array.Find(sounds, item => item.name == soundName);

        if (s.clip == null)
        {
            Debug.LogWarning("Ses bulunamadi: " + soundName);
            return;
        }

        audioSource.pitch = s.pitch <= 0 ? 1f : s.pitch;
        float vol = s.volume <= 0 ? 1f : s.volume;

        audioSource.PlayOneShot(s.clip, vol);
    }
}