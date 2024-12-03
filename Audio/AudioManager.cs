using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource SoundFXObject;
    [SerializeField] private AudioSource BackGroundMusicSource;
    [SerializeField] private AudioSource ButtonMusicSource;
    [SerializeField] private AudioSource OtherSoundFXSource;
    [SerializeField] private AudioSourceSO AudioSO;
    
    private void Awake()
    {    
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else Destroy(gameObject);
    }

    public void PlayCampfireSound()
    {
        OtherSoundFXSource.clip = AudioSO.CampfireSound;
        OtherSoundFXSource.Play();
    }

    public void StopCampfireSound()
    {
        OtherSoundFXSource.Stop();
    }

    public void PlayMainMenuBackgroundMusic()
    {
        BackGroundMusicSource.clip = AudioSO.BackGroundMusic[0];
        BackGroundMusicSource.Play();
    }
    
    public void PlayUpgradeMenuBackgroundMusic()
    {
        BackGroundMusicSource.clip = AudioSO.BackGroundMusic[1];
        BackGroundMusicSource.Play();
    }
    
    public void PlayDungeonBackgroundMusic()
    {
        BackGroundMusicSource.clip = AudioSO.BackGroundMusic[2];
        BackGroundMusicSource.Play();
    }

    public void PlayCombatBackgroundMusic()
    {
        BackGroundMusicSource.clip = AudioSO.BackGroundMusic[3];
        BackGroundMusicSource.Play();
    }

    public void PlayButtonHoverSound()
    {
        ButtonMusicSource.clip = AudioSO.ButtonHoverSound;
        ButtonMusicSource.Play();
    }
    
    public void PlayButtonClickSound()
    {
        ButtonMusicSource.clip = AudioSO.ButtonClickSound;
        ButtonMusicSource.Play();
    }

    public void PlayUpgradeSound()
    {
        OtherSoundFXSource.clip = AudioSO.UpgradeSound;
        OtherSoundFXSource.Play();
    }

    public void PlayCheatSound()
    {
        OtherSoundFXSource.clip = AudioSO.CheatSound;
        OtherSoundFXSource.Play();
    }
    

    public void PlayPunch(Transform spawnPosition)
    {
        int random = Random.Range(0, 2);
        
        AudioSource audioSource = Instantiate(SoundFXObject, spawnPosition);
        audioSource.clip = AudioSO.Punch[random];
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
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
