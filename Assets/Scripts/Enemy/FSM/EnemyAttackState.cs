using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyAttackState : EnemyState
{
    private Vector2 direction;

    public EnemyAttackState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
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
                // TODO: 적한테 돌진하는 로직
                DashAttack();
                CheckOffScreen();
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
        direction = Vector2.zero;
    }

    private void DashAttack()
    {
        Dash();
        SlowRotate();
    }

    private void Dash()
    {
        enemy.transform.Translate(Vector3.up * 5f * Time.deltaTime);
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

    public void CheckOffScreen()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(enemy.transform.position);
        if (viewPos.x < -0.2 || viewPos.x > 1.2 || viewPos.y < -0.2 || viewPos.y > 1.2)
        {
            PoolManager.Instance.Despawn(enemy);
        }
    }
}
