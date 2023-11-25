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
        //����� ���߿� ����
        //soundManager.BgSoundPlay(soundManager.background);
        // ���� ���� �ʱ�ȭ �� ĳ���� ����
        //�ڷ�ƾ���� 
        // playerCharacters�� enemyCharacters ����Ʈ�� ĳ���� �߰�
        // �ϴ��� �������� �������� �ڵ�� �ۼ��غ���
        m_playerTurn = true;
        Debug.Log(playerCharacters[0].maxHP);
    }

    private void Update()
    {
        PlayGame();
    }
    public void PlayGame()
    {
        if(m_playerTurn == true)//�Ʊ� ���Ͻ�
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
                //���׶��� �װ� ��ų ������ ���� ���� ���콺 �ø��� �߱�
                //���� ��ų�ε����� ��ü����, ����, Ÿ�� ����̸� ��ų �ε����� ���� ���콺 �� �ø� �� �ִ� �ֵ� ����
                //Debug.Log(dataPanelConnect.skillIndex);
                if(dataPanelConnect.skillIndex == 2)
                {
                    if (character.targetPetIndex == 0)
                    {
                        StartCoroutine(PlayerAttack(dataPanelConnect.skillIndex, character.targetPetIndex));
                    }
                }
                else if(dataPanelConnect.skillIndex == 7 || dataPanelConnect.skillIndex == 6) //��, ����, ��� ��ų�̸�
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
        else if (m_enemyTurn == true)//���� ���Ͻ�
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
        }
        else
        {
            //update�� ����� ����
            m_playerTurn = false;
            //������, mp ����

            //���� �ִϸ��̼�
            LoadSkillAniAndEffect(playerCharacters[currentPlayerIndex], dataPanelConnect.skillIndex);


            //������ �����̴� ���� ����
            petSkillDamage(skillIndex, targetEnemy);
            playerCharacters[currentPlayerIndex].currentHP -= slider.slideValue;
            playerCharacters[currentPlayerIndex].currentMP -= DB_petsSkill.GetEntity(skillIndex).useMp;//mp ����

            //����
            soundManager.SFXPlay(soundManager.petSkillSound[skillIndex]);
            yield return new WaitForSeconds(m_animationTime);

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
                m_playerTurn = false;
                m_enemyTurn = false;
            }
        }
    }

    IEnumerator EnemyAttack()//������ �ޱ�
    {
        m_enemyTurn = false;
        int randomValue = Random.Range(0, 4);
        int EnemyRandomDamage = 0;
        while (playerCharacters[randomValue].deadCheck)//�÷��̾� ������ �ٸ� �� ����
        {
            randomValue = Random.Range(0, 4);
        }
        
        character.targetCheckCIrcle.transform.position = playerCharacters[randomValue].transform.position;//���� ǥ���ϱ�
        EnemyRandomDamage = DamageCalculate(DB_enemySkill.GetEntity(currentEnemyIndex).lowDamage, DB_enemySkill.GetEntity(currentEnemyIndex).highDamage + 1);//������ ������ �ֱ�
        playerCharacters[randomValue].currentHP -= EnemyRandomDamage;

        //����
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
        //������ �������� �ʰ�
        if(AreAllCharactersDead(playerCharacters) == true)//��� �׾����� ����
        {
            Debug.Log("pet all dead");
            //�ڷ�ƾ���� �й� ǥ�� �ִϸ��̼�
            //ȭ����߱�
            //�й��г� ����
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
        if (skillIndex == 0 || skillIndex == 1 || skillIndex == 3 || skillIndex == 9 || skillIndex == 10)//��ų�� ���� �����̸�
        {
            petDefaultDamage(skillIndex, targetEnemy);
        }
        else if (skillIndex == 4 || skillIndex == 5)// ���� ����
        {
            Debug.Log("skillMultiAttack");
            
            for(int i =0; i<4; i++)//�Ű����� list ���� list�� ���̷� ��ü
            {
                petDefaultDamage(skillIndex, i);
            }
        }
        else if(skillIndex == 6)
        {
            for (int i = 0; i < 4; i++)//�Ű����� list ���� list�� ���̷� ��ü
            {
                petDefaultHeal(skillIndex, i);
            }
        }
        else if(skillIndex == 7 || skillIndex == 2)//���� ��, ����
        {
            petDefaultHeal(skillIndex, targetEnemy);
        }

        else
        {
            Debug.Log("���� ����");
        }
        //petDefaultDamage(skillIndex, targetEnemy);
    }

    public void petDefaultDamage(int skillIndex, int targetEnemy)//���� ����Ʈ
    {
        Debug.Assert(skillIndex>=0 && targetEnemy>=0);//�׻� ���̵Ǿ�� �Ű����� ���� ��ȿ���� ����
        m_damageRandomResult = DamageCalculate(DB_petsSkill.GetEntity(skillIndex).lowDamage, (DB_petsSkill.GetEntity(skillIndex).highDamage + 1 + slider.slideValue));//������ ����
        enemyCharacters[targetEnemy].currentHP -= m_damageRandomResult;//���ʹ� �ǰ�
        dataPanelConnect.ShowDamageText(m_damageRandomResult, enemyCharacters[targetEnemy].gameObject.transform.position);//������ �ؽ�Ʈ ǥ��
        ShowEffect(skillIndex, enemyCharacters[targetEnemy], effectGameObjects);
        StartCoroutine(GetDamageTurnRed(enemyCharacters[targetEnemy]));
    }
    public void petDefaultHeal(int skillIndex, int targetEnemy)//ġ�� ����Ʈ
    {
        m_damageRandomResult = DamageCalculate(DB_petsSkill.GetEntity(skillIndex).lowDamage, (DB_petsSkill.GetEntity(skillIndex).highDamage + 1 + slider.slideValue));//������ ����
        playerCharacters[targetEnemy].currentHP += m_damageRandomResult;//�Ʊ� ��
        dataPanelConnect.ShowDamageText(m_damageRandomResult, playerCharacters[targetEnemy].gameObject.transform.position);//������ �ؽ�Ʈ ǥ��
        ShowEffect(skillIndex, playerCharacters[targetEnemy], effectGameObjects);
        //StartCoroutine(GetDamageTurnRed(playerCharacters[targetEnemy])); �Ķ����� �ٲ��� ������
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

    public void LoadSkillAniAndEffect(Character petchar, int skillIndex)// Character target)//�ִϸ��̼� �� ��ų ����Ʈ ����
    {
        if(skillIndex!=4||skillIndex!=8)// ���� �ִϸ��̼� ���� ��
        { 
            petchar.animator.SetTrigger("Attack"+skillIndex);
            Debug.Log("animation");
        }

        //Ÿ�� �����ǿ� ����Ʈ
    }

    public void LoadSkillAniEnemy(Character enemyChar)
    {
        enemyChar.animator.SetTrigger("Attack");
    }
    public void DamageAni(Character chara)
    {
        chara.animator.SetTrigger("Damage");
    }
    public void ShowEffect(int skillIndex, Character enemychar, List<GameObject> effect)//����Ʈ
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
        m_playerTurn = false;
        //�ʱ�ȭ
        dataPanelConnect.BeforeChooseSkill();//��ų�г� �ʱ�ȭ
        dataPanelConnect.skillIndex = -1; //��ų�ε��� �ʱ�ȭ
        character.targetEnemyIndex = -1; //���ʹ� Ÿ�� �ε��� �ʱ�ȭ
        character.targetPetIndex = -1;
                     
        character.targetCheckCIrcle.transform.position = character.checkCircleDefaultPosition;//üũ ���� �����ڸ���

        soundManager.SFXPlay(soundManager.passButton);
        currentPlayerIndex += 1; //���� �÷��̾� �� ����
        ChangeTurnText();//������ ���° turn ���� ǥ��
        m_enemyTurn = true;
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
        m_playerTurn = true;
        m_enemyTurn = false;
        currentPlayerIndex = 0;
        currentEnemyIndex = 0;
        currentTurn =0;
        //�г� �ݱ�
        gameClearPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        //�� ǥ�� ����
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

