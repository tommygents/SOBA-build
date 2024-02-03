using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseDetector : MonoBehaviour
{
    public Player parent;
    // Start is called before the first frame update
    void Start()
    {
       parent = GetComponentInParent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerBase _pb = collision.gameObject.GetComponent<PlayerBase>();
        if (_pb != null)
        {
            parent.inBaseZone = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerBase _pb = collision.gameObject.GetComponent<PlayerBase>();
        if (_pb != null)
        {
            parent.inBaseZone = false;
        }
    }
}
