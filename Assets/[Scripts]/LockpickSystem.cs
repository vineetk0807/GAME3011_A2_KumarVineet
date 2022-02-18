using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sounds
{
    LOCKPICKING,
    CLICKED,
    ACTIVATE,
    STOP
}

public class LockpickSystem : MonoBehaviour
{
    // Audio Source
    public AudioSource source;

    // List of Audio Clips to play
    public List<AudioClip> soundsToPlay = new List<AudioClip>();

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySounds(int sound)
    {
        switch (sound)
        {
            case (int)Sounds.LOCKPICKING:
                source.loop = true;
                source.clip = soundsToPlay[(int)Sounds.LOCKPICKING];
                source.Play();
                break;

            case (int)Sounds.CLICKED:
                source.loop = false;
                source.clip = soundsToPlay[(int)Sounds.CLICKED];
                source.Play();
                break;

            case (int)Sounds.ACTIVATE:
                source.loop = false;
                source.clip = soundsToPlay[(int)Sounds.ACTIVATE];
                source.Play();
                break;

            case (int)Sounds.STOP:
                source.Stop();
                source.loop = false;
                break;
        }
    }
}
