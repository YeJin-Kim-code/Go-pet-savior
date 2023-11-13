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

    IEnumerator PlayerAttack(int skillIndex, int targetEnemy)//�÷��̾� turn
    {
        //update�� ����� ����
        PlayerTurn = false;

        //�ִϸ��̼� �� ��� �߰�

        //�ϴ� ������ ū ������ ����
        enemyCharacters[targetEnemy].currentHP -= DB_petsSkill.GetEntity(skillIndex).highDamage;//���ʹ� �ǰ�
        playerCharacters[currentPlayerIndex].currentMP -= DB_petsSkill.GetEntity(skillIndex).useMp;//mp ����

        yield return new WaitForSeconds(animationTime);

        //PlayerTurn = false;//�� �Ѱ��ֱ�
        dataPanelConnect.skillIndex = -1; //��ų�ε��� �ʱ�ȭ
        character.targetEnemyIndex = -1; //���ʹ� Ÿ�� �ε��� �ʱ�ȭ
        currentPlayerIndex += 1; //���� �÷��̾� �� ����
        character.targetCheckCIrcle.transform.position = character.checkCircleDefaultPosition;//� ĳ���ͼ����ߴ��� üũ
        ChangeTurnText();//������ ���° turn ���� ǥ��
        EnemyTurn = true;
        DeadCheck(enemyCharacters[targetEnemy].currentHP, enemyCharacters[targetEnemy]);//��������
        if (currentPlayerIndex >=4)
        {
            currentPlayerIndex = 0;
        }
    }


    IEnumerator EnemyAttack()//������ �ޱ�
    {

        EnemyTurn = false;
        int randomValue = Random.Range(0, 4);
        character.targetCheckCIrcle.transform.position = playerCharacters[randomValue].transform.position;//���� ǥ���ϱ�
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
        // ��� ĳ������ HP�� 0 �������� Ȯ��
        foreach (Character playerCharacter in playerCharacters)
        {
            if (playerCharacter.currentHP > 0)
            {
                // ���� ������ ĳ���Ͱ� �ִٸ� false ��ȯ
                return false;
            }
        }

        // ��� ĳ���Ͱ� �׾��ٸ� true ��ȯ
        return true;
    }

    public void ChangeTurnText()//�� text ����
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
