using UnityEngine;

public class EnemyHoldState : EnemyState
{
    public EnemyHoldState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
    }

    public override void Execute()
    {
        if (Vector2.Distance(enemy.transform.position, enemy.HoldPosition) > 0.1f)
            stateMachine.ChangeState(stateMachine.enterState);

        SlowRotate();

        if (Time.time - stateMachine.stateChangeTime >= 3f)
            stateMachine.ChangeState(stateMachine.attackState);
    }

    public override void Exit()
    {
    }

    private void SlowRotate()
    {
        Vector2 targetDirection = enemy._playerTransform.position - enemy.transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90f);

        enemy.transform.rotation = Quaternion.RotateTowards
            (
                enemy.transform.rotation,
                targetRotation,
                stateMachine.data.rotationSpeed * Time.deltaTime
            );
    }
}
