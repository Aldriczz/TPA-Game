using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AudioSO", menuName = "SO/AudioSO")]
public class AudioSourceSO : ScriptableObject
{
    public List<AudioClip> BackGroundMusic = new List<AudioClip>();
    public AudioClip CampfireSound;
    public AudioClip ButtonClickSound;
    public AudioClip ButtonHoverSound;
    public AudioClip UpgradeSound;
    public AudioClip CheatSound;
    
    public List<AudioClip> FootStep = new List<AudioClip>();
    public List<AudioClip> SwordSlash = new List<AudioClip>();  
    public List<AudioClip> Punch = new List<AudioClip>();  
    public List<AudioClip> SwordCriticalSlash = new List<AudioClip>();
    public List<AudioClip> GetHit = new List<AudioClip>();
    public List<AudioClip> Died = new List<AudioClip>();
    public AudioClip ArcaneSlashToggle;
    public AudioClip ArcaneSlashImpact;
    public AudioClip AlertAudio;
    public AudioClip AgroAudio;
   
}
