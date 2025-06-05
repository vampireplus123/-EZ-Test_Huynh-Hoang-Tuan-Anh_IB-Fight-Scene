using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Fighter victim = other.GetComponent<Fighter>();
        Fighter attacker = GetComponentInParent<Fighter>(); 

        if (victim != null && attacker != null)
        {
            victim.TakeDamge(attacker.damage);
            victim.BeingHit();
        }
    }

}
