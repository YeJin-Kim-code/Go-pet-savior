using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    // Start is called before the first frame update
    void Start()
    {
        SetEnemyChar();
    }

    public void SetEnemyChar()
    {
        string objectTag = this.gameObject.tag;
        switch (objectTag)
        {
            case "EnemyBig":
                charIndex = 0;
                break;
            case "EnemyNormal":
                charIndex = 1;
                break;
            case "EnemySmall":
                charIndex = 2;
                break;
            case "EnemyHeal":
                charIndex = 3;
                break;
        }
        this.maxHP = DB_enemyInfo.GetEntity(charIndex).hp;
        this.currentHP = DB_enemyInfo.GetEntity(charIndex).hp;
    }
}
