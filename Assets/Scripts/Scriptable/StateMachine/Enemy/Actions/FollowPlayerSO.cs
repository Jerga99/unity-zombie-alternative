
using Eincode.ZombieSurvival.Enemy;
using Eincode.ZombieSurvival.Manager;
using UnityEngine;

[CreateAssetMenu(
    fileName = "FollowPlayerSO",
    menuName = "StateMachine/Enemy/Follow Player"
)]
public class FollowPlayerSO : StateActionSO<FollowPlayer>
{
    public float SpeedModifier;

    public override StateAction CreateAction(StateMachine stateMachine)
    {
        return new FollowPlayer();
    }
}

// TODO: CACHING!
public class FollowPlayer : StateAction
{
    public new FollowPlayerSO OriginSO => (FollowPlayerSO)_originSO;
    private float _randomSpeedModifier;
    private EnemyBehaviour _enemyBehaviour;
    private Transform _player;

    public Vector3 EnemyPosition => _enemyBehaviour.transform.position;

    public override void Awake(StateMachine stateMachine)
    {
        _enemyBehaviour = stateMachine.GetComponent<EnemyBehaviour>();
        _player = SceneManager.Instance.Player.transform;
        _randomSpeedModifier = Random.Range(1.0f, 1.5f);
    }

    public override void OnUpdate()
    {
        if (_enemyBehaviour.IsDeath) { return; }

        ChasePlayer();
    }

    private void ChasePlayer()
    {
        if (_enemyBehaviour.AccuReactionTime >= _enemyBehaviour.ReactionTime)
        {
            var shouldTurnRight = (EnemyPosition.x - _player.position.x) > 0;

            if (!shouldTurnRight && _enemyBehaviour.FacingRight)
            {
                _enemyBehaviour.Flip();
            }
            else if (shouldTurnRight && !_enemyBehaviour.FacingRight)
            {
                _enemyBehaviour.Flip();
            }

            _enemyBehaviour.AccuReactionTime = 0;
        }

        _enemyBehaviour.transform.position = Vector3.MoveTowards(
            EnemyPosition,
            _player.position,
            Time.deltaTime * OriginSO.SpeedModifier * _randomSpeedModifier
        );

        _enemyBehaviour.AccuReactionTime += Time.deltaTime;
    }
}

