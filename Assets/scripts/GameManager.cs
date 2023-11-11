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
        
        // ���� ���� �ʱ�ȭ �� ĳ���� ����
        //�ڷ�ƾ���� 
        // playerCharacters�� enemyCharacters ����Ʈ�� ĳ���� �߰�
        // �ϴ��� �������� �������� �ڵ�� �ۼ��غ���
        PlayerTurn = true;
        Debug.Log(playerCharacters[0].maxHP);
    }

    private void Update()
    {
        PlayGame();
    }
    public void PlayGame()
    {
        if(PlayerTurn == true)//�Ʊ� ���Ͻ�
        {
            dataPanelConnect.DisplayCharInfo(currentPlayerIndex);//�г� ��ġ
            if(dataPanelConnect.skillIndex != -1)//��ų ��ư�� ���ȴٸ�, �ϳ����� ��ų �� -1�� �ʱ�ȭ
            {
                //Debug.Log(dataPanelConnect.skillIndex);
                if(character.targetEnemyIndex != -1)
                {
                    StartCoroutine(PlayerAttack(dataPanelConnect.skillIndex, character.targetEnemyIndex));
                }
            }

            //playerCharacters[currentPlayerIndex]
        }
        else if (EnemyTurn == true)//���� ���Ͻ�
        {
            StartCoroutine(EnemyAttack());

        }
    }

    IEnumerator PlayerAttack(int skillIndex, int targetEnemy)
    {
        //update�� ����� ����
        PlayerTurn = false;


        //�ִϸ��̼� �� ��� �߰�
        Debug.Log("skillIndex : " + skillIndex);
        Debug.Log("targetEnemy : " + targetEnemy);

        //�ϴ� ������ ū ������ ����
        enemyCharacters[targetEnemy].currentHP -= DB_petsSkill.GetEntity(skillIndex).highDamage;//���ʹ� �ǰ�
        Debug.Log("targetenemy currnetHp : " + enemyCharacters[character.targetEnemyIndex].currentHP);


        playerCharacters[currentPlayerIndex].currentMP -= DB_petsSkill.GetEntity(skillIndex).useMp;//mp ����
        Debug.Log("playerChar currentmp : " + playerCharacters[currentPlayerIndex].currentMP);

        yield return new WaitForSeconds(animationTime);

        //PlayerTurn = false;//�� �Ѱ��ֱ�
        dataPanelConnect.skillIndex = -1; //��ų�ε��� �ʱ�ȭ
        character.targetEnemyIndex = -1; //���ʹ� Ÿ�� �ε��� �ʱ�ȭ
        currentPlayerIndex += 1; //���� �÷��̾� �� ����
        currentTurn += 1;
        EnemyTurn = true;
        Debug.Log("currnetPlayerIndex : " + currentPlayerIndex);
        Debug.Log("currnetTurn : " + currentTurn);
        if(currentPlayerIndex >=4)
        {
            currentPlayerIndex = 0;
        }
    }


    IEnumerator EnemyAttack()//������ �ޱ�
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
