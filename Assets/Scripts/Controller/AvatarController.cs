using UnityEngine;

[CreateAssetMenu(fileName = "PlayerController",menuName = "InputController/PlayerController")]
public class AvatarController : InputController
{
    public override bool RetrieveJumpInput()
    {
        return Input.GetButton("Jump");
    }

    public override float RetrieveMoveInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public override bool RetrieveAttackInput()
    {
        return Input.GetButtonDown("Fire1");
    }
}
