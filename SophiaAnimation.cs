using UnityEngine;

public class SophiaAnimation : MonoBehaviour
{
    public void AttackAnimationComplete()
    {
        Sophia sophia = GetComponentInParent<Sophia>();
        if (sophia != null)
        {
            sophia.AnimationTrigger();
        }
    }
    public void AttackHit() => GetComponent<Sophia>().PerformAttack();

}
