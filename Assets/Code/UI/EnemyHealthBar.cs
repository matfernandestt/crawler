public class EnemyHealthBar : HealthBar
{
    private BattleContext _battleContext;
    
    private void Start()
    {
        CombatManager.Instance.onSetupBattle += OnSetupBattle;
        CombatManager.Instance.onBattleEnded += OnBattleEnded;
        
        slider.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if(_battleContext != null)
            _battleContext.Enemy.OnTakeDamage -= EnemyOnOnTakeDamage;
        CombatManager.Instance.onSetupBattle -= OnSetupBattle;
        CombatManager.Instance.onBattleEnded -= OnBattleEnded;
    }

    private void OnSetupBattle(BattleContext ctx)
    {
        _battleContext = ctx;
        
        SetMaxValue(_battleContext.Enemy.MaxHealth);
        SetValue(_battleContext.Enemy.CurrentHealth);
        _battleContext.Enemy.OnTakeDamage += EnemyOnOnTakeDamage;
        
        slider.gameObject.SetActive(true);
    }

    private void EnemyOnOnTakeDamage(Combatant enemy, int damage)
    {
        SetValue(enemy.CurrentHealth);
        
        if(enemy.CurrentHealth <= 0)
            slider.gameObject.SetActive(false);
    }
    
    private void OnBattleEnded()
    {
        slider.gameObject.SetActive(false);
    }
}