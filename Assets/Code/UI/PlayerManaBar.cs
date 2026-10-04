public class PlayerManaBar : HealthBar
{
    private BattleContext _battleContext;
    
    private void Start()
    {
        CombatManager.Instance.onSetupBattle += OnSetupBattle;
    }

    private void OnDestroy()
    {
        if (_battleContext != null)
        {
            _battleContext.Player.OnChangeMana -= PlayerManaChanged;
        }

        CombatManager.Instance.onSetupBattle -= OnSetupBattle;
    }

    private void OnSetupBattle(BattleContext ctx)
    {
        _battleContext = ctx;
        
        SetMaxValue(_battleContext.Player.MaxMana);
        SetValue(_battleContext.Player.CurrentMana);
        _battleContext.Player.OnChangeMana += PlayerManaChanged;
    }

    private void PlayerManaChanged(Combatant player, int damage)
    {
        SetValue(player.CurrentMana);
    }
}