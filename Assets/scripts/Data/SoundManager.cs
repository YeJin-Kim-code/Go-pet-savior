using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip background;//�����
    public AudioClip starGame;//���� ����
    public AudioClip roundStart;//���� �ٲ�
    public AudioClip skillButtonClick;//��ų��ư ������
    public AudioClip passButton;//�н� ��ư
    public AudioClip clickTarget;//�÷��̾ Ÿ�� ������
    //public AudioSource backAudio;
    //��ų
    public List<AudioClip> petSkillSound;


    //�߿��� �۸��� �ЂE �Ҹ� �ֱ�
    //�ǰݹ޴� �Ҹ� �ֱ�
    //������ �Ҹ� �ֱ�
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
