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
    public int charIndex = 0; // �⺻��
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
        //hpText.gameObject.SetActive(false); // �ʱ⿡�� ��Ȱ��ȭ
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
        // �ؽ�Ʈ ������Ʈ
        //UpdateHPText();
        GameManager.Instance.dataPanelConnect.UpdateNowHpText(this.currentHP, this.maxHP);
    }

    public void OnMouseExit()
    {
        GameManager.Instance.dataPanelConnect.HideNowHpText();
    }
    void UpdateHPText()
    {
        // HP �ؽ�Ʈ ������Ʈ
        hpText.text = "HP : " + this.currentHP.ToString() + "/" + this.maxHP.ToString();
    }
    public void ClickCheckEnemy()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//ȭ���� ��ǥ�踦 ���� ��ǥ��� ��ȯ���ִ� �Լ�(ex. 100 x 100 �ػ��� ��� ��� ��ǥ�� ��ũ����ǥ�� ��Ÿ���� 50,50 ������ ������ǥ�� 0,0 �̴�.)			
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if (hit.collider != null)
            {
                //Destroy(newPrefabInstance);//ǥ�� ������Ʈ ����

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
                // ���� ��ũ��Ʈ ������Ʈ�� ��ġ�� ȸ���� ������
                Vector3 currentPosition = hit.collider.gameObject.transform.position;

                // �������� ���� ��ũ��Ʈ ������Ʈ�� ��ġ�� ����
                //newPrefabInstance = Instantiate(targetCheckCIrcle, currentPosition, currentRotation);
                targetCheckCIrcle.transform.position = currentPosition;

            }
        }
    }
    public void ClickCheckPlayer()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//ȭ���� ��ǥ�踦 ���� ��ǥ��� ��ȯ���ִ� �Լ�(ex. 100 x 100 �ػ��� ��� ��� ��ǥ�� ��ũ����ǥ�� ��Ÿ���� 50,50 ������ ������ǥ�� 0,0 �̴�.)			
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if (hit.collider != null)
            {
                //Destroy(newPrefabInstance);//ǥ�� ������Ʈ ����

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
                // ���� ��ũ��Ʈ ������Ʈ�� ��ġ�� ȸ���� ������
                Vector3 currentPosition = hit.collider.gameObject.transform.position;

                // �������� ���� ��ũ��Ʈ ������Ʈ�� ��ġ�� ����
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
    //���콺 ������ destroy
}
