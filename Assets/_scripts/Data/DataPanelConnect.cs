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
    public TextMeshProUGUI damageTextPrefab;  // 데미지를 표시할 Text UI 프리팹
    public Transform canvasTransform;  // UI가 속할 Canvas의 Transform
    public TextMeshProUGUI showNowHpText;
    public TextMeshProUGUI hpText;
    public int charIndex;//일단 지정 추후 수정
    public int skillCount = 3;
    public int skillIndex = -1;

    public bool isSkillPanelDisplay = false;
    private void Start()
    {
        BeforeChooseSkill();
        hpText = Instantiate(GameManager.Instance.dataPanelConnect.showNowHpText, canvasTransform);
        hpText.gameObject.SetActive(false); // 초기에는 비활성화
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

        GameManager.Instance.soundManager.SFXPlay(GameManager.Instance.soundManager.skillButtonClick);
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
    public void ShowDamageText(int damageAmount, Vector3 position)
    {
        // 데미지 텍스트 생성
        TextMeshProUGUI damageText = Instantiate(damageTextPrefab, canvasTransform);
        if(skillIndex == 6)//힐 예외처리
        {
            damageText.text = "+" + damageAmount.ToString();
        }
        else
        {
            damageText.text = "-" + damageAmount.ToString();
        }

        Vector3 randomOffset = new Vector3(Random.Range(1f, 3f), Random.Range(-1f, 1f), 0f);
        damageText.transform.position = Camera.main.WorldToScreenPoint(position + randomOffset);

        // 일정 시간 후에 텍스트 삭제
        Destroy(damageText.gameObject, 1f);  // 여기서 2초 후에 삭제되도록 설정, 필요에 따라 조절 가능
    }

    public void ShowNowHpText(Vector3 position)
    {
        hpText.gameObject.SetActive(true);
        Vector3 fixPosition = new Vector3(1f, 1f, 0f);
        hpText.transform.position = Camera.main.WorldToScreenPoint(position + fixPosition);

    }

    public void HideNowHpText()
    {
        hpText.gameObject.SetActive(false);
    }

    public void UpdateNowHpText(int currentHp, int maxHp)
    {
        hpText.text = "HP : " + currentHp.ToString() + "/" + maxHp.ToString();
    }
    public void TestButton()
    {
        DisplayCharInfo(0);
    }
}
