using UnityEngine;

public class EnemyExitState : EnemyState
{
    public EnemyExitState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
    }

    public override void Execute()
    {
        switch (stateMachine.data.Type)
        {
            case EnemyType.Dash:
                PoolManager.Instance.Despawn(enemy);
                break;
            case EnemyType.Bullet:
                break;
            default:
                Debug.LogError("Undefined Enemy Type");
                break;
        }
    }

    public override void Exit()
    {
    }
}
