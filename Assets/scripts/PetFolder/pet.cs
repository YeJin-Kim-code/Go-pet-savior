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
    //        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//화면의 좌표계를 월드 좌표계로 전환해주는 함수(ex. 100 x 100 해상도의 경우 가운데 좌표가 스크린좌표로 나타내면 50,50 이지만 월드좌표는 0,0 이다.)			
    //        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
    //        if (hit.collider != null)
    //        {

    //            string objectTag = hit.collider.gameObject.tag;
    //            int charIndex = 0; // 기본값

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
}

