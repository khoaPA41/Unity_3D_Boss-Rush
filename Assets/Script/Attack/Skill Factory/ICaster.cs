using UnityEngine;

namespace Attack.Skill_Factory
{
    public interface ICaster
    {
        void ComsumeMana(int amount);
        Transform GetTransform();
        GameObject TargetCaster();
    }
}