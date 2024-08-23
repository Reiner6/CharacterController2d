using UnityEngine;

public class Animation : MonoBehaviour
{
    protected Animator animator;
    protected Move move;
    protected Jump jump;
    protected Ground ground;
    protected Combat combat;
    protected SpriteRenderer spriteRenderer;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        move = GetComponent<Move>();
        jump = GetComponent<Jump>();
        ground = GetComponent<Ground>();
        combat = GetComponent<Combat>();
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
    }
}