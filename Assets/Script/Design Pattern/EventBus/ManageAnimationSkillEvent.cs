using System;
using UnityEngine;

public class ManageAnimationSkillEvent : MonoBehaviour
{
    public event Action SituationEvent;
    public event Action NextActionEvent;
    public event Action ReleasePoolObjectEvent;
    public event Action SlashWeaponEvent;
    
    public void SendSituationEvent() => SituationEvent?.Invoke();
    public void SendNextActionEvent() => NextActionEvent?.Invoke();
    public void SendReleasePoolObjectEvent() => ReleasePoolObjectEvent?.Invoke();
    public void SendSlashWeaponEventEvent() => SlashWeaponEvent?.Invoke();

}
