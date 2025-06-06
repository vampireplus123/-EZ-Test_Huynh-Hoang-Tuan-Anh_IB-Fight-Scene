using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Fighter victim = other.GetComponent<Fighter>();
        Fighter attacker = GetComponentInParent<Fighter>(); 

        if (victim == null || attacker == null) return;

        if (victim.tag == attacker.tag)
        {
            Debug.Log("Cham chung tag");
            return;
        }

        if (attacker.isDie)
        {
            Debug.Log("Attacker is dead, no damage dealt.");
            return;
        }

        if (victim.isDie)
        {
            Debug.Log("Victim already dead.");
            return;
        }

        Debug.Log("Touched");
        victim.TakeDamge(attacker.GetDamage());
        victim.BeingHit();
    }
}
