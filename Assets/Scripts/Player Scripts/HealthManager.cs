using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{

    [SerializeField]
    private int maxHP = 20;
    public virtual int MaxHP
    {
        get { return maxHP; }
        set { maxHP = value; }
    }
    public int HP;
    private GameObject parent;
    [SerializeField] private Image healthBar;

    // Start is called before the first frame update
    void Start()
    {
        MaxHP = maxHP;
        HP = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
        {
            Die();
        }

        if (healthBar != null)
        {
            healthBar.fillAmount = (float)HP/MaxHP;
            //Debug.Log("HP: " + HP + " Max HP: " + MaxHP + " Ratio: "+ (HP / MaxHP));
        }
    }

    private void Die()
    {
        Player _player = GetComponentInParent<Player>();
        if (_player != null)
        {
            
        }

        else
        {

        }
    }

    public void UpdateHealthBar()
    {

    }
}
