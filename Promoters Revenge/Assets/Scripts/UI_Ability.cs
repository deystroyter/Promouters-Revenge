using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ability : MonoBehaviour
{
    [SerializeField] private Image _abilityIcon;
    [SerializeField] private TextMeshProUGUI _keyText;
    [SerializeField] private Image _cooldownBackground;

    public float Ñooldown;
    public float ElapsedTime;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void DrawCooldown()
    {
        StartCoroutine(CooldownClocking());
    }

    private IEnumerator CooldownClocking()
    {
        ElapsedTime += Time.deltaTime;
        var x = 1 - ElapsedTime / Ñooldown;
        _cooldownBackground.fillAmount -= x;
        yield return new WaitForSeconds(Time.deltaTime);
    }

    public void SetIcon(Sprite iconSprite)
    {
        _abilityIcon.sprite = iconSprite;
    }

    public void SetKeyText(string key)
    {
        _keyText.text = key;
    }
}