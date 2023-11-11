using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Camera _mainCamera;
    [SerializeField]
    public DataPanelConnect m_dataPanel;
    public int charIndex = 0; // �⺻��
    public int targetEnemyIndex =  -1;
    public int maxHP;
    public int currentHP;
    public int maxMP;
    public int currentMP;

    public GameObject targetCheckCIrcle;
    public GameObject newPrefabInstance;

    private void Start()
    {
        _mainCamera = Camera.main;
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
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//ȭ���� ��ǥ�踦 ���� ��ǥ��� ��ȯ���ִ� �Լ�(ex. 100 x 100 �ػ��� ��� ��� ��ǥ�� ��ũ����ǥ�� ��Ÿ���� 50,50 ������ ������ǥ�� 0,0 �̴�.)			
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if (hit.collider != null)
            {
                //Destroy(newPrefabInstance);//ǥ�� ������Ʈ ����

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
                // ���� ��ũ��Ʈ ������Ʈ�� ��ġ�� ȸ���� ������
                //Vector3 currentPosition = this.transform.position;
                //Quaternion currentRotation = this.transform.rotation;

                // �������� ���� ��ũ��Ʈ ������Ʈ�� ��ġ�� ����
                //newPrefabInstance = Instantiate(targetCheckCIrcle, currentPosition, currentRotation);
            }
        }
    }

    public void SetPetChar()//�� charindex�� ����Ʈ�� �ѹ��� mp,hp���� ����, �ʱ�ȭ
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

}
