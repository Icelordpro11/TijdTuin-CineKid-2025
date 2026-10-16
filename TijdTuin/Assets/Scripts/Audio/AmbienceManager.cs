using UnityEngine;

/// <summary>
/// <c>AmbienceManager</c> class handles the ambience audio sources and their volume based on game events,
/// </summary>
public class AmbienceManager : MonoBehaviour
{
    
    [Header("References")]
    [SerializeField] private FunctionCaller _functionCaller;
    [SerializeField] private VoiceLinePlayer _voiceLinePlayer;
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource _deadAmbienceAudioSource;
    [SerializeField] private AudioSource _lifeAmbienceAudioSource;
    
    public static AmbienceManager instance;
    private TimeMachine _timeMachine;
    
    private void Start()
    {
        _deadAmbienceAudioSource.volume = .2f;
        _timeMachine = transform.parent.GetComponentInChildren<TimeMachine>();

        _deadAmbienceAudioSource.Play();
    }

    private void Update()
    {
        if (_functionCaller.FinishedTalk || !_voiceLinePlayer.isTalking)
        {
            _deadAmbienceAudioSource.volume = .5f;
            _lifeAmbienceAudioSource.volume = .5f;
        }
        else if (_voiceLinePlayer.isTalking)
        {
            _deadAmbienceAudioSource.volume = .2f;
            _lifeAmbienceAudioSource.volume = .2f;
        }

        if (!_timeMachine.ambienceChange) return;
        _deadAmbienceAudioSource.Stop();
        
        if (!_lifeAmbienceAudioSource.isPlaying)
        {
            _lifeAmbienceAudioSource.Play(); 
        }
    }
}
