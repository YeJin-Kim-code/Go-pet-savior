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

    public List<Character> playerCharacters;
    public List<Character> enemyCharacters;

    public int currentPlayerIndex = 0;
    public int currentEnemyIndex = 0;
    public int currentTurn = 1;
    public float animationTime = 3f;

    public bool PlayerTurn = true;
    public bool EnemyTurn = false;
    public DataPanelConnect dataPanelConnect;
    void Start()
    {
        
        // 게임 시작 초기화 및 캐릭터 생성
        //코루틴으로 
        // playerCharacters와 enemyCharacters 리스트에 캐릭터 추가
        // 일단은 수동으로 놓았지만 코드로 작성해보기
        PlayerTurn = true;
        Debug.Log(playerCharacters[0].maxHP);
    }

    private void Update()
    {
        PlayGame();
    }
    public void PlayGame()
    {
        if(PlayerTurn == true)//아군 턴일시
        {
            dataPanelConnect.DisplayCharInfo(currentPlayerIndex);//패널 배치
            if(dataPanelConnect.skillIndex != -1)//스킬 버튼이 눌렸다면, 턴끝나면 스킬 값 -1로 초기화
            {
                //Debug.Log(dataPanelConnect.skillIndex);
                if(character.targetEnemyIndex != -1)
                {
                    StartCoroutine(PlayerAttack(dataPanelConnect.skillIndex, character.targetEnemyIndex));
                }
            }

            //playerCharacters[currentPlayerIndex]
        }
        else if (EnemyTurn == true)//적군 턴일시
        {
            StartCoroutine(EnemyAttack());

        }
    }

    IEnumerator PlayerAttack(int skillIndex, int targetEnemy)//플레이어 turn
    {
        //update문 재실행 차단


        //애니메이션 및 기능 추가

        //mp 부족한지 체크
        if(playerCharacters[currentPlayerIndex].currentMP<= DB_petsSkill.GetEntity(skillIndex).useMp)
        {
            //경고창 띄우고 넘어가기
            
            Debug.Log("mp가 부족");
            //PlayerTurn = true;
        }
        else
        {
            PlayerTurn = false;
            //데미지, mp 조절

            enemyCharacters[targetEnemy].currentHP -= DB_petsSkill.GetEntity(skillIndex).highDamage;//에너미 피격
            playerCharacters[currentPlayerIndex].currentMP -= DB_petsSkill.GetEntity(skillIndex).useMp;//mp 차감

            yield return new WaitForSeconds(animationTime);

            //초기화
            dataPanelConnect.BeforeChooseSkill();//스킬패널 초기화
            dataPanelConnect.skillIndex = -1; //스킬인덱스 초기화
            character.targetEnemyIndex = -1; //에너미 타겟 인덱스 초기화
                                             //선택체크
            character.targetCheckCIrcle.transform.position = character.checkCircleDefaultPosition;//체크 원을 원래자리로
                                                                                                  //턴
            currentPlayerIndex += 1; //현재 플레이어 턴 설정
            ChangeTurnText();//지금이 몇번째 turn 인지 표시
            EnemyTurn = true;
            //사망감지
            DeadCheck(enemyCharacters[targetEnemy].currentHP, enemyCharacters[targetEnemy]);//죽음감지
            if (AreAllCharactersDead(enemyCharacters) == true)//모두 죽었는지 감지
            {
                Debug.Log("enemy all dead");
                //코루틴으로 승리 표현 애니메이션
                //화면멈추기
                //승리패널 띄우기 - 재화 나가기 다시하기
                gameClearPanel.SetActive(true);
            }
            //인덱스 관리
            if (currentPlayerIndex >= 4)
            {
                currentPlayerIndex = 0;
            }
        }

    }


    IEnumerator EnemyAttack()//데미지 받기
    {

        EnemyTurn = false;
        int randomValue = Random.Range(0, 4);
        character.targetCheckCIrcle.transform.position = playerCharacters[randomValue].transform.position;//선택 표시하기
        playerCharacters[randomValue].currentHP -= DB_enemySkill.GetEntity(currentEnemyIndex).highDamage;

        yield return new WaitForSeconds(animationTime);

        character.targetCheckCIrcle.transform.position = character.checkCircleDefaultPosition;

        currentEnemyIndex += 1;
        ChangeTurnText();
        PlayerTurn = true;

        DeadCheck(playerCharacters[randomValue].currentHP, playerCharacters[randomValue]);
        if(AreAllCharactersDead(playerCharacters) == true)//모두 죽었는지 감지
        {
            Debug.Log("pet all dead");
            //코루틴으로 패배 표현 애니메이션
            //화면멈추기
            //패배패널 띄우기
            gameOverPanel.SetActive(true);
        }

        if (currentEnemyIndex >= 4)
        {
            currentEnemyIndex = 0;
        }
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

    private static GameManager instance = null;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
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

    public void DeadCheck(int hp, Character targetObject)//죽음감지
    {
        if (hp <= 0)//타겟 캐릭터 hp가 0이면
        {
            Renderer renderer = targetObject.GetComponent<Renderer>();
            renderer.material.color = Color.black;
        }
    }

    public void stopTouch()//에너미 턴일시 화면터치 불가
    {
        
    }
    public void mpLack()
    {

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
        //체력, mp 다시 배치
        foreach (Character player in playerCharacters)
        {
            if (player is pet)
            {
                ((pet)player).SetPetChar();
            }

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
        PlayerTurn = true;
        EnemyTurn = false;
        currentPlayerIndex = 0;
        currentEnemyIndex = 0;
        currentTurn =0;
        //패널 닫기
        gameClearPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        //턴 표시 업댓
        ChangeTurnText();
    }

}

