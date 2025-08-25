public interface IState
{
    void Enter();
    void Update();
    void Exit();
}

public interface IDamgeable
{
    void TakeDamage(float damage);
}