using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Camera mainCamera;
    public DataPanelConnect m_dataPanel;
    private void Start()
    {
        mainCamera = Camera.main;
        m_dataPanel = GameObject.FindObjectOfType<DataPanelConnect>();
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
                int charIndex = 0; // �⺻��

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
}
