using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UI를 사용하려면 추가해야 한다.

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Text mScoreText, mRestartText, mStateText;
    private Coroutine mAlphaAnimRoutine;
    // Start is called before the first frame update
    void Start()
    {
        mRestartText.gameObject.SetActive(false);
    }

    public void ShowRestartText(bool isActive) // 함수 값을 boolean으로 받아서 true 값을 보낼때 텍스트 보여주게 만들기
    {
        mRestartText.gameObject.SetActive(isActive);
        if(isActive)
        {
			mAlphaAnimRoutine = StartCoroutine(RestartTextRoutine());

		}
        else
        {
			StopCoroutine(mAlphaAnimRoutine);
		}

	}

    private IEnumerator RestartTextRoutine()
    {
        WaitForSeconds pointOne = new WaitForSeconds(0.1f);
        mRestartText.color = Color.white;
		Color colorgap = Color.black * 0.1f;
        float timer = 1;
        bool bDown = true;
        while (true)
        {
            yield return pointOne;
            if (bDown)
            {
				mRestartText.color -= colorgap;
			}
            else
            {
				mRestartText.color += colorgap;
			}
			timer -= 0.1f;

            if(timer <= 0)
            {
                bDown = !bDown; 
                timer = 1;
            }
            
        }
    }

    public void ShowState(string value) // 점수는 값을 string으로 받아서 텍스트를 계속 최신화해주기 
    {
        mStateText.text = value; // 이렇게 되면 Gamecontroller가 UI를 관리하게 되는 비대현상이 생김. 추후에 어떻게 해결할지 고민해보기
    }

    // Update is called once per frame
    public void ShowScore(float score)
    {
        mScoreText.text = "Score :" + score.ToString();
    }
}
