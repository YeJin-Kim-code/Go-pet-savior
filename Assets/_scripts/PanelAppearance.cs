using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAppearance : MonoBehaviour
{
    //public GameObject panelClear;
    //public GameObject panelFail;
    public float duration = 1.0f;
    public Vector3 targetScale = new Vector3(1, 1, 1);

    private void Start()
    {
        // ���� �� �г��� �ʱ� ũ��� ����
        //panelClear.transform.localScale = Vector3.zero;
        //panelFail.transform.localScale = Vector3.zero;

        // ���� �ִϸ��̼� ����
        //StartCoroutine(AppearAnimation(panelClear));
    }

    public System.Collections.IEnumerator AppearAnimation(GameObject panel)
    {
        float timer = 0.0f;
        Vector3 initialScale = panel.transform.localScale;

        while (timer < duration)
        {
            // �ð��� ���� ������ ��� (��: EaseInOut)
            float t = timer / duration;
            float easedT = Mathf.SmoothStep(0.0f, 1.0f, t);

            // ũ�� ����
            panel.transform.localScale = Vector3.Lerp(initialScale, targetScale, easedT);

            // �ð� ������Ʈ
            timer += Time.deltaTime;

            // �� ������ ���
            yield return null;
        }

        // ���� ũ�� ���� (������ ����)
        panel.transform.localScale = targetScale;
    }
}
