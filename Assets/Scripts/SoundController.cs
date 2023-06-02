using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eSoundtype
{
    ExpAst,
    ExpEen,
    ExpPla,
    FireEen,
    FirePla
}

public class SoundController : MonoBehaviour
{
    [SerializeField]
    private AudioSource mBGM, mEffect;
    [SerializeField]
    private AudioClip[] mEffectArr;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Play()
    {
		//샘플 쓰지말것 start > InvokeRepeating("Play", 3, 3);
		AudioSource.PlayClipAtPoint(mEffectArr[2], Vector3.zero);
    }

    public void PlayEffectSound(int id)
    {
        mEffect.PlayOneShot(mEffectArr[id]);
    }
}
