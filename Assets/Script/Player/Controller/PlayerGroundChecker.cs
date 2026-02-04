using UnityEngine;

public class PlayerGroundChecker : MonoBehaviour
{
    [SerializeField] private Transform groundCheckTr;

    [SerializeField] private Vector3 halfBox;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    private bool isGrounded;
    private Collider col;

    public void Initialized(Collider col)
    {
        this.col = col;
    }
    
    public bool IsGrounded()
    {
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, col.bounds.extents.y + 0.2f, groundLayer);

        if (isGrounded)
        {
            Debug.Log(isGrounded);
            this.isGrounded = isGrounded;
            return isGrounded;
        }

        Vector3 vec = groundCheckTr.position;
        Quaternion qut = Quaternion.identity;

        //isGrounded = Physics.BoxCast(vec, halfBox, Vector3.down, out RaycastHit hit, qut, groundCheckDistance, groundLayer, QueryTriggerInteraction.Ignore);
        isGrounded = Physics.CheckBox(vec + Vector3.down * groundCheckDistance, halfBox, qut, groundLayer, QueryTriggerInteraction.Ignore);

        Debug.Log(isGrounded);
        this.isGrounded = isGrounded;
        return isGrounded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;

        Matrix4x4 old = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(groundCheckTr.position, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, halfBox * 2f);

        Gizmos.DrawWireCube(Vector3.down * groundCheckDistance, halfBox * 2f);
        Gizmos.matrix = old;
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
