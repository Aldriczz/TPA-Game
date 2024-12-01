using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource SoundFXObject;
    [SerializeField] private AudioSourceSO AudioSO;
    
    void Start()
    {
        if(Instance == null) Instance = this; else Destroy(gameObject);
    }

    public void PlaySwordSlash(Transform spawnPosition)
    {
        int random = Random.Range(0, 3);
        
        AudioSource audioSource = Instantiate(SoundFXObject, spawnPosition);
        audioSource.clip = AudioSO.SwordSlash[random];
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }

    public void PlaySwordCriticalSlash(Transform spawnPosition)
    {
        int random = Random.Range(0, 3);
        
        AudioSource audioSource = Instantiate(SoundFXObject, spawnPosition);
        audioSource.clip = AudioSO.SwordCriticalSlash[random];
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }

    public void PlayArcaneStrikeToggle(Transform spawnPosition)
    {
        AudioSource audioSource = Instantiate(SoundFXObject, spawnPosition);
        audioSource.clip = AudioSO.ArcaneSlashToggle;
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }
    
    public void PlayArcaneStrikeImpact(Transform spawnPosition)
    {
        AudioSource audioSource = Instantiate(SoundFXObject, spawnPosition);
        audioSource.clip = AudioSO.ArcaneSlashImpact;
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }
    
    public void PlayFootStep(Transform spawnPosition)
    {
        int random = Random.Range(0, 3);
        
        AudioSource audioSource = Instantiate(SoundFXObject, spawnPosition);
        audioSource.clip = AudioSO.FootStep[random];
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }

    public void PlayGetHit(Transform spawnPosition)
    {
        int random = Random.Range(0, 3);
        
        AudioSource audioSource = Instantiate(SoundFXObject, spawnPosition);
        audioSource.clip = AudioSO.GetHit[random];
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }
    
    public void PlayDied(Transform spawnPosition)
    {
        int random = Random.Range(0, 2);
        
        AudioSource audioSource = Instantiate(SoundFXObject, spawnPosition);
        audioSource.clip = AudioSO.Died[random];
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }

    public void PlayAlert(Transform spawnPosition)
    {
        AudioSource audioSource = Instantiate(SoundFXObject, spawnPosition);
        audioSource.clip = AudioSO.AlertAudio;
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }
    
    public void PlayAgro(Transform spawnPosition)
    {
        AudioSource audioSource = Instantiate(SoundFXObject, spawnPosition);
        audioSource.clip = AudioSO.AgroAudio;
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }
    
    
    
    void Update()
    {
        
    }
}
