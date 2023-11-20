using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DataPanelConnect : MonoBehaviour
{
    public Image petImage;
    public TextMeshProUGUI petName;
    public TextMeshProUGUI petHp;//gameManager의 currentHp 싱글톤으로 작업
    public TextMeshProUGUI petMp;//gameManager의 currentMp 싱글톤으로 작업
    public Button petSkill1;
    public Button petSkill2;
    public Button petSkill3;
    public TextMeshProUGUI petSkillDescript;
    public Slider controlSlider;
    public TextMeshProUGUI controlText;
    public TextMeshProUGUI SkillDamageText;
    public TextMeshProUGUI skillUseHpText;
    public TextMeshProUGUI skillUseMpText;


    public int charIndex;//일단 지정 추후 수정
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
        //캐릭터 이미지
        petImage.sprite = DB_petInfo.GetEntity(_charIndex).petFaceImage;
        //캐릭터이름 
        petName.text = DB_petInfo.GetEntity(_charIndex).animalName;
        //캐릭터hp
        petHp.text = "HP : " + GameManager.Instance.playerCharacters[GameManager.Instance.currentPlayerIndex].currentHP + "/" + DB_petInfo.GetEntity(_charIndex).hp.ToString();//추후 게임메니져에서 currnetHp가져오기 계산은 각 캐릭터 스크립트에서
        //캐릭터mp
        petMp.text = "MP : "+ GameManager.Instance.playerCharacters[GameManager.Instance.currentPlayerIndex].currentMP + "/" + DB_petInfo.GetEntity(_charIndex).mp.ToString();//추후 게임메니져에서 currnetmp가져오기 게임매니져에서 
        //캐릭터 스킬1
        petSkill1.image.sprite = DB_petInfo.GetEntity(_charIndex).skillOne;
        //캐릭터 스킬2
        petSkill2.image.sprite = DB_petInfo.GetEntity(_charIndex).skillTwo;
        //캐릭터 스킬3
        petSkill3.image.sprite = DB_petInfo.GetEntity(_charIndex).skillThree;
    }

    public void DisplayCharSkillInfo(int _skillIndex)//스킬인덱스와 스킬조절바 수를 매개변수로 int _skillControlBar
    {
        controlText.gameObject.SetActive(true);
        controlSlider.gameObject.SetActive(true);
        SkillDamageText.gameObject.SetActive(true);
        skillUseMpText.gameObject.SetActive(true);
        skillUseHpText.gameObject.SetActive(true);
        skillIndex = _skillIndex + charIndex * skillCount;
        isSkillPanelDisplay = true;
        //컨트롤
        controlText.text = "Skill Control : "+GameManager.Instance.slider.slideValue.ToString();
        //스킬묘사
        petSkillDescript.text = DB_petsSkill.GetEntity(skillIndex).skillContent;
        //스킬에따른 데미지
        SkillDamageText.text = "Damage : " + DB_petsSkill.GetEntity(skillIndex).lowDamage.ToString() + " ~ " + DB_petsSkill.GetEntity(skillIndex).highDamage.ToString();
        //스킬 mp
        skillUseMpText.text = "Mp : " + DB_petsSkill.GetEntity(skillIndex).useMp.ToString();
        //스킬 hp
        skillUseHpText.text = "Hp : " + DB_petInfo.GetEntity(charIndex).hp.ToString() + " -> " + DB_petInfo.GetEntity(charIndex).hp.ToString();
    }
    public void BeforeChooseSkill()
    {
        isSkillPanelDisplay=false;
        controlText.gameObject.SetActive(false);
        //컨트롤 초기화
        controlSlider.value = 0;
        controlSlider.gameObject.SetActive(false);
        petSkillDescript.text = "스킬을 선택하세요";
        SkillDamageText.gameObject.SetActive(false);
        skillUseMpText.text = "스킬을 선택하세요";
        skillUseHpText.gameObject.SetActive(false);
    }
    public void SkillControlBarUpdate()
    {
        if(isSkillPanelDisplay)
        {
            controlText.text = "Skill Control : " + GameManager.Instance.slider.slideValue.ToString();
            //스킬에따른 데미지
            SkillDamageText.text = "Damage : " + DB_petsSkill.GetEntity(skillIndex).lowDamage.ToString() + " ~ " + (DB_petsSkill.GetEntity(skillIndex).highDamage+ GameManager.Instance.slider.slideValue).ToString();
            //스킬 hp
            skillUseHpText.text = "Hp : " + GameManager.Instance.playerCharacters[charIndex].currentHP.ToString() + " -> " + (GameManager.Instance.playerCharacters[charIndex].currentHP - GameManager.Instance.slider.slideValue).ToString();
        }

    }

    public void TestButton()
    {
        DisplayCharInfo(0);
    }
}
