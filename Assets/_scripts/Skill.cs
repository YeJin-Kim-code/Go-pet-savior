using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
 
    public int lowDamage;
    public int highDamage;
    public int useMp;
    public void PetSkillSet(int index)//스킬 번호 받고 그 스킬의 인포 꺼내주는 함수
    {
        lowDamage = DB_petsSkill.GetEntity(index).lowDamage;
        highDamage = DB_petsSkill.GetEntity(index).highDamage;
        useMp = DB_petsSkill.GetEntity(index).useMp;
    }

    public void skillAniSound(int index)//스킬당 애니메이션과 사운드 연결
    {
        
    }
}
