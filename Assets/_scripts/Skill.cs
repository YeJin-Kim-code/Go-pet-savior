using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
 
    public int lowDamage;
    public int highDamage;
    public int useMp;
    public void PetSkillSet(int index)//��ų ��ȣ �ް� �� ��ų�� ���� �����ִ� �Լ�
    {
        lowDamage = DB_petsSkill.GetEntity(index).lowDamage;
        highDamage = DB_petsSkill.GetEntity(index).highDamage;
        useMp = DB_petsSkill.GetEntity(index).useMp;
    }

    public void skillAniSound(int index)//��ų�� �ִϸ��̼ǰ� ���� ����
    {
        
    }
}
