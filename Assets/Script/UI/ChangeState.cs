using Script.Design_Pattern.StateMachine.Boss.Base;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ChangeState : StateMachineBehaviour
{
    [Header("Skill Situation Event")]
    [Tooltip("This event can spawn VFX, turn on sound")]
    [SerializeField] private bool usingSkillSituation;
    [SerializeField] private float triggerSkillSituation;
    
    [Header("Next Action Event")]
    [Tooltip("This event can change to next animation")]
    [SerializeField] private bool usingNextAction;
    [SerializeField] private float triggerNextAction;
    
    [Header("Next Action Event")]
    [Tooltip("This event can active weapon vfx")]
    [SerializeField] private bool usingWeaponVFX;
    [SerializeField] private float triggerWeaponVFX;

    [Header("Event Flag")] 
    private bool _hasTriggeredSkillSituation;
    private bool _hasTriggeredNextAction;
    private bool _hasTriggeredWeaponVFX;
    
    [Header("Params for weapon VFX")]
    [SerializeField] private bool isRightWeapon;
    [SerializeField] private bool isBothWeapon;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _hasTriggeredSkillSituation = false;
        _hasTriggeredNextAction = false;
        _hasTriggeredWeaponVFX = false;
    }

     public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
     {
         if(usingSkillSituation) SkillSituation(animator, stateInfo);
         if(usingNextAction) NextAction(animator, stateInfo);
         if(usingWeaponVFX)WeaponVFX(animator, stateInfo);
     }


     private void SkillSituation(Animator animator, AnimatorStateInfo stateInfo)
     {
         if (_hasTriggeredSkillSituation || stateInfo.normalizedTime < triggerSkillSituation) return;
         var animationEvent = animator.GetComponent<ManageAnimationSkillEvent>();
         animationEvent.SendSituationEvent();
         _hasTriggeredSkillSituation = true;
     }
     
     private void NextAction(Animator animator, AnimatorStateInfo stateInfo)
     {
         if (_hasTriggeredNextAction || stateInfo.normalizedTime < triggerNextAction) return;
         var animationEvent = animator.GetComponent<ManageAnimationSkillEvent>();
         animationEvent.SendNextActionEvent();
         _hasTriggeredNextAction = true;
     }
     
     private void WeaponVFX(Animator animator, AnimatorStateInfo stateInfo)
     {
         if (_hasTriggeredWeaponVFX || stateInfo.normalizedTime < triggerWeaponVFX) return;
         var animationEvent = animator.GetComponent<ManageAnimationSkillEvent>();
         animationEvent.SendSlashWeaponEventEvent();
         _hasTriggeredWeaponVFX = true;
         
         var stateMachine = animator.GetComponent<FinalBossStateMachine>();

         if (!isBothWeapon)
         {
             stateMachine.isRightWeaponVFX = isRightWeapon;
         }
         stateMachine.isBothWeaponVFX = isBothWeapon;
     }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
     public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
     {
         var animationEvent = animator.GetComponent<ManageAnimationSkillEvent>();
         // if (usingSkillSituation && !_hasTriggeredSkillSituation)
         // {
         //     animationEvent.SendSituationEvent();
         //     _hasTriggeredSkillSituation = true;
         // }
         
         if (usingNextAction && !_hasTriggeredNextAction)
         {
             animationEvent.SendNextActionEvent();
             _hasTriggeredNextAction = true;
         }
     }
     
#if UNITY_EDITOR
    [CustomEditor(typeof(ChangeState))]
    public class ChangeStateEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var script = (ChangeState)target;
            serializedObject.Update();
            EditorGUILayout.Space(5);
            
            /*Skill Situation*/
            script.usingSkillSituation = EditorGUILayout.ToggleLeft("Skill Situation Event", script.usingSkillSituation, EditorStyles.boldLabel);
            if (script.usingSkillSituation)
            {
                EditorGUI.indentLevel++;
                script.triggerSkillSituation = EditorGUILayout.FloatField("Time: 0 - 1", script.triggerSkillSituation);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space(3);
            
            /*Next Action*/
            script.usingNextAction = EditorGUILayout.ToggleLeft("Next Action Event", script.usingNextAction, EditorStyles.boldLabel);
            if (script.usingNextAction)
            {
                EditorGUI.indentLevel++;
                script.triggerNextAction = EditorGUILayout.FloatField("Time: 0 - 1", script.triggerNextAction);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space(3);
            
            /*Weapon VFX*/
            script.usingWeaponVFX = EditorGUILayout.ToggleLeft("Weapon VFX Event", script.usingWeaponVFX, EditorStyles.boldLabel);
            if (script.usingWeaponVFX)
            {
                EditorGUI.indentLevel++;
                script.triggerWeaponVFX = EditorGUILayout.FloatField("Time: 0 - 1", script.triggerWeaponVFX);
                script.isRightWeapon = EditorGUILayout.ToggleLeft("Right or Left: ", script.isRightWeapon);
                script.isBothWeapon = EditorGUILayout.ToggleLeft("Both: ", script.isBothWeapon);

                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space(3);

            if (!GUI.changed) return;
            EditorUtility.SetDirty(script);
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
