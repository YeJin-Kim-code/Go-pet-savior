using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour
{

    public TextMeshProUGUI turnText;
    public GameObject gameClearPanel;
    public GameObject gameOverPanel;

    public Character character;
    public DataPanelConnect dataPanelConnect;
    public SliderScript slider;
    public SoundManager soundManager;
    public List<Character> playerCharacters;
    public List<Character> enemyCharacters;
    public List<GameObject> effectGameObjects;

    public int currentPlayerIndex = 0;
    public int currentEnemyIndex = 0;
    private int currentTurn = 1;
    private float m_animationTime = 2f;
    private int m_damageRandomResult;

    private bool m_playerTurn = true;
    private bool m_enemyTurn = false;

    void Start()
    {
        //배경음 나중에 수정
        //soundManager.BgSoundPlay(soundManager.background);
        // 게임 시작 초기화 및 캐릭터 생성
        //코루틴으로 
        // playerCharacters와 enemyCharacters 리스트에 캐릭터 추가
        // 일단은 수동으로 놓았지만 코드로 작성해보기
        m_playerTurn = true;
        Debug.Log(playerCharacters[0].maxHP);
    }

    private void Update()
    {
        PlayGame();
    }
    public void PlayGame()
    {
        if(m_playerTurn == true)//아군 턴일시
        {
            while (playerCharacters[currentPlayerIndex].deadCheck)
            {
                currentPlayerIndex++;
                if(currentPlayerIndex>4)
                {
                    currentPlayerIndex = 0;
                }
                Debug.Log(currentPlayerIndex);
            }
            dataPanelConnect.DisplayCharInfo(currentPlayerIndex);//패널 배치
            if(dataPanelConnect.skillIndex != -1)//스킬 버튼이 눌렸다면, 턴끝나면 스킬 값 -1로 초기화
            {
                //동그란원 그거 스킬 누르고 나서 부터 마우스 올리면 뜨기
                //만약 스킬인덱스가 전체공격, 자힐, 타힐 등등이면 스킬 인덱스에 따라 마우스 원 올릴 수 있는 애들 제한
                //Debug.Log(dataPanelConnect.skillIndex);
                if(dataPanelConnect.skillIndex == 2)
                {
                    if (character.targetPetIndex == 0)
                    {
                        StartCoroutine(PlayerAttack(dataPanelConnect.skillIndex, character.targetPetIndex));
                    }
                }
                else if(dataPanelConnect.skillIndex == 7 || dataPanelConnect.skillIndex == 6) //힐, 자힐, 방어 스킬이면
                {
                    if(character.targetPetIndex != -1)
                    {
                        StartCoroutine(PlayerAttack(dataPanelConnect.skillIndex, character.targetPetIndex));
                    }

                }
                else
                {
                    if(character.targetEnemyIndex != -1)
                    {
                        StartCoroutine(PlayerAttack(dataPanelConnect.skillIndex, character.targetEnemyIndex));
                    }
                }

            }

            //playerCharacters[currentPlayerIndex]
        }
        else if (m_enemyTurn == true)//적군 턴일시
        {
            StartCoroutine(EnemyAttack());

        }
    }

    IEnumerator PlayerAttack(int skillIndex, int targetEnemy)//플레이어 turn
    {
        //mp 부족한지 체크
        if(playerCharacters[currentPlayerIndex].currentMP<= DB_petsSkill.GetEntity(skillIndex).useMp)
        {
            //경고창 띄우고 넘어가기
            
            Debug.Log("mp가 부족");
        }
        else
        {
            //update문 재실행 차단
            m_playerTurn = false;
            //데미지, mp 조절

            //공격 애니메이션
            LoadSkillAniAndEffect(playerCharacters[currentPlayerIndex], dataPanelConnect.skillIndex);


            //데미지 슬라이더 까지 적용
            petSkillDamage(skillIndex, targetEnemy);
            playerCharacters[currentPlayerIndex].currentHP -= slider.slideValue;
            playerCharacters[currentPlayerIndex].currentMP -= DB_petsSkill.GetEntity(skillIndex).useMp;//mp 차감

            //사운드
            soundManager.SFXPlay(soundManager.petSkillSound[skillIndex]);
            yield return new WaitForSeconds(m_animationTime);

            TurnPass();
            //사망감지
            DeadCheck(enemyCharacters[targetEnemy].currentHP, enemyCharacters[targetEnemy]);//죽음감지
            if (AreAllCharactersDead(enemyCharacters) == true)//모두 죽었는지 감지
            {
                Debug.Log("enemy all dead");
                //코루틴으로 승리 표현 애니메이션
                //화면멈추기
                //승리패널 띄우기 - 재화 나가기 다시하기
                gameClearPanel.SetActive(true);
                m_playerTurn = false;
                m_enemyTurn = false;
            }
        }
    }

    IEnumerator EnemyAttack()//데미지 받기
    {
        m_enemyTurn = false;
        int randomValue = Random.Range(0, 4);
        int EnemyRandomDamage = 0;
        while (playerCharacters[randomValue].deadCheck)//플레이어 죽으면 다른 애 공격
        {
            randomValue = Random.Range(0, 4);
        }
        
        character.targetCheckCIrcle.transform.position = playerCharacters[randomValue].transform.position;//선택 표시하기
        EnemyRandomDamage = DamageCalculate(DB_enemySkill.GetEntity(currentEnemyIndex).lowDamage, DB_enemySkill.GetEntity(currentEnemyIndex).highDamage + 1);//데미지 랜덤값 주기
        playerCharacters[randomValue].currentHP -= EnemyRandomDamage;

        //연출
        LoadSkillAniEnemy(enemyCharacters[currentEnemyIndex]);
        DamageAni(playerCharacters[randomValue]);
        dataPanelConnect.ShowDamageText(EnemyRandomDamage, playerCharacters[randomValue].transform.position);
        StartCoroutine(GetDamageTurnRed(playerCharacters[randomValue]));
        yield return new WaitForSeconds(m_animationTime);

        character.targetCheckCIrcle.transform.position = character.checkCircleDefaultPosition;

        currentEnemyIndex += 1;
        ChangeTurnText();
        m_playerTurn = true;

        DeadCheck(playerCharacters[randomValue].currentHP, playerCharacters[randomValue]);
        //죽으면 선택하지 않게
        if(AreAllCharactersDead(playerCharacters) == true)//모두 죽었는지 감지
        {
            Debug.Log("pet all dead");
            //코루틴으로 패배 표현 애니메이션
            //화면멈추기
            //패배패널 띄우기
            gameOverPanel.SetActive(true);
            m_playerTurn = false;
            m_enemyTurn = false;
        }

        if (currentEnemyIndex >= 4)
        {
            currentEnemyIndex = 0;
        }
    }
    public void petSkillDamage(int skillIndex,int targetEnemy)
    {
        if (skillIndex == 0 || skillIndex == 1 || skillIndex == 3 || skillIndex == 9 || skillIndex == 10)//스킬이 단일 공격이면
        {
            petDefaultDamage(skillIndex, targetEnemy);
        }
        else if (skillIndex == 4 || skillIndex == 5)// 다중 공격
        {
            Debug.Log("skillMultiAttack");
            
            for(int i =0; i<4; i++)//매개변수 list 쓰면 list의 길이로 대체
            {
                petDefaultDamage(skillIndex, i);
            }
        }
        else if(skillIndex == 6)
        {
            for (int i = 0; i < 4; i++)//매개변수 list 쓰면 list의 길이로 대체
            {
                petDefaultHeal(skillIndex, i);
            }
        }
        else if(skillIndex == 7 || skillIndex == 2)//단일 힐, 자힐
        {
            petDefaultHeal(skillIndex, targetEnemy);
        }

        else
        {
            Debug.Log("구현 예정");
        }
        //petDefaultDamage(skillIndex, targetEnemy);
    }

    public void petDefaultDamage(int skillIndex, int targetEnemy)//공격 디폴트
    {
        Debug.Assert(skillIndex>=0 && targetEnemy>=0);//항상 참이되어야 매개변수 값이 유효한지 점검
        m_damageRandomResult = DamageCalculate(DB_petsSkill.GetEntity(skillIndex).lowDamage, (DB_petsSkill.GetEntity(skillIndex).highDamage + 1 + slider.slideValue));//랜덤값 공격
        enemyCharacters[targetEnemy].currentHP -= m_damageRandomResult;//에너미 피격
        dataPanelConnect.ShowDamageText(m_damageRandomResult, enemyCharacters[targetEnemy].gameObject.transform.position);//데미지 텍스트 표시
        ShowEffect(skillIndex, enemyCharacters[targetEnemy], effectGameObjects);
        StartCoroutine(GetDamageTurnRed(enemyCharacters[targetEnemy]));
    }
    public void petDefaultHeal(int skillIndex, int targetEnemy)//치유 디폴트
    {
        m_damageRandomResult = DamageCalculate(DB_petsSkill.GetEntity(skillIndex).lowDamage, (DB_petsSkill.GetEntity(skillIndex).highDamage + 1 + slider.slideValue));//랜덤값 공격
        playerCharacters[targetEnemy].currentHP += m_damageRandomResult;//아군 힐
        dataPanelConnect.ShowDamageText(m_damageRandomResult, playerCharacters[targetEnemy].gameObject.transform.position);//데미지 텍스트 표시
        ShowEffect(skillIndex, playerCharacters[targetEnemy], effectGameObjects);
        //StartCoroutine(GetDamageTurnRed(playerCharacters[targetEnemy])); 파랑으로 바껴도 좋을듯
    }
    public int DamageCalculate(int smallDamage, int bigDamage)
    {
        int randomDamageValue = Random.Range(smallDamage, bigDamage);
        Debug.Log(randomDamageValue);
        return randomDamageValue;
    }
    bool AreAllCharactersDead(List<Character> characterList)
    {
        // 모든 캐릭터의 HP가 0 이하인지 확인
        foreach (Character Character in characterList)
        {
            if (Character.currentHP > 0)
            {
                // 아직 생존한 캐릭터가 있다면 false 반환
                return false;
            }
        }

        // 모든 캐릭터가 죽었다면 true 반환
        return true;
    }

    public void ChangeTurnText()//턴 text 연결
    {
        currentTurn += 1;
        turnText.text = currentTurn.ToString();
    }

    public void LoadSkillAniAndEffect(Character petchar, int skillIndex)// Character target)//애니메이션 및 스킬 이팩트 관리
    {
        if(skillIndex!=4||skillIndex!=8)// 지금 애니메이션 나온 수
        { 
            petchar.animator.SetTrigger("Attack"+skillIndex);
            Debug.Log("animation");
        }

        //타겟 포지션에 이팩트
    }

    public void LoadSkillAniEnemy(Character enemyChar)
    {
        enemyChar.animator.SetTrigger("Attack");
    }
    public void DamageAni(Character chara)
    {
        chara.animator.SetTrigger("Damage");
    }
    public void ShowEffect(int skillIndex, Character enemychar, List<GameObject> effect)//이팩트
    {
        if(effect[skillIndex] != null)
        {
            GameObject effectobj = Instantiate(effect[skillIndex], enemychar.transform.position, Quaternion.identity);
            Destroy(effectobj, 1f);
        }
    }

    IEnumerator GetDamageTurnRed(Character damagedChar)
    {
        Renderer render = damagedChar.GetComponent<Renderer>();
        int countTime = 0;
        while(countTime < 3)
        {
            if(countTime%2 ==0)
            {
                render.material.color = Color.red;
                Debug.Log("red");
            }
            else
            {
                render.material.color = Color.white;
                Debug.Log("white");
            }

            yield return new WaitForSeconds(0.2f);
            countTime++;
        }
        render.material.color = Color.white;

    }
    public void DeadCheck(int hp, Character targetObject)//죽음감지
    {
        if (hp <= 0)//타겟 캐릭터 hp가 0이면
        {
            Renderer renderer = targetObject.GetComponent<Renderer>();
            renderer.material.color = Color.black;
            targetObject.deadCheck = true;
        }
    }

    public void StopTouch()//에너미 턴일시 화면터치 불가
    {
        
    }
    public void TurnPass()//턴 pass 버튼
    {
        m_playerTurn = false;
        //초기화
        dataPanelConnect.BeforeChooseSkill();//스킬패널 초기화
        dataPanelConnect.skillIndex = -1; //스킬인덱스 초기화
        character.targetEnemyIndex = -1; //에너미 타겟 인덱스 초기화
        character.targetPetIndex = -1;
                     
        character.targetCheckCIrcle.transform.position = character.checkCircleDefaultPosition;//체크 원을 원래자리로

        soundManager.SFXPlay(soundManager.passButton);
        currentPlayerIndex += 1; //현재 플레이어 턴 설정
        ChangeTurnText();//지금이 몇번째 turn 인지 표시
        m_enemyTurn = true;
        //인덱스 관리
        if (currentPlayerIndex >= 4)
        {
            currentPlayerIndex = 0;
        }
    }

    public void reBorn(List<Character> characterList)//부활 again 등 다시 체력이 찬다면
    {
        foreach (Character Character in characterList)
        {
            if (Character.currentHP > 0)
            {
                Renderer renderer = Character.GetComponent<Renderer>();
                renderer.material.color = Color.white;

            }
        }
    }
    public void Restart()
    {
        //체력, mp 다시 배치, deadcheck변수 초기화
        foreach (Character player in playerCharacters)
        {
            if (player is pet)
            {
                ((pet)player).SetPetChar();
            }
            player.deadCheck = false;

        }
        foreach (Character enemy in enemyCharacters)
        {
            if (enemy is Enemy)
            {
                ((Enemy)enemy).SetEnemyChar();
            }

        }
        //체력 높아지면 색깔 살려주고
        reBorn(enemyCharacters);
        reBorn(playerCharacters);
        //턴 초기화
        m_playerTurn = true;
        m_enemyTurn = false;
        currentPlayerIndex = 0;
        currentEnemyIndex = 0;
        currentTurn =0;
        //패널 닫기
        gameClearPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        //턴 표시 업댓
        ChangeTurnText();
    }
    private static GameManager instance = null;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //Destroy(this.gameObject);
        }
    }

    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
}

