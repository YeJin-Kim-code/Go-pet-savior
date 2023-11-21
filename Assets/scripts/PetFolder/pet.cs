using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pet : Character
{
    private List<System.Action> skillFunctions = new List<System.Action>();
    private void Start()
    {

        SetPetChar();
        animator = this.GetComponent<Animator>();
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

    // 스킬 실행 함수
    void ExecuteSkill(int index)
    {
        if (index >= 0 && index < skillFunctions.Count)
        {
            // 해당 인덱스의 스킬 함수를 호출
            skillFunctions[index]?.Invoke();
        }
        else
        {
            Debug.LogError("Invalid skill index: " + index);
        }
    }
    public void SetSkill()
    {
        
    }
    // 각각의 스킬 함수들 함수를 만들어서 애니메이션 적용할 애, 스킬번호, 이팩트 줄 위치 매개변수로 받자
    void catSkill1()
    {
        Debug.Log("Executing Skill 1");
        // 스킬 1에 대한 애니메이션 및 연출 코드 추가
    }

    void Skill2()
    {
        Debug.Log("Executing Skill 2");
        // 스킬 2에 대한 애니메이션 및 연출 코드 추가
    }

}

