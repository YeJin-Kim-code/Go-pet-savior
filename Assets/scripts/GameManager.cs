using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Character> playerCharacters;
    public List<Character> enemyCharacters;

    private int currentPlayerIndex = 0;
    public int currentEnemyIndex = 0;



    void Start()
    {
        // ���� ���� �ʱ�ȭ �� ĳ���� ����

        // playerCharacters�� enemyCharacters ����Ʈ�� ĳ���� �߰�
        // �ϴ��� �������� �������� �ڵ�� �ۼ��غ���
        Debug.Log(playerCharacters[0].maxHP);
    }
}
