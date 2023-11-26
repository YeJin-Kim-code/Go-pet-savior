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
        // 시작 시 패널을 초기 크기로 설정
        //panelClear.transform.localScale = Vector3.zero;
        //panelFail.transform.localScale = Vector3.zero;

        // 등장 애니메이션 실행
        //StartCoroutine(AppearAnimation(panelClear));
    }

    public System.Collections.IEnumerator AppearAnimation(GameObject panel)
    {
        float timer = 0.0f;
        Vector3 initialScale = panel.transform.localScale;

        while (timer < duration)
        {
            // 시간에 따른 보간값 계산 (예: EaseInOut)
            float t = timer / duration;
            float easedT = Mathf.SmoothStep(0.0f, 1.0f, t);

            // 크기 보간
            panel.transform.localScale = Vector3.Lerp(initialScale, targetScale, easedT);

            // 시간 업데이트
            timer += Time.deltaTime;

            // 한 프레임 대기
            yield return null;
        }

        // 최종 크기 설정 (안전을 위해)
        panel.transform.localScale = targetScale;
    }
}
