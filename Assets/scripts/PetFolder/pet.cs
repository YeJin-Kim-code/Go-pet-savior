using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pet : Character
{

    private void Start()
    {

        SetPetChar();
    }

    private void Update()
    {
        //ClickCheck();
    }

    //public void ClickCheck()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//ȭ���� ��ǥ�踦 ���� ��ǥ��� ��ȯ���ִ� �Լ�(ex. 100 x 100 �ػ��� ��� ��� ��ǥ�� ��ũ����ǥ�� ��Ÿ���� 50,50 ������ ������ǥ�� 0,0 �̴�.)			
    //        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
    //        if (hit.collider != null)
    //        {

    //            string objectTag = hit.collider.gameObject.tag;
    //            int charIndex = 0; // �⺻��

    //            switch (objectTag)
    //            {
    //                case "cat":
    //                    charIndex = 0;
    //                    break;
    //                case "duck":
    //                    charIndex = 1;
    //                    break;
    //                case "turtle":
    //                    charIndex = 2;
    //                    break;
    //                case "dog":
    //                    charIndex = 3;
    //                    break;
    //            }
    //        }
    //    }
    //}
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
}

