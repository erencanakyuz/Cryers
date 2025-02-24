using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        public bool loop;
        [Range(0f, 1f)]
        public float volume = 1f;
        [Range(0.1f, 3f)]
        public float pitch = 1f;

        [HideInInspector]
        public AudioSource source;
    }

    public List<Sound> sounds;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string name)
    {
        Sound s = sounds.Find(sound => sound.name == name);
        if (s != null)
        {
            s.source.Play();
            Debug.Log("succes");
        }
        else
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
    }

    public void Stop(string name)
    {
        Sound s = sounds.Find(sound => sound.name == name);
        if (s != null)
        {
            s.source.Stop();
        }
        else
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
    }
}
