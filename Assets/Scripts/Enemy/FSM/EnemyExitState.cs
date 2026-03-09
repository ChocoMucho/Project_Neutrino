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
        PoolManager.Instance.Despawn(enemy);
    }

    public override void Exit()
    {
    }
}
