using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    //[SerializeField]
    //float x, y, z, ArSpeed;

    private Rigidbody mRB;
    [SerializeField]
    private float mSpeed, mTorque;

	private EffectPool mEffectPool;

	private SoundController mSoundController;
	private GameController mGamecontroller;

	private void Awake()
	{
		mRB = GetComponent<Rigidbody>();
		mRB.velocity = Vector3.back * mSpeed;
		mSoundController = GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundController>();
		mGamecontroller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}
	private void OnEnable()
	{
		mRB.angularVelocity = Random.onUnitSphere * mTorque; // angularVelocity : 회전속도, Random.onUnitSphere : 랜덤한 구형태의xyz좌표
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Bolt"))
		{
			if (mEffectPool == null) // 죽었을 때 mEffectPool 확인해서 이펙트 넣어주기
			{
				mEffectPool = GameObject.FindGameObjectWithTag("EffectPool").GetComponent<EffectPool>();
			}
			
			Timer effect = mEffectPool.GetFromPool((int)mEffecttype.Asteroid);
			effect.transform.position = transform.position;

			mSoundController.PlayEffectSound((int)eSoundtype.ExpAst);

			mGamecontroller.AddScore(1);

			gameObject.SetActive(false);
			other.gameObject.SetActive(false);
		}
	}

	// Update is called once per frame
	//   void Update()
	//   {
	//	//transform.Translate(0, 0, -ArSpeed, Space.World);
	//}
}
