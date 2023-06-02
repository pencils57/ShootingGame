using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float mSpeed; 
    private Rigidbody mRB;


	[SerializeField]
	private BoltPool mBoltPool;
	[SerializeField]
	private Transform mBoltPos;


	private EffectPool mEffectPool;

	private SoundController mSoundController;

	private GameController mGameController;

	private void Awake()
	{
		mRB= GetComponent<Rigidbody>();
		mSoundController = GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundController>();
		mGameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}
	private void OnEnable()
	{
		transform.position = new Vector3(Random.Range(-5f, 5), 0, 16);
		mRB.velocity= Vector3.back *mSpeed;
		StartCoroutine(MovePattern());
		StartCoroutine(AutoFire());
		//InvokeReapting 은 coroutine과 비슷한 형식이지만 해당 오브젝트가 비활성화되어도 계속 실행되는 차이가 있다.
	}

	public void SetBoltPool(BoltPool pool)
	{
		mBoltPool = pool;
	}

	private IEnumerator AutoFire()
	{
		WaitForSeconds fireRate = new WaitForSeconds(2.5f);
		while (true)
		{
			yield return fireRate;
			Bolt newBolt = mBoltPool.GetFromPool();
			
			newBolt.transform.position = mBoltPos.position;
			newBolt.transform.rotation = mBoltPos.rotation;
			mSoundController.PlayEffectSound((int)eSoundtype.FireEen);
		}
	}

	private IEnumerator MovePattern()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
			if (transform.position.x < 0)
			{
				mRB.velocity += Vector3.right * Random.Range(1f, 2f);
			}
			else
			{
				mRB.velocity += Vector3.left * Random.Range(1f, 2f);
			}
			yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
			mRB.velocity = Vector3.back * mSpeed;

			//Vector3 oriVel = mRB.velocity;
			//oriVel.x = 0;
			//mRB.velocity = oriVel;

		}
		
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Bolt"))
		{
			if(mEffectPool == null)
			{
				mEffectPool = GameObject.FindGameObjectWithTag("EffectPool").GetComponent<EffectPool>();
			}
			Timer effect = mEffectPool.GetFromPool((int)mEffecttype.Enmey);
			effect.transform.position = transform.position;

			mSoundController.PlayEffectSound((int)eSoundtype.ExpEen);

			mGameController.AddScore(10);

			gameObject.SetActive(false);
			other.gameObject.SetActive(false);
		}
	}
}
