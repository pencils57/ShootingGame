using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetting : MonoBehaviour
{
	
    private PlayerController mPlayer;
	private Enemy mEnemy;
	private Bolt mBolt;

	private void Awake()
	{
		GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
		if(playerObj != null)
		{
			mPlayer = playerObj.GetComponent<PlayerController>();
		}
		else
		{
			mPlayer = null;
		}
		//mPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();  플레이어가 죽었을때 널익스트레션 터지는걸 방지하기 위해 if문으로 수정
	}

	private void OnEnable()
	{
		StartCoroutine(FollowTarget());
		if (mPlayer != null && mPlayer.gameObject.activeInHierarchy) //&& mEnemy.gameObject.activeInHierarchy)
		{
			mBolt.SetTarget(mPlayer.transform.position);
		}
	}

	private  IEnumerator FollowTarget()
	{
		WaitForSeconds gap = new WaitForSeconds(2.5f);
		while (true)
		{
			
			
			yield return gap;
			Debug.Log("targetting");
		}
	}
}
