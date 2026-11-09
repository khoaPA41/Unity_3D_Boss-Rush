using UnityEngine;

namespace Attack.SkillFactory
{
    public interface ICaster
    {
        void ConsumeMana(int amount);
        Transform GetTransform();
        GameObject TargetCaster();
    }
}