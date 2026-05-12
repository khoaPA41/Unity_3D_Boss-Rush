using UnityEngine;

public interface ICaster
{
    void ComsumeMana(int amount);
    Transform GetTransform();
    GameObject TargetCaster();
}
