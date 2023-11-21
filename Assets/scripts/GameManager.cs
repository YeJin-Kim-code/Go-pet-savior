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

    public List<Character> playerCharacters;
    public List<Character> enemyCharacters;

    public int currentPlayerIndex = 0;
    public int currentEnemyIndex = 0;
    public int currentTurn = 1;
    public float animationTime = 3f;

    public bool PlayerTurn = true;
    public bool EnemyTurn = false;

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
            while (playerCharacters[currentPlayerIndex].deadCheck)
            {
                currentPlayerIndex++;
                if(currentPlayerIndex>4)
                {
                    currentPlayerIndex = 0;
                }
                Debug.Log(currentPlayerIndex);
            }
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

        //mp �������� üũ
        if(playerCharacters[currentPlayerIndex].currentMP<= DB_petsSkill.GetEntity(skillIndex).useMp)
        {
            //���â ���� �Ѿ��
            
            Debug.Log("mp�� ����");
            //PlayerTurn = true;
        }
        else
        {
            //update�� ����� ����
            PlayerTurn = false;
            //������, mp ����

            //���� �ִϸ��̼� �� ����
            LoadSkillAniAndEffect(playerCharacters[currentPlayerIndex], dataPanelConnect.skillIndex);
            //�����̴� ���� ����
            enemyCharacters[targetEnemy].currentHP -= DamageCalculate(DB_petsSkill.GetEntity(skillIndex).lowDamage, (DB_petsSkill.GetEntity(skillIndex).highDamage+1+slider.slideValue));//���ʹ� �ǰ�
            playerCharacters[currentPlayerIndex].currentHP -= slider.slideValue;
            playerCharacters[currentPlayerIndex].currentMP -= DB_petsSkill.GetEntity(skillIndex).useMp;//mp ����

            yield return new WaitForSeconds(animationTime);

            TurnPass();
            //�������
            DeadCheck(enemyCharacters[targetEnemy].currentHP, enemyCharacters[targetEnemy]);//��������
            if (AreAllCharactersDead(enemyCharacters) == true)//��� �׾����� ����
            {
                Debug.Log("enemy all dead");
                //�ڷ�ƾ���� �¸� ǥ�� �ִϸ��̼�
                //ȭ����߱�
                //�¸��г� ���� - ��ȭ ������ �ٽ��ϱ�
                gameClearPanel.SetActive(true);
            }

        }

    }


    IEnumerator EnemyAttack()//������ �ޱ�
    {

        EnemyTurn = false;
        int randomValue = Random.Range(0, 4);
        while (playerCharacters[randomValue].deadCheck)//�÷��̾� ������ �ٸ� �� ����
        {
            randomValue = Random.Range(0, 4);
        }
        
        character.targetCheckCIrcle.transform.position = playerCharacters[randomValue].transform.position;//���� ǥ���ϱ�
        playerCharacters[randomValue].currentHP -= DamageCalculate(DB_enemySkill.GetEntity(currentEnemyIndex).lowDamage, DB_enemySkill.GetEntity(currentEnemyIndex).highDamage+1);//������ ������ �ֱ�

        yield return new WaitForSeconds(animationTime);

        character.targetCheckCIrcle.transform.position = character.checkCircleDefaultPosition;

        currentEnemyIndex += 1;
        ChangeTurnText();
        PlayerTurn = true;

        DeadCheck(playerCharacters[randomValue].currentHP, playerCharacters[randomValue]);
        //������ �������� �ʰ�
        if(AreAllCharactersDead(playerCharacters) == true)//��� �׾����� ����
        {
            Debug.Log("pet all dead");
            //�ڷ�ƾ���� �й� ǥ�� �ִϸ��̼�
            //ȭ����߱�
            //�й��г� ����
            gameOverPanel.SetActive(true);
            PlayerTurn = false;
        }

        if (currentEnemyIndex >= 4)
        {
            currentEnemyIndex = 0;
        }
    }
    public int DamageCalculate(int smallDamage, int bigDamage)
    {
        int randomDamageValue = Random.Range(smallDamage, bigDamage);
        Debug.Log(randomDamageValue);
        return randomDamageValue;
    }
    bool AreAllCharactersDead(List<Character> characterList)
    {
        // ��� ĳ������ HP�� 0 �������� Ȯ��
        foreach (Character Character in characterList)
        {
            if (Character.currentHP > 0)
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
    public void LoadSkillAniAndEffect(Character petchar, int skillIndex)// Character target)//�ִϸ��̼� �� ��ų ����Ʈ ����
    {
        if(skillIndex==2||skillIndex==6)// ���� �ִϸ��̼� ���� ��
        { 
            petchar.animator.SetTrigger("Attack"+skillIndex);//(pet)�ľ��ҵ� �ϴ�
            Debug.Log("animation");
        }

        //Ÿ�� �����ǿ� ����Ʈ
    }
    public void DeadCheck(int hp, Character targetObject)//��������
    {
        if (hp <= 0)//Ÿ�� ĳ���� hp�� 0�̸�
        {
            Renderer renderer = targetObject.GetComponent<Renderer>();
            renderer.material.color = Color.black;
            targetObject.deadCheck = true;
        }
    }

    public void StopTouch()//���ʹ� ���Ͻ� ȭ����ġ �Ұ�
    {
        
    }
    public void TurnPass()//�� pass ��ư
    {
        PlayerTurn = false;
        //�ʱ�ȭ
        dataPanelConnect.BeforeChooseSkill();//��ų�г� �ʱ�ȭ
        dataPanelConnect.skillIndex = -1; //��ų�ε��� �ʱ�ȭ
        character.targetEnemyIndex = -1; //���ʹ� Ÿ�� �ε��� �ʱ�ȭ
                                         //����üũ
        character.targetCheckCIrcle.transform.position = character.checkCircleDefaultPosition;//üũ ���� �����ڸ���
                                                                                              //��
        currentPlayerIndex += 1; //���� �÷��̾� �� ����
        ChangeTurnText();//������ ���° turn ���� ǥ��
        EnemyTurn = true;
        //�ε��� ����
        if (currentPlayerIndex >= 4)
        {
            currentPlayerIndex = 0;
        }
    }
    public void reBorn(List<Character> characterList)//��Ȱ again �� �ٽ� ü���� ���ٸ�
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
        //ü��, mp �ٽ� ��ġ, deadcheck���� �ʱ�ȭ
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
        //ü�� �������� ���� ����ְ�
        reBorn(enemyCharacters);
        reBorn(playerCharacters);
        //�� �ʱ�ȭ
        PlayerTurn = true;
        EnemyTurn = false;
        currentPlayerIndex = 0;
        currentEnemyIndex = 0;
        currentTurn =0;
        //�г� �ݱ�
        gameClearPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        //�� ǥ�� ����
        ChangeTurnText();
    }

}

