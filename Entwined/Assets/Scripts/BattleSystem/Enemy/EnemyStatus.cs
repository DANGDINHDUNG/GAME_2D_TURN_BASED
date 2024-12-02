using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyStatus : CharacterStatus
{
    #region Defines
    [SerializeField] private GameObject floatingDamagePrefab;       // Hiển thị damage quái nhận vào.
    [SerializeField] private GameObject TargetIcon;     // Hiển thị đánh dấu khi quái bị chọn làm mục tiêu tấn công.
    [SerializeField] private int currentDefend;     // Phòng thủ hiện tại của quái.
    #endregion

    #region Properties
    public int CurrentDefend => currentDefend;
    #endregion

    #region Core MonoBehaviour
    private void Start()
    {
        CharacterBase enemyBase = GetComponent<CharacterBase>();

        floatingHealthBar = GetComponentInChildren<FloatingBar>();
        currentHp = GetComponent<CharacterBase>().BaseHp;
        currentDefend = 200 + 10 * GetComponent<CharacterBase>().Level;
        floatingHealthBar.SetMaxValue(currentHp);

        if (enemyBase.Level <= 80)
        {
            currentHp = enemyBase.Level * enemyBase.Level + 5 * enemyBase.Level + enemyBase.BaseHp;
        }
        else
        {
            currentHp = 20 * (int)Mathf.Pow((float)(enemyBase.Level - 60), 2) + enemyBase.BaseHp;
        }
    }

    private void Update()
    {
        if (FieldManager.Instance.TargetEnemy == this.gameObject)
        {
            TargetIcon.SetActive(true);
        }
        else
        {
            TargetIcon.SetActive(false);
        }
    }
    #endregion

    #region Public Methods

    /// <summary>
    /// Trừ lượng máu hiện tại của quái khi nhận damage từ người chơi.
    /// </summary>
    /// <param name="damage"></param>
    public override void TakeDamage(Tuple<int,bool> damage)
    {
        currentHp -= damage.Item1;
        floatingHealthBar.SetValue(currentHp);
        if (currentHp <= 0)
        {
            // Gán complêt bằng true để kết thúc lượt khi quái chết.
            this.gameObject.GetComponent<EnemyBattle>().IsCompleted = true;
            BattleHandler.instance.participants.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
        StartCoroutine(GetHit());
        ShowFoatingDamage(damage);
    }

    private void ShowFoatingDamage(Tuple<int, bool> damage)
    {
        var go = Instantiate(floatingDamagePrefab, transform.position, Quaternion.identity);
        go.GetComponent<TextMeshPro>().text = damage.Item1.ToString();

        if (damage.Item2)
        {
            go.GetComponent<TextMeshPro>().color = Color.red;
            go.GetComponent<TextMeshPro>().fontWeight = FontWeight.Bold;
            go.GetComponent<TextMeshPro>().fontSize = 10;
        }
    }

    // Nhân vật hiển thị màu đỏ khi bị tấn công.
    IEnumerator GetHit()
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
    }
    #endregion
}
