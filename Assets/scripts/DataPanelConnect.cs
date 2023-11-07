using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DataPanelConnect : MonoBehaviour
{
    public Image petImage;
    public TextMeshProUGUI petName;
    public TextMeshProUGUI petHp;//gameManager�� currentHp �̱������� �۾�
    public TextMeshProUGUI petMp;//gameManager�� currentMp �̱������� �۾�
    public Button petSkill1;
    public Button petSkill2;
    public Button petSkill3;
    public TextMeshProUGUI petSkillDescript;
    public TextMeshProUGUI controlText;
    public TextMeshProUGUI SkillDamageText;
    public TextMeshProUGUI skillUseHpText;
    public TextMeshProUGUI skillUseMpText;

    public int charIndex;//�ϴ� ���� ���� ����
    private void Start()
    {

    }
    public void DisplayCharInfo(int _charIndex)
    {
        charIndex = _charIndex;
        //ĳ���� �̹���
        petImage.sprite = DB_petInfo.GetEntity(_charIndex).petFaceImage;
        //ĳ�����̸� 
        petName.text = DB_petInfo.GetEntity(_charIndex).animalName;
        //ĳ����hp
        petHp.text = "HP : current/" + DB_petInfo.GetEntity(_charIndex).hp.ToString();//���� ���Ӹ޴������� currnetHp�������� ����� �� ĳ���� ��ũ��Ʈ����
        //ĳ����mp
        petMp.text = "MP : current/" + DB_petInfo.GetEntity(_charIndex).mp.ToString();//���� ���Ӹ޴������� currnetmp�������� ���ӸŴ������� 
        //ĳ���� ��ų1
        petSkill1.image.sprite = DB_petInfo.GetEntity(_charIndex).skillOne;
        //ĳ���� ��ų2
        petSkill2.image.sprite = DB_petInfo.GetEntity(_charIndex).skillTwo;
        //ĳ���� ��ų3
        petSkill3.image.sprite = DB_petInfo.GetEntity(_charIndex).skillThree;
    }

    public void DisplayCharSkillInfo(int _skillIndex)//��ų�ε����� ��ų������ ���� �Ű������� int _skillControlBar
    {
        //��ų����
        petSkillDescript.text = DB_petsSkill.GetEntity(_skillIndex).skillContent;
        //��ų������ ������
        SkillDamageText.text = "Damage : " + DB_petsSkill.GetEntity(_skillIndex).lowDamage.ToString() + " ~ " + DB_petsSkill.GetEntity(_skillIndex).highDamage.ToString();
        //��ų mp
        skillUseMpText.text = "Mp : " + DB_petsSkill.GetEntity(_skillIndex).useMp.ToString();
        //��ų hp
        skillUseHpText.text = "Hp : " + DB_petInfo.GetEntity(charIndex).hp.ToString() + " -> " + DB_petInfo.GetEntity(charIndex).hp.ToString();
    }

    public void SkillControlBarUpdate(int _skillControlBar)
    {
        //��ų�� ���� ������
        
        //��ų hp
    }

    public void TestButton()
    {
        DisplayCharInfo(0);
    }
}
