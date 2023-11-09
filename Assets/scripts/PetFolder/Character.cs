using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Camera mainCamera;
    public DataPanelConnect m_dataPanel;
    public int charIndex = 0; // �⺻��
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
        ClickCheck();
    }

    public void ClickCheck()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//ȭ���� ��ǥ�踦 ���� ��ǥ��� ��ȯ���ִ� �Լ�(ex. 100 x 100 �ػ��� ��� ��� ��ǥ�� ��ũ����ǥ�� ��Ÿ���� 50,50 ������ ������ǥ�� 0,0 �̴�.)			
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if (hit.collider != null)
            {

                string objectTag = hit.collider.gameObject.tag;

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
                m_dataPanel.DisplayCharInfo(charIndex);
            }
        }
    }

    public void SetPetChar()//�� charindex�� ����Ʈ�� �ѹ��� mp,hp���� ����
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
        this.MaxMP = DB_petInfo.GetEntity(charIndex).mp;
        this.currentMP = DB_petInfo.GetEntity(charIndex).mp;
    }

    //�ֳʹ� ��ũ��Ʈ�� ���λ������ϱ�
    //mp������ �ʿ���⵵�ϰ� �� �Լ��� ��ġ�� �ڵ尡 ����
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


    //�����ؾ�
    public int MaxHP
    {
        get { return maxHP; }
        set { maxHP = value; }
    }

    public int CurrentHP
    {
        get { return currentHP; }
        set { currentHP = Mathf.Clamp(value, 0, maxHP); }
    }

    public int MaxMP
    {
        get { return maxMP; }
        set { maxMP = value; }
    }

    public int CurrentMP
    {
        get { return currentMP; }
        set { currentMP = Mathf.Clamp(value, 0, maxMP); }
    }

}
