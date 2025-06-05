using UnityEngine;

public class DrawADestination : MonoBehaviour
{ 
    public Vector3 boxSize = new Vector3(1, 1, 1); 
    public Color gizmoColor = Color.red;          

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}
