using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AsteroidPool : MonoBehaviour
{
    [SerializeField]
    private Asteroid[] mOrigin;
    private List<Asteroid>[] mPool;
    // Start is called before the first frame update
    void Start()
    {
        mPool = new List<Asteroid>[mOrigin.Length];
        for(int i = 0; i < mPool.Length; i++)
        {
            mPool[i] = new List<Asteroid>();
        }
    }
	public Asteroid GetFromPool(int index)
	{
		for (int i = 0; i < mPool[index].Count; i++)
		{
			if (!mPool[index][i].gameObject.activeInHierarchy)
			{
				mPool[index][i].gameObject.SetActive(true);
				return mPool[index][i];
			}
		}
		Asteroid newObj = Instantiate(mOrigin[index]);
		mPool[index].Add(newObj);
		return newObj;
	}
}
