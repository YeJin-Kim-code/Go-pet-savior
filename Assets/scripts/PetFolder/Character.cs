using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Camera mainCamera;
    public DataPanelConnect m_dataPanel;
    public int charIndex = 0; // 기본값
    public int targetEnemyIndex =  -1;
    public int maxHP;
    public int currentHP;
    public int maxMP;
    public int currentMP;

    private void Start()
    {
        mainCamera = Camera.main;
        m_dataPanel = GameObject.FindObjectOfType<DataPanelConnect>();
        SetPetChar();
        SetEnemyChar();
    }

    private void Update()
    {
        ClickCheckEnemy();
    }

    public void ClickCheckEnemy()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//화면의 좌표계를 월드 좌표계로 전환해주는 함수(ex. 100 x 100 해상도의 경우 가운데 좌표가 스크린좌표로 나타내면 50,50 이지만 월드좌표는 0,0 이다.)			
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if (hit.collider != null)
            {

                string objectTag = hit.collider.gameObject.tag;

                switch (objectTag)
                {
                    case "EnemyBig":
                        targetEnemyIndex = 0;
                        Debug.Log("this enemy is 0");
                        break;
                    case "EnemyNormal":
                        targetEnemyIndex = 1;
                        Debug.Log("this enemy is 1");
                        break;
                    case "EnemySmall":
                        targetEnemyIndex = 2;
                        Debug.Log("this enemy is 2");
                        break;
                    case "EnemyHeal":
                        targetEnemyIndex = 3;
                        Debug.Log("this enemy is 3");
                        break;
                }
            }
        }
    }

    public void SetPetChar()//이 charindex를 리스트의 넘버로 mp,hp정보 저장, 초기화
    {
        string objectTag = this.gameObject.tag;
        switch (objectTag)
        {
            case "cat":
                charIndex = 0;
                break;
            case "duck":
                charIndex = 1;
                break;
            case "turtle":
                charIndex = 2;
                break;
            case "dog":
                charIndex = 3;
                break;
        }
        this.maxHP = DB_petInfo.GetEntity(charIndex).hp;
        this.currentHP = DB_petInfo.GetEntity(charIndex).hp;
        this.maxMP = DB_petInfo.GetEntity(charIndex).mp;
        this.currentMP = DB_petInfo.GetEntity(charIndex).mp;
    }

    //애너미 스크립트를 따로빼던가하기
    //mp변수는 필요없기도하고 펫 함수와 겹치는 코드가 많음
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
