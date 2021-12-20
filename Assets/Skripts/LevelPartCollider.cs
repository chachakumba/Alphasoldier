using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class LevelPartCollider : MonoBehaviour
{
    public int colIndex;
    public bool isPartActive;
    public ContactFilter2D playerFilter;
    Collider2D col;
    public LevelPartsDisabler levelDis;
    private void Awake()
    {
        playerFilter = Manager.instance.playerFilter;
        col = GetComponent<Collider2D>();
    }
    private void Start()
    {
        RaycastHit2D[] hit = new RaycastHit2D[1];
        if (col.Cast(Vector2.zero, playerFilter, hit) > 0)
        {
                if (hit[0].collider.GetComponentInChildren<Player>() != null)
                {
                    isPartActive = true;
                    levelDis.CheckParts();
                }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isPartActive = true;
        levelDis.CheckParts();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isPartActive = false;
        levelDis.CheckParts();
    }
}
