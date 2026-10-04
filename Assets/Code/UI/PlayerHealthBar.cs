public class PlayerHealthBar : HealthBar
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
            _battleContext.Player.OnTakeDamage -= PlayerHealthChanged;
            _battleContext.Player.OnHeal -= PlayerHealthChanged;
        }

        CombatManager.Instance.onSetupBattle -= OnSetupBattle;
    }

    private void OnSetupBattle(BattleContext ctx)
    {
        _battleContext = ctx;
        
        SetMaxValue(_battleContext.Player.MaxHealth);
        SetValue(_battleContext.Player.CurrentHealth);
        _battleContext.Player.OnTakeDamage += PlayerHealthChanged;
        _battleContext.Player.OnHeal += PlayerHealthChanged;
    }

    private void PlayerHealthChanged(Combatant player, int damage)
    {
        SetValue(player.CurrentHealth);
    }
}
