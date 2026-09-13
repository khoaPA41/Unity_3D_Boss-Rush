using UnityEngine;

public class ManageAttackAnimation : MonoBehaviour
{
    private readonly int AttackSpeedAnimationParam = Animator.StringToHash("AttackSpeed");
    [SerializeField] private float attackAnimationSpeedSlow;

    [SerializeField] private float attackAnimationSpeedFast;


    private Animator _animator;


    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetSlowSpeedAnimation()
    {
        _animator.SetFloat(AttackSpeedAnimationParam, attackAnimationSpeedSlow);
    }

    public void ReturnNormalSpeedAnimation()
    {
        _animator.SetFloat(AttackSpeedAnimationParam, attackAnimationSpeedFast);
    }
}
