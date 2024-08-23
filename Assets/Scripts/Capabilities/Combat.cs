//TODO: Implement combat system
using UnityEngine;

[RequireComponent(typeof(Controller))]
public class Combat : MonoBehaviour
{
    private Ground ground;
    private Controller controller;
    public bool desiredAttack;
    int count;
    void Awake()
    {
        ground = GetComponent<Ground>();
        controller = GetComponent<Controller>();

    }

    void Update()
    {
        Attack();
        desiredAttack |= controller.input.RetrieveAttackInput();
    }
    
    public void Attack()
    {
        if (desiredAttack)
        {
            count++;
            if (count > 1)
                count = 0;
            desiredAttack = false;
        }
    }

    public bool GetAttack()    
    {
        return desiredAttack;
    }

    public int AttackCounter()
    {
        return count;
    }
}
