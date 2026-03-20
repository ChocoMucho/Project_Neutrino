using JetBrains.Annotations;
using UnityEngine;

public class EnemyStateMachine
{
    EnemyState currentState;

    public EnemyEnterState enterState { get; private set; }
    public EnemyHoldState holdState { get; private set; }
    public EnemyAttackState attackState { get; private set; }
    public EnemyExitState exitState { get; private set; }

    public EnemyDataSO data { get; private set; }

    public float stateChangeTime { get; private set; }

    public EnemyStateMachine(Enemy enemy)
    {
        enterState = new EnemyEnterState(enemy, this);
        holdState = new EnemyHoldState(enemy, this);
        attackState = new EnemyAttackState(enemy, this);
        exitState = new EnemyExitState(enemy, this);
    }

    public void Init(EnemyDataSO newData) // Enemy에서 호출
    {
        this.data = newData;
        if(currentState != null) 
        {
            currentState.Exit();
        }
        currentState = enterState;
        currentState.Enter();
    }

    public void ChangeState(EnemyState newState)
    {
        // TODO: null check
        Debug.Log($"Changing state from {currentState.GetType().Name} to {newState.GetType().Name}");
        currentState.Exit();
        currentState = newState;
        stateChangeTime = Time.time; // 상태 전환 시점 기록
        currentState.Enter();
    }

    public void Update()
    {
        currentState.Execute();
    }

}
