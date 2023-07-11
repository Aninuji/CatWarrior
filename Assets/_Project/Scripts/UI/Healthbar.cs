using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Healthbar : MonoBehaviour
{

    [SerializeField]
    private TMP_Text _text;

    [SerializeField]
    private Image _bar;

    [SerializeField]
    private Gradient _color;

    public void setHealthUI(int currentHealth, int maxHealth)
    {
        _text.text = currentHealth + "/" + maxHealth;
        float normalizedHealth = (float)currentHealth / (float)maxHealth;
        Debug.Log("Normal: "+normalizedHealth);
        _bar.rectTransform.localScale = new Vector3(normalizedHealth, 1, 1);
        _bar.color = _color.Evaluate(normalizedHealth);
    }
}
