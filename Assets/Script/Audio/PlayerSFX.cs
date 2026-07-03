using UnityEngine;
using UnityEngine.Audio;

public class PlayerSFX : MonoBehaviour
{
    [Header("Movement Audio Source")]
    [SerializeField] private AudioSource walkSource;
    [SerializeField] private AudioSource runSource;
    [SerializeField] private AudioSource jumpSource;
    [SerializeField] private AudioSource landingSource;

    [Header("Attack Audio Source")]
    [SerializeField] private AudioSource attackSource;
    [SerializeField] private AudioSource kickSource;
    [SerializeField] private AudioSource normalDodgeSource;
    [SerializeField] private AudioSource perfectDodgeSource;

    [Header("Hit Audio Source")]
    [SerializeField] private AudioSource hitSource;

    public void PlayWalkSound()
    {
        if(walkSource.isPlaying) walkSource.Stop();
        walkSource.Play();
    }

    public void PlayRunSound()
    {
        if(runSource.isPlaying) runSource.Stop();
        runSource.Play();
    }
    
    public void PlayJumpSound()
    {
        if(jumpSource.isPlaying) jumpSource.Stop();
        jumpSource.Play();
    }
    
    public void PlayLandingSound()
    {
        if(landingSource.isPlaying) landingSource.Stop();
        landingSource.Play();
    }
    
    public void PlayAttackSound()
    {
        if(attackSource.isPlaying) attackSource.Stop();
        attackSource.Play();
    }
    
    public void PlayKickSound()
    {
        if(kickSource.isPlaying) kickSource.Stop();
        kickSource.Play();
    }
    
    public void PlayHitSound()
    {
        if(hitSource.isPlaying) hitSource.Stop();
        hitSource.Play();
    }
    
    public void PlayPerfectDodgeSound()
    {
        if(perfectDodgeSource.isPlaying) perfectDodgeSource.Stop();
        perfectDodgeSource.Play();
    }
}
