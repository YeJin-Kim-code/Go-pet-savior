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
    public Slider controlSlider;
    public TextMeshProUGUI controlText;
    public TextMeshProUGUI SkillDamageText;
    public TextMeshProUGUI skillUseHpText;
    public TextMeshProUGUI skillUseMpText;


    public int charIndex;//�ϴ� ���� ���� ����
    public int skillCount = 3;
    public int skillIndex = -1;

    public bool isSkillPanelDisplay = false;
    private void Start()
    {
        BeforeChooseSkill();
    }
    private void Update()
    {
        SkillControlBarUpdate();
    }
    public void DisplayCharInfo(int _charIndex)
    {
        charIndex = _charIndex;
        //ĳ���� �̹���
        petImage.sprite = DB_petInfo.GetEntity(_charIndex).petFaceImage;
        //ĳ�����̸� 
        petName.text = DB_petInfo.GetEntity(_charIndex).animalName;
        //ĳ����hp
        petHp.text = "HP : " + GameManager.Instance.playerCharacters[GameManager.Instance.currentPlayerIndex].currentHP + "/" + DB_petInfo.GetEntity(_charIndex).hp.ToString();//���� ���Ӹ޴������� currnetHp�������� ����� �� ĳ���� ��ũ��Ʈ����
        //ĳ����mp
        petMp.text = "MP : "+ GameManager.Instance.playerCharacters[GameManager.Instance.currentPlayerIndex].currentMP + "/" + DB_petInfo.GetEntity(_charIndex).mp.ToString();//���� ���Ӹ޴������� currnetmp�������� ���ӸŴ������� 
        //ĳ���� ��ų1
        petSkill1.image.sprite = DB_petInfo.GetEntity(_charIndex).skillOne;
        //ĳ���� ��ų2
        petSkill2.image.sprite = DB_petInfo.GetEntity(_charIndex).skillTwo;
        //ĳ���� ��ų3
        petSkill3.image.sprite = DB_petInfo.GetEntity(_charIndex).skillThree;
    }

    public void DisplayCharSkillInfo(int _skillIndex)//��ų�ε����� ��ų������ ���� �Ű������� int _skillControlBar
    {
        controlText.gameObject.SetActive(true);
        controlSlider.gameObject.SetActive(true);
        SkillDamageText.gameObject.SetActive(true);
        skillUseMpText.gameObject.SetActive(true);
        skillUseHpText.gameObject.SetActive(true);
        skillIndex = _skillIndex + charIndex * skillCount;
        isSkillPanelDisplay = true;
        //��Ʈ��
        controlText.text = "Skill Control : "+GameManager.Instance.slider.slideValue.ToString();
        //��ų����
        petSkillDescript.text = DB_petsSkill.GetEntity(skillIndex).skillContent;
        //��ų������ ������
        SkillDamageText.text = "Damage : " + DB_petsSkill.GetEntity(skillIndex).lowDamage.ToString() + " ~ " + DB_petsSkill.GetEntity(skillIndex).highDamage.ToString();
        //��ų mp
        skillUseMpText.text = "Mp : " + DB_petsSkill.GetEntity(skillIndex).useMp.ToString();
        //��ų hp
        skillUseHpText.text = "Hp : " + DB_petInfo.GetEntity(charIndex).hp.ToString() + " -> " + DB_petInfo.GetEntity(charIndex).hp.ToString();
    }
    public void BeforeChooseSkill()
    {
        isSkillPanelDisplay=false;
        controlText.gameObject.SetActive(false);
        //��Ʈ�� �ʱ�ȭ
        controlSlider.value = 0;
        controlSlider.gameObject.SetActive(false);
        petSkillDescript.text = "��ų�� �����ϼ���";
        SkillDamageText.gameObject.SetActive(false);
        skillUseMpText.text = "��ų�� �����ϼ���";
        skillUseHpText.gameObject.SetActive(false);
    }
    public void SkillControlBarUpdate()
    {
        if(isSkillPanelDisplay)
        {
            controlText.text = "Skill Control : " + GameManager.Instance.slider.slideValue.ToString();
            //��ų������ ������
            SkillDamageText.text = "Damage : " + DB_petsSkill.GetEntity(skillIndex).lowDamage.ToString() + " ~ " + (DB_petsSkill.GetEntity(skillIndex).highDamage+ GameManager.Instance.slider.slideValue).ToString();
            //��ų hp
            skillUseHpText.text = "Hp : " + GameManager.Instance.playerCharacters[charIndex].currentHP.ToString() + " -> " + (GameManager.Instance.playerCharacters[charIndex].currentHP - GameManager.Instance.slider.slideValue).ToString();
        }

    }

    public void TestButton()
    {
        DisplayCharInfo(0);
    }
}
