using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip background;//배경음
    public AudioClip starGame;//게임 시작
    public AudioClip roundStart;//라운드 바뀔때
    public AudioClip skillButtonClick;//스킬버튼 눌릴때
    public AudioClip passButton;//패스 버튼
    public AudioClip clickTarget;//플레이어가 타겟 누를때
    //public AudioSource backAudio;
    //스킬
    public List<AudioClip> petSkillSound;


    //야옹이 멍멍이 꽥괙 소리 넣기
    //피격받는 소리 넣기
    //죽을때 소리 넣기
    public void SFXPlay(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.Log("clip not exist");
        }
        else
        {
            GameObject soundObject = new GameObject("Sound");
            AudioSource audiosource = soundObject.AddComponent<AudioSource>();
            audiosource.clip = clip;
            audiosource.Play();

            Destroy(soundObject, clip.length);
        }

    }

    public void BgSoundPlay(AudioClip clip)
    {
        GameObject soundObject = new GameObject("BackGroundSound");
        AudioSource backAudio = soundObject.AddComponent<AudioSource>();
        backAudio.clip = clip;
        backAudio.loop = true;
        backAudio.volume = 1.0f;
        backAudio.Play();

    }
}
