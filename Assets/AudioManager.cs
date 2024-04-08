using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource calmAudioSource;
    public AudioSource upbeatAudioSource;
    public AudioSource kenAudioSource;
    public Text displayText;
    public string displayTextPlaceholder = "Currently playing -> ";

    private enum AudioState { Calm, Upbeat, Ken };
    private AudioState currentAudioState;

    void Start()
    {
        // Start with the first audio source
        currentAudioState = AudioState.Calm;
        displayText.text = displayTextPlaceholder + GetCurrentAudioLabel();
        PlayCurrentAudio();
    }

    public void SwitchToNextAudio()
    {
        // Cycle through the audio sources
        switch (currentAudioState)
        {
            case AudioState.Calm:
                currentAudioState = AudioState.Upbeat;
                break;
            case AudioState.Upbeat:
                currentAudioState = AudioState.Ken;
                break;
            case AudioState.Ken:
                currentAudioState = AudioState.Calm;
                break;
        }
        displayText.text = displayTextPlaceholder + GetCurrentAudioLabel();
        PlayCurrentAudio();
    }

    private void PlayCurrentAudio()
    {
        // Stop all audio sources
        StopAllAudio();

        // Play the current audio source
        switch (currentAudioState)
        {
            case AudioState.Calm:
                calmAudioSource.Play();
                break;
            case AudioState.Upbeat:
                upbeatAudioSource.Play();
                break;
            case AudioState.Ken:
                kenAudioSource.Play();
                break;
        }
    }

    private void StopAllAudio()
    {
        calmAudioSource.Stop();
        upbeatAudioSource.Stop();
        kenAudioSource.Stop();
    }

    public string GetCurrentAudioLabel()
    {
        switch (currentAudioState)
        {
            case AudioState.Calm:
                return "Calm";
            case AudioState.Upbeat:
                return "Upbeat";
            case AudioState.Ken:
                return "Ken";
            default:
                return "Unknown";
        }
    }
}
