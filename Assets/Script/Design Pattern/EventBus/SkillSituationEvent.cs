using System;
using UnityEngine;

public class SkillSituationEvent : MonoBehaviour
{
    public static SkillSituationEvent Instance;

    public event Action SituationEvent;
    public event Action NextActionEvent;
    public event Action ReleasePoolObjectEvent;


    private void Awake()
    {
        Instance = this;
    }

    public void SendSituationEvent() => SituationEvent?.Invoke();
    
    public void SendNextActionEvent() => NextActionEvent?.Invoke();
    
    public void SendReleasePoolObjectEvent() => ReleasePoolObjectEvent?.Invoke();

}
