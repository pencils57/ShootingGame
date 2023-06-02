using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 재시작, 로드할 때 사용

public class GameController : MonoBehaviour
{
    [SerializeField]
    private float mScore = 0;
    [SerializeField]
    private UIController mUIController;
    [SerializeField]
    private PlayerController mPlayer;
    private bool mbRestart;
    [SerializeField]
    private ItemPool mItemPool;
    [Header("Hazard")] // 구획 나누기용, 아래 무조건 시리얼라이즈필드가 있어야지 적용된다. 줄바꿈만 하고싶으면 [Space]
    [SerializeField]
    private AsteroidPool mAstPool;
    [SerializeField]
    private EnemyPool mEnemyPool;
    [SerializeField]
    private float mPeriod;
    [SerializeField]
    private int mASTSpawnCount, mEnemySpawnCount;
    private Coroutine mHazardRoutine;
    private float mCountdown;

	private int mRoundCount;
	// Start is called before the first frame update
	void Start()
    {
        mbRestart = false;
		mCountdown = mPeriod;
        mUIController.ShowScore(mScore);
		mHazardRoutine = StartCoroutine(SpawnHazard());
        //StartCoroutine("SpawnHazard", 10);
    }

    //한번 실행 코루틴, while문 쓰면 무한 루틴 > start에서 실행해줘야한다.
    private IEnumerator SampleRoutine()
    {
        yield return new WaitForSeconds(5);

        Debug.Log("Called!");
    }

    public void AddScore(float amount)
    {
        mScore += amount;
		mUIController.ShowScore(mScore);
		//Debug.Log("Score : " + mScore);
    }

    public void GameOver()
    {
        mbRestart = true;
		mUIController.ShowState("Game Over!");
        mUIController.ShowRestartText(true);
        StopCoroutine(mHazardRoutine);
    }
    
    public void Restart()
    {
        SceneManager.LoadScene(0);
  //      mPlayer.gameObject.SetActive(true);
  //      mPlayer.transform.position = Vector3.zero;

  //      mScore = 0;
  //      mUIController.ShowScore(mScore);

  //      mHazardRoutine = StartCoroutine(SpawnHazard());

  //      mUIController.ShowRestartText(false);
  //      mUIController.ShowState("");
		//mbRestart = false;
	}

	public void Update()
	{
        if (mbRestart == true && Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
	}

	private IEnumerator SpawnHazard()
    {
        WaitForSeconds Period = new WaitForSeconds(mPeriod);
        int currentAST, currentEnemy;
        float ASTRate;
        while (true)
        {
            //wait
            yield return Period;
            //execute
            currentAST = mASTSpawnCount + mRoundCount;
            currentEnemy = mEnemySpawnCount + mRoundCount/2;
            ASTRate = (float)currentAST / (currentAST + currentEnemy);
           

            while(currentAST > 0 && currentEnemy > 0)
            {
				float rate = Random.Range(0, 1f);
				if (rate < ASTRate) //운석생성
				{
					Asteroid ast = mAstPool.GetFromPool(Random.Range(0, 2));
					ast.transform.position = new Vector3(Random.Range(-5f, 5f), 0, 16);
                    currentAST--;
				}
				else // 적 생성
				{
					Enemy enemy = mEnemyPool.GetFromPool();
					enemy.transform.position = new Vector3(Random.Range(-5f, 5f), 0, 16);
                    currentEnemy--;
				}
				yield return new WaitForSeconds(.5f);
			}
			for (int i = 0; i < currentAST; i++)
			{
                Asteroid ast = mAstPool.GetFromPool(Random.Range(0, 3));
				ast.transform.position = new Vector3(Random.Range(-5f, 5f), 0, 16);
                yield return new WaitForSeconds(.5f);
			}
			for (int i = 0; i < currentEnemy; i++)
			{
				Enemy enemy = mEnemyPool.GetFromPool();
				enemy.transform.position = new Vector3(Random.Range(-5f, 5f), 0, 16);
				yield return new WaitForSeconds(.5f);
			}
			mRoundCount++;
			Debug.Log(mRoundCount.ToString());
            Item item = mItemPool.GetFromPool(Random.Range(0,2));
            item.transform.position = new Vector3(Random.Range(-5f, 5f), 0, 16);
		}
        
	}


}
