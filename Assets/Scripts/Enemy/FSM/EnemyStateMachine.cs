using JetBrains.Annotations;
using UnityEngine;

public class EnemyStateMachine
{
    EnemyState currentState;

    EnemyEnterState enterState;
    EnemyHoldState holdState;
    EnemyAttackState attackState;
    EnemyExitState exitState;

    private EnemyDataSO data;

    public EnemyStateMachine(Enemy enemy, EnemyDataSO data)
    {
        this.data = data;

        enterState = new EnemyEnterState(enemy, this);
        holdState = new EnemyHoldState(enemy, this);
        attackState = new EnemyAttackState(enemy, this);
        exitState = new EnemyExitState(enemy, this);
    }

    public void Init() // Enemy에서 호출
    {
        currentState = enterState;
        currentState.Enter();
    }

    public void ChangeState(EnemyState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        currentState.Execute();
    }

}
