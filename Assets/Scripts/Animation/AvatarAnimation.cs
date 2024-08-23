using UnityEngine;

public class AvatarAnimation : Animation
{
    void LateUpdate()
    {
        if (animator.GetBool("Ground") && animator.GetBool("Air"))
        {
            animator.SetBool("Air", false);
        }

        animator.SetBool("Ground", ground.OnGround);

        if (!animator.GetBool("Ground") && !animator.GetBool("Air"))
        {
            animator.SetBool("Air", true);
            animator.SetTrigger("EnterAir");

        }
        animator.SetBool("Falling", move.RB.velocity.y < 0);
        animator.SetBool("Walk", move.RB.velocity.x != 0);

        if (Mathf.Abs(move.RB.velocity.x) > 0)
            spriteRenderer.flipX = move.RB.velocity.x < 0;

        if (combat.GetAttack())
        {
            animator.SetTrigger("Attack_" + combat.AttackCounter());
            animator.SetTrigger("Attacking");
        }
    }
}