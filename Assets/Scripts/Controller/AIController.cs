using UnityEngine;
//WIP
[CreateAssetMenu(fileName = "AIController", menuName = "InputController/AIController")]
public class AIController : InputController
{
    public override bool RetrieveJumpInput()
    {
        return true;
    }
    public override bool RetrieveAttackInput()
    {
        return false;
    }

    public override float RetrieveMoveInput()
    {
        return 1f;
    }
}