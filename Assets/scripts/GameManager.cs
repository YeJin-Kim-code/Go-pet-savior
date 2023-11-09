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
        // 게임 시작 초기화 및 캐릭터 생성

        // playerCharacters와 enemyCharacters 리스트에 캐릭터 추가
        // 일단은 수동으로 놓았지만 코드로 작성해보기
        Debug.Log(playerCharacters[0].maxHP);
    }
}
