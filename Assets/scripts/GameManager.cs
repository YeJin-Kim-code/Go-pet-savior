using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

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

    IEnumerator PlayerAttack(int skillIndex, int targetEnemy)
    {
        //update문 재실행 차단
        PlayerTurn = false;


        //애니메이션 및 기능 추가
        Debug.Log("skillIndex : " + skillIndex);
        Debug.Log("targetEnemy : " + targetEnemy);

        //일단 지금은 큰 데미지 기준
        enemyCharacters[targetEnemy].currentHP -= DB_petsSkill.GetEntity(skillIndex).highDamage;//에너미 피격
        Debug.Log("targetenemy currnetHp : " + enemyCharacters[character.targetEnemyIndex].currentHP);


        playerCharacters[currentPlayerIndex].currentMP -= DB_petsSkill.GetEntity(skillIndex).useMp;//mp 차감
        Debug.Log("playerChar currentmp : " + playerCharacters[currentPlayerIndex].currentMP);

        yield return new WaitForSeconds(animationTime);

        //PlayerTurn = false;//턴 넘겨주기
        dataPanelConnect.skillIndex = -1; //스킬인덱스 초기화
        character.targetEnemyIndex = -1; //에너미 타겟 인덱스 초기화
        currentPlayerIndex += 1; //현재 플레이어 턴 설정
        currentTurn += 1;
        EnemyTurn = true;
        Debug.Log("currnetPlayerIndex : " + currentPlayerIndex);
        Debug.Log("currnetTurn : " + currentTurn);
        if(currentPlayerIndex >=4)
        {
            currentPlayerIndex = 0;
        }
    }


    IEnumerator EnemyAttack()//데미지 받기
    {

        EnemyTurn = false;
        int randomValue = Random.Range(0, 4);

        Debug.Log("attack charnum : " + randomValue);

        playerCharacters[randomValue].currentHP -= DB_enemySkill.GetEntity(currentEnemyIndex).highDamage;
        Debug.Log(playerCharacters[randomValue].name);
        Debug.Log(playerCharacters[randomValue].currentHP);


        yield return new WaitForSeconds(animationTime);
        currentEnemyIndex += 1;
        currentTurn += 1;
        Debug.Log("currnetTurn : " + currentTurn);
        PlayerTurn = true;
        if (currentEnemyIndex >= 4)
        {
            currentEnemyIndex = 0;
        }
    }

    public void GameEndCheck()
    {

    }
}
