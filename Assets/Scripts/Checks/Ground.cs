using UnityEngine;

public class Ground : MonoBehaviour
{
    private bool onGround;
    private float frictionValue;

    public bool OnGround => onGround;
    public float FrictionValue => frictionValue;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        onGround = false;
        frictionValue = 0;
    }
    private void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 normal = collision.GetContact(i).normal;

            onGround = normal.y >= 0.9f;
        }
    }

    private void RetrieveFriction(Collision2D collision)
    {
        if (collision.rigidbody == null || collision.rigidbody.sharedMaterial == null)
            return;
        PhysicsMaterial2D material = collision.rigidbody.sharedMaterial;
        frictionValue = 0;
        if (material != null)
        {
            frictionValue = material.friction;
        }
    }
}
