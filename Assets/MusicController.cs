using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioClip [] clips;

    private AudioSource audioSource;
    private int currentMusic = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = clips[currentMusic];
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying) 
        {
            currentMusic++;
            if (currentMusic >= clips.Length)
                currentMusic = 0;

            audioSource.clip = clips[currentMusic];
            audioSource.Play();


        }
    }
}
