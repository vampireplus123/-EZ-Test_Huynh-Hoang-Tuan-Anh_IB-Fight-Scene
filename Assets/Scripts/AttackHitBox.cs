using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Fighter victim = other.GetComponent<Fighter>();
        Fighter attacker = GetComponentInParent<Fighter>(); 

        if (victim != null && attacker != null)
        {
            if (victim.tag == attacker.tag)
            {
                Debug.Log("Cham chung tag");
                return;
            }
            Debug.Log("Touched");
            victim.TakeDamge(attacker.GetDamage());
            victim.BeingHit();
        }
    }

}
