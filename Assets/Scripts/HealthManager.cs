using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    private void Die()
    {
        Player _player = GetComponentInParent<Player>();
        if (_player != null)
        {
            _player.Die();
        }

        else
        {

        }
    }
}
