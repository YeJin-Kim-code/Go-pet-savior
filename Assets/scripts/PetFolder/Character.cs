using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Character : MonoBehaviour
{

    [SerializeField]
    public Animator animator;
    public int charIndex = 0; // �⺻��
    public int targetEnemyIndex =  -1;
    public int maxHP;
    public int currentHP;
    public int maxMP;
    public int currentMP;
    public bool deadCheck = false;
    public GameObject targetCheckCIrcle;
    public Vector3 checkCircleDefaultPosition = new Vector3(10f, 10f, 10f);
    private void Start()
    {
        //m_dataPanel = GameObject.FindObjectOfType<DataPanelConnect>();
        //SetPetChar();
        //SetEnemyChar();


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
                Vector3 currentPosition = hit.collider.gameObject.transform.position;

                // �������� ���� ��ũ��Ʈ ������Ʈ�� ��ġ�� ����
                //newPrefabInstance = Instantiate(targetCheckCIrcle, currentPosition, currentRotation);
                targetCheckCIrcle.transform.position = currentPosition;

            }
        }
    }

    public void DeadCheck()
    {

    }

}
