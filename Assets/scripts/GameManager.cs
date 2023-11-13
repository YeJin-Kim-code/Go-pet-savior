using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GameManager : MonoBehaviour
{

    public TextMeshProUGUI TurnText;

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
        PlayerTurn = false;

        //애니메이션 및 기능 추가

        //일단 지금은 큰 데미지 기준
        enemyCharacters[targetEnemy].currentHP -= DB_petsSkill.GetEntity(skillIndex).highDamage;//에너미 피격
        playerCharacters[currentPlayerIndex].currentMP -= DB_petsSkill.GetEntity(skillIndex).useMp;//mp 차감

        yield return new WaitForSeconds(animationTime);

        //PlayerTurn = false;//턴 넘겨주기
        dataPanelConnect.skillIndex = -1; //스킬인덱스 초기화
        character.targetEnemyIndex = -1; //에너미 타겟 인덱스 초기화
        currentPlayerIndex += 1; //현재 플레이어 턴 설정
        character.targetCheckCIrcle.transform.position = character.checkCircleDefaultPosition;//어떤 캐릭터선택했는지 체크
        ChangeTurnText();//지금이 몇번째 turn 인지 표시
        EnemyTurn = true;
        DeadCheck(enemyCharacters[targetEnemy].currentHP, enemyCharacters[targetEnemy]);//죽음감지
        if (currentPlayerIndex >=4)
        {
            currentPlayerIndex = 0;
        }
    }


    IEnumerator EnemyAttack()//데미지 받기
    {

        EnemyTurn = false;
        int randomValue = Random.Range(0, 4);
        character.targetCheckCIrcle.transform.position = playerCharacters[randomValue].transform.position;//선택 표시하기
        playerCharacters[randomValue].currentHP -= DB_enemySkill.GetEntity(currentEnemyIndex).highDamage;

        yield return new WaitForSeconds(animationTime);
        currentEnemyIndex += 1;
        character.targetCheckCIrcle.transform.position = character.checkCircleDefaultPosition;
        ChangeTurnText();
        PlayerTurn = true;
        DeadCheck(playerCharacters[randomValue].currentHP, playerCharacters[randomValue]);
        if (currentEnemyIndex >= 4)
        {
            currentEnemyIndex = 0;
        }
    }

    bool AreAllCharactersDead()
    {
        // 모든 캐릭터의 HP가 0 이하인지 확인
        foreach (Character playerCharacter in playerCharacters)
        {
            if (playerCharacter.currentHP > 0)
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
        TurnText.text = currentTurn.ToString();
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

    public void DeadCheck(int hp, Character targetObject)
    {
        if (hp <= 0)
        {
            Renderer renderer = targetObject.GetComponent<Renderer>();
            renderer.material.color = Color.black;
        }
    }
}
