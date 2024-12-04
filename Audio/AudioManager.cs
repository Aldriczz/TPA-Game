using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource SkillFXSource;
    [SerializeField] private AudioSource BackGroundMusicSource;
    [SerializeField] private AudioSource ButtonMusicSource;
    [SerializeField] private AudioSource OtherSoundFXSource;
    [SerializeField] private AudioSource CombatSoundFXSource;
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
    
    public void PlayPunch()
    {
        int random = Random.Range(0, 2);
        
        CombatSoundFXSource.clip = AudioSO.Punch[random];
        CombatSoundFXSource.Play();
    }
    public void PlaySwordSlash()
    {
        int random = Random.Range(0, 3);
        
        CombatSoundFXSource.clip = AudioSO.SwordSlash[random];
        CombatSoundFXSource.Play();
    }

    public void PlaySwordCriticalSlash()
    {
        int random = Random.Range(0, 3);
        
        CombatSoundFXSource.clip = AudioSO.SwordCriticalSlash[random];
        CombatSoundFXSource.Play();
    }

    public void PlayArcaneStrikeToggle()
    {
        SkillFXSource.clip = AudioSO.ArcaneSlashToggle;
        SkillFXSource.Play();
    }
    
    public void PlayArcaneStrikeImpact()
    {
        SkillFXSource.clip = AudioSO.ArcaneSlashImpact;
        SkillFXSource.Play();
    }
    
    public void PlayDivineStrike()
    { 
        SkillFXSource.clip = AudioSO.DivineArcane;
        SkillFXSource.Play();
    }
    public void PlayBuff()
    { 
        SkillFXSource.clip = AudioSO.BuffSound;
        SkillFXSource.Play();
    }
    public void PlayFootStep()
    {
        int random = Random.Range(0, 3);
        
        OtherSoundFXSource.clip = AudioSO.FootStep[random];
        OtherSoundFXSource.Play();
    }

    public void PlayGetHit()
    {
        int random = Random.Range(0, 3);
        
        OtherSoundFXSource.clip = AudioSO.GetHit[random];
        OtherSoundFXSource.Play();
    }
    
    public void PlayDied()
    {
        int random = Random.Range(0, 2);
        
        OtherSoundFXSource.clip = AudioSO.Died[random];
        OtherSoundFXSource.Play();
    }

    public void PlayAlert()
    {
        OtherSoundFXSource.clip = AudioSO.AlertAudio;
        OtherSoundFXSource.Play();
    }
    
    public void PlayAgro()
    {
        OtherSoundFXSource.clip = AudioSO.AgroAudio;
        OtherSoundFXSource.Play();
    }
    
    
    
    void Update()
    {
        
    }
}
