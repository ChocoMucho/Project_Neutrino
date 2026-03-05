using UnityEngine;

public abstract class EnemyState
{
    protected Enemy enemy; // data so
    protected EnemyStateMachine stateMachine; // switch state

    public EnemyState(Enemy enemy, EnemyStateMachine stateMachine)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
    }

    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();
}
