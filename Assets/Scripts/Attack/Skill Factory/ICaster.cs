using UnityEngine;

namespace Attack.Skill_Factory
{
    public interface ICaster
    {
        void ConsumeMana(int amount);
        Transform GetTransform();
        GameObject TargetCaster();
    }
}