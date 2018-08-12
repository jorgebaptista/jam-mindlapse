using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundFXPlayer : MonoBehaviour
{

    AudioSource audioSource;

    AudioClip Analogue_Boss_Spawn;
    AudioClip Analogue_Monster_Attack;
    AudioClip Analogue_Monster_Death;
    AudioClip Analogue_Monster_Hover;
    AudioClip Analogue_Monster_Pass_Through_Wall;
    AudioClip Analogue_Monster_Spawn;
    AudioClip Analogue_Player_Fall;
    AudioClip Analogue_Player_Ouch;
    AudioClip Analogue_Player_Spawn;
    AudioClip Analogue_Player_Step;
    AudioClip Analogue_Room_Collapse;
    AudioClip Analogue_Room_Crack;
    AudioClip Analogue_Room_Rebuild;
    AudioClip Analogue_Room_Repair;
    AudioClip Analogue_Sanity_Gain;
    AudioClip Analogue_Spell_Cast;
    AudioClip Analogue_Spell_Land;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        LoadPlayerSounds();
        LoadMonsterSounds();
        LoadEnvironmentSounds();
    }

    #region Player Sounds

    public void PlayFootStepSound()
    {
        audioSource.pitch = (Random.Range(0.6f, 1.0f));
        audioSource.PlayOneShot(Analogue_Player_Step);
    }

    public void PlayFallSound()
    {
        audioSource.PlayOneShot(Analogue_Player_Fall);
    }

    public void PlayOuchSound()
    {
        audioSource.PlayOneShot(Analogue_Player_Ouch);
    }

    public void PlaySpawnSound()
    {
        audioSource.PlayOneShot(Analogue_Player_Spawn);
    }

    public void PlaySanityGainSound()
    {
        audioSource.PlayOneShot(Analogue_Monster_Death);
    }

    public void PlaySpellCastSound()
    {
        audioSource.pitch = (Random.Range(0.8f, 1.0f));
        audioSource.PlayOneShot(Analogue_Spell_Cast);
    }

    public void PlaySpellLandSound()
    {
        audioSource.PlayOneShot(Analogue_Spell_Land);
    }

    #endregion

    #region Monster Sounds

    public void PlayMonsterAttackSound()
    {
        audioSource.PlayOneShot(Analogue_Monster_Attack);
    }

    public void PlayMonsterDeathSound()
    {
        audioSource.PlayOneShot(Analogue_Monster_Death);
    }

    public void ToggleMonsterHoverSound(bool active)
    {
        // Need to change this so it loops
        if (active)
            audioSource.PlayOneShot(Analogue_Monster_Hover);
    }

    public void PlayMonsterWallPassSound()
    {
        audioSource.PlayOneShot(Analogue_Monster_Pass_Through_Wall);
    }

    public void PlayMonsterSpawnSound()
    {
        audioSource.PlayOneShot(Analogue_Monster_Spawn);
    }

    #endregion

    #region Environment Sounds

    public void PlayRoomCollapseSound()
    {
        audioSource.PlayOneShot(Analogue_Room_Collapse);
    }

    public void PlayRoomCrackSound()
    {
        audioSource.PlayOneShot(Analogue_Room_Crack);
    }

    public void ToggleRoomRebuildSound()
    {
        audioSource.PlayOneShot(Analogue_Room_Rebuild);
    }

    public void PlayRoomRepairSound(bool active)
    {
        // Need to change this to loop
        if (active)
            audioSource.PlayOneShot(Analogue_Room_Repair);
    }

    #endregion

    #region Load Sounds

    private void LoadPlayerSounds()
    {
        Analogue_Player_Fall = Resources.Load("Analogue_Player_Fall") as AudioClip;
        Analogue_Player_Ouch = Resources.Load("Analogue_Player_Ouch") as AudioClip;
        Analogue_Player_Spawn = Resources.Load("Analogue_Player_Spawn") as AudioClip;
        Analogue_Player_Step = Resources.Load("Analogue_Player_Step") as AudioClip;

        Analogue_Sanity_Gain = Resources.Load("Analogue_Sanity_Gain") as AudioClip;

        Analogue_Spell_Cast = Resources.Load("Analogue_Spell_Cast") as AudioClip;
        Analogue_Spell_Land = Resources.Load("Analogue_Spell_Land") as AudioClip;
    }

    private void LoadMonsterSounds()
    {
        Analogue_Boss_Spawn = Resources.Load("Analogue_Boss_Spawn") as AudioClip;

        Analogue_Monster_Attack = Resources.Load("Analogue_Monster_Attack") as AudioClip;
        Analogue_Monster_Death = Resources.Load("Analogue_Monster_Death") as AudioClip;
        Analogue_Monster_Hover = Resources.Load("Analogue_Monster_Hover") as AudioClip;
        Analogue_Monster_Pass_Through_Wall = Resources.Load("Analogue_Monster_Pass_Through_Wall") as AudioClip;
        Analogue_Monster_Spawn = Resources.Load("Analogue_Monster_Spawn") as AudioClip;
    }

    private void LoadEnvironmentSounds()
    {
        Analogue_Room_Collapse = Resources.Load("Analogue_Room_Collapse") as AudioClip;
        Analogue_Room_Crack = Resources.Load("Analogue_Room_Crack") as AudioClip;
        Analogue_Room_Rebuild = Resources.Load("Analogue_Room_Rebuild") as AudioClip;
        Analogue_Room_Repair = Resources.Load("Analogue_Room_Repair") as AudioClip;
    }

    #endregion

}
