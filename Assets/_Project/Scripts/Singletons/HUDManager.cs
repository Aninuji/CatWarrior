using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : SingletonPattern<HUDManager>
{
    public Healthbar healthbar;
    public CharacterHealthController _playerHealth;

    private void Awake()
    {
        base.Init(this, false);
    }

    private void Start()
    {
        _playerHealth = GameManager.Instance._player.GetComponent<CharacterHealthController>();
        _playerHealth.OnDamage += OnPlayerHealthChange;
        _playerHealth.OnHeal += OnPlayerHealthChange;
        healthbar.setHealthUI(_playerHealth.Health, _playerHealth.MaxHealth);
    }
    private void OnPlayerHealthChange() {
        healthbar.setHealthUI(_playerHealth.Health, _playerHealth.MaxHealth);
    }

    private void OnDestroy()
    {
        _playerHealth.OnDamage -= OnPlayerHealthChange;
        _playerHealth.OnHeal -= OnPlayerHealthChange;
    }
}
