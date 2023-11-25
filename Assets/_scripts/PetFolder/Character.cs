using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Character : MonoBehaviour
{

    [SerializeField]
    public Animator animator;
    public int charIndex = 0; // 기본값
    public int targetEnemyIndex =  -1;
    public int targetPetIndex = -1;
    public int maxHP;
    public int currentHP;
    public int maxMP;
    public int currentMP;
    public bool deadCheck = false;
    public GameObject targetCheckCIrcle;
    public Vector3 checkCircleDefaultPosition = new Vector3(10f, 10f, 10f);
    public TextMeshProUGUI hpText;
    private void Start()
    {
        //m_dataPanel = GameObject.FindObjectOfType<DataPanelConnect>();
        //SetPetChar();
        //SetEnemyChar();
        //hpText = Instantiate(GameManager.Instance.dataPanelConnect.showNowHpText, transform);
        //hpText.gameObject.SetActive(false); // 초기에는 비활성화
    }

    private void Update()
    {
        ClickCheckEnemy();
        ClickCheckPlayer();
    }
    public void OnMouseOver()
    {
        //StartCoroutine(ShowNowHp());

        //hpText.gameObject.SetActive(true);
        //Vector3 fixPosition = new Vector3(1f, 1f, 0f);
        //hpText.transform.position = Camera.main.WorldToScreenPoint(this.transform.position + fixPosition);
        GameManager.Instance.dataPanelConnect.ShowNowHpText(this.transform.position);
        // 텍스트 업데이트
        //UpdateHPText();
        GameManager.Instance.dataPanelConnect.UpdateNowHpText(this.currentHP, this.maxHP);
    }

    public void OnMouseExit()
    {
        GameManager.Instance.dataPanelConnect.HideNowHpText();
    }
    void UpdateHPText()
    {
        // HP 텍스트 업데이트
        hpText.text = "HP : " + this.currentHP.ToString() + "/" + this.maxHP.ToString();
    }
    public void ClickCheckEnemy()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//화면의 좌표계를 월드 좌표계로 전환해주는 함수(ex. 100 x 100 해상도의 경우 가운데 좌표가 스크린좌표로 나타내면 50,50 이지만 월드좌표는 0,0 이다.)			
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if (hit.collider != null)
            {
                //Destroy(newPrefabInstance);//표시 오브젝트 삭제

                string objectTag = hit.collider.gameObject.tag;

                switch (objectTag)
                {
                    case "EnemyBig":
                        targetEnemyIndex = 0;
                        Debug.Log("this enemy is 0");
                        break;
                    case "EnemyNormal":
                        targetEnemyIndex = 1;
                        Debug.Log("this enemy is 1");
                        break;
                    case "EnemySmall":
                        targetEnemyIndex = 2;
                        Debug.Log("this enemy is 2");
                        break;
                    case "EnemyHeal":
                        targetEnemyIndex = 3;
                        Debug.Log("this enemy is 3");
                        break;


                }
                // 현재 스크립트 오브젝트의 위치와 회전을 가져옴
                Vector3 currentPosition = hit.collider.gameObject.transform.position;

                // 프리팹을 현재 스크립트 오브젝트의 위치에 생성
                //newPrefabInstance = Instantiate(targetCheckCIrcle, currentPosition, currentRotation);
                targetCheckCIrcle.transform.position = currentPosition;

            }
        }
    }
    public void ClickCheckPlayer()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//화면의 좌표계를 월드 좌표계로 전환해주는 함수(ex. 100 x 100 해상도의 경우 가운데 좌표가 스크린좌표로 나타내면 50,50 이지만 월드좌표는 0,0 이다.)			
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if (hit.collider != null)
            {
                //Destroy(newPrefabInstance);//표시 오브젝트 삭제

                string objectTag = hit.collider.gameObject.tag;

                switch (objectTag)
                {
                    case "cat":
                        targetPetIndex = 0;
                        Debug.Log("this pet is 0");
                        break;
                    case "duck":
                        targetPetIndex = 1;
                        Debug.Log("this pet is 1");
                        break;
                    case "turtle":
                        targetPetIndex = 2;
                        Debug.Log("this pet is 2");
                        break;
                    case "dog":
                        targetPetIndex = 3;
                        Debug.Log("this pet is 3");
                        break;


                }
                // 현재 스크립트 오브젝트의 위치와 회전을 가져옴
                Vector3 currentPosition = hit.collider.gameObject.transform.position;

                // 프리팹을 현재 스크립트 오브젝트의 위치에 생성
                //newPrefabInstance = Instantiate(targetCheckCIrcle, currentPosition, currentRotation);
                targetCheckCIrcle.transform.position = currentPosition;

            }
        }
    }
    public void DeadCheck()
    {

    }
    IEnumerator ShowNowHp()
    {

        TextMeshProUGUI showHpText = Instantiate(GameManager.Instance.dataPanelConnect.showNowHpText, GameManager.Instance.dataPanelConnect.canvasTransform);
        showHpText.text = "HP : " + this.currentHP.ToString() + "/" + this.maxHP.ToString();
        Vector3 fixPosition = new Vector3(1f, 1f, 0f);
        showHpText.transform.position = Camera.main.WorldToScreenPoint(this.transform.position + fixPosition);

        yield return new WaitForSeconds(5f);

        Destroy(showHpText);
    }
    //마우스 놓으면 destroy
}
