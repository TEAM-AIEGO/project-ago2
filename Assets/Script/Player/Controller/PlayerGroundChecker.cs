using UnityEngine;

public class PlayerGroundChecker : MonoBehaviour
{
    private Collider col;

    public void Initialized(Collider col)
    {
        this.col = col;
    }
    
    public bool IsGrounded()
    {
        int layerMask = LayerMask.GetMask("Ground");
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, col.bounds.extents.y + 0.2f, layerMask);
        return isGrounded;
    }

    //private bool IsGrounded()
    //{
    //    int layerMask = LayerMask.GetMask("Ground");
    //    bool isGrounded = Physics.Raycast(transform.position, Vector3.down, col.bounds.extents.y + 0.2f, layerMask);
    //    if (isGrounded && isGroundPounding)
    //    {
    //        isGroundPounding = false;
    //    }
    //    return isGrounded;
    //}
}
