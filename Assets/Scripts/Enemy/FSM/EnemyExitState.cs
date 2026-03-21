using UnityEngine;

public class EnemyExitState : EnemyState
{
    private Vector3 exitDirection;
    private float currentSpeed;
    private const float acceleration = 12f;

    public EnemyExitState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        currentSpeed = stateMachine.data.Speed;
        
        if (stateMachine.data.Type == EnemyType.Bullet)
        {
            float xDir = (enemy.transform.position.x < 0) ? -0.8f : 0.8f;
            exitDirection = new Vector3(xDir, 1f, 0f).normalized;
        }
    }

    public override void Execute()
    {
        switch (stateMachine.data.Type)
        {
            case EnemyType.Dash:
                PoolManager.Instance.Despawn(enemy);
                break;
            case EnemyType.Bullet:
                // 1. 매 프레임 속도를 증가시킵니다 (가속도 적용)
                currentSpeed += acceleration * Time.deltaTime;

                // 2. 가속도가 붙은 속도로 지정된 대각선 방향 이동
                enemy.transform.position += exitDirection * currentSpeed * Time.deltaTime;

                // 3. 화면 밖으로 완전히 나갔는지 체크하여 제거
                if (CheckOffScreen())
                {
                    PoolManager.Instance.Despawn(enemy);
                }
                break;
            default:
                Debug.LogError("Undefined Enemy Type");
                break;
        }
    }

    public override void Exit()
    {
    }

    public bool CheckOffScreen()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(enemy.transform.position);
        if (viewPos.x < -0.2 || viewPos.x > 1.2 || viewPos.y < -0.2 || viewPos.y > 1.2)
            return true;

        return false;
    }
}
