using UnityEngine;

public class EnemyEnterState : EnemyState
{
    public EnemyEnterState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
    }

    public override void Execute()
    {
        enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, enemy.HoldPosition, 5 * Time.deltaTime);
        if(Vector2.Distance(enemy.transform.position, enemy.HoldPosition) < 0.1f)
        {
            stateMachine.ChangeState(stateMachine.holdState);
        }
    }

    public override void Exit()
    {
    }
}
