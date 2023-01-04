using UnityEngine;
using UnityEngine.UI;

public abstract class BaseAbilitySO : ScriptableObject
{
    public Image AbilityImage;
    public string Name;
    public bool IsPassive;
    public bool IsDelayed;
    public LayerMask CollideWith;

    [SerializeField] private FloatValueSO _cooldown;
    public float Cooldown => _cooldown.RuntimeValue;
}

public abstract class BaseAbility
{
    public Image image;
    public bool IsCooldownPending => CurentCooldown != 0;

    internal float _overallCooldown;
    internal float _curentCooldown;

    public float OverallCooldown
    {
        get { return _overallCooldown; }
        set { _overallCooldown = value; }
    }

    public float CurentCooldown
    {
        get { return _curentCooldown; }
        set { _curentCooldown = value; }
    }

    public virtual void RunCooldown()
    {
        if (CurentCooldown > 0)
        {
            CurentCooldown -= Time.deltaTime;
        }
        else
        {
            CurentCooldown = 0;
        }
    }

    public abstract void Awake(MonoBehaviour runner);

    public virtual void Start()
    {
        CurentCooldown = OverallCooldown;
    }

    public virtual void Update()
    {
        RunCooldown();
    }

    protected abstract void TriggerAbility();
}
