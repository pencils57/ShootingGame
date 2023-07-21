using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float mSpeed, mTilt;

    [SerializeField]
    private float mXMax, mXMin, mZMax, mZMin;

    private Rigidbody mRB;

    [SerializeField]
    private float mFireRate;
    private float mCurrentFireRate;

    [SerializeField]
    private BoltPool mPool;

    [SerializeField]
    private Transform mBoltPos;

    private EffectPool mEffectPool;

    private Vector3 mInput = new Vector3();

    private SoundController mSoundController;

    private GameController mGameController;

    [SerializeField]
    private float mBoltGap, mBoltDir;
    [SerializeField]
    private int mBoltCount = 1;
    [SerializeField]
    private bool mSupporterFlag;
    [SerializeField]
    private GameObject[] mSupporterArr;    //SetActive true false ����ҰŴϱ� Gameobject
    private GameObject mSupporter;
    [SerializeField]
    private Transform[] mSupporterBoltArr; // �����͸� ����ٴϴ� transform�� ����ҰŴϱ� transform

    private float mSupporterFileTime;
    private float mSupporterCurrentTime;
    private bool mSupFireCan;

    private Coroutine mCoSupporterAtk;

    public float InputMag
    {
        get
        {
            return mInput.magnitude;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mRB = GetComponent<Rigidbody>();
        mCurrentFireRate = 0;
        mSoundController = GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundController>();
        mGameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        for (int i = 0; i < mSupporterArr.Length; i++)
        {
            mSupporterArr[i] = Instantiate(mSupporterArr[i], transform.position + new Vector3(-1f + i * 2f, 0, 0), Quaternion.identity);
            mSupporterArr[i].name = "Sup" + (i + 1);
            mSupporterBoltArr[i] = mSupporterArr[i].transform;
            mSupporterArr[i].SetActive(false);
            mSupporterArr[i].transform.SetParent(this.gameObject.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        mInput.x = horizontal;
        mInput.z = vertical;

        mRB.velocity = mInput.normalized * mSpeed;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, mXMin, mXMax), transform.position.y, Mathf.Clamp(transform.position.z, mZMin, mZMax));

        transform.rotation = Quaternion.Euler(0, 0, mTilt * horizontal);

        mCurrentFireRate -= Time.deltaTime;
        if (mCurrentFireRate < 0 && Input.GetButton("Fire1"))
        {
            Fire();
            //�Ѿ� ���� : n
            //���� ������ �Ѿ� ��ġ = -(n - 1) / 2f * �Ѿ� ����
        }



        if (Input.GetKeyDown(KeyCode.C))
        {
            for (int i = 0; i < mSupporterArr.Length; i++)
            {
                mSupporterArr[i].SetActive(true);
            }

            if( mCoSupporterAtk is not null)
            {
                StopCoroutine(mCoSupporterAtk);
            }
            mCoSupporterAtk = StartCoroutine(CoSupporterAtk());
        }
    }

    private IEnumerator CoSupporterAtk()
    {
        float SupAtkTime = 1f;
        float CurrentTime = 0f;
        float TotalTime = 10f;
        while (TotalTime > 0f)
        {
            CurrentTime += Time.deltaTime;
            if (CurrentTime >= SupAtkTime)
            {
                CurrentTime = 0f;
                for (int i = 0; i < mSupporterBoltArr.Length; i++)
                {
                    Bolt newBolt = mPool.GetFromPool();
                    newBolt.transform.position = mSupporterBoltArr[i].transform.position;
                }
            }
            TotalTime -= Time.deltaTime;
            yield return null;
        }
        for (int i = 0; i < mSupporterBoltArr.Length; i++)
        {
            mSupporterArr[i].SetActive(false);

        }
    }


    public void GetItem(eItemType type)
    {
        switch (type)
        {
            case eItemType.Bolt:
                mBoltCount++;
                break;
            case eItemType.Supporter:
                if( mCoSupporterAtk is not null)
            {
                StopCoroutine(mCoSupporterAtk);
            }
            mCoSupporterAtk = StartCoroutine(CoSupporterAtk());
                break;
            default:
                Debug.LogError("Wrong item type" + type);
                break;
        } //enum으로 switch문을 쓸 때 꼭 default에 에러를 넣고 타입을 추가하자.
    }

    private void Fire()
    {
        float startX = (1 - mBoltCount) / 2f * mBoltGap;
        float startDirY = (1 - mBoltCount) * mBoltDir;
        Vector3 pos = mBoltPos.position;
        Quaternion dir = Quaternion.Euler(0, 0, 0);
        pos.x += startX;
        dir.eulerAngles += new Vector3(0, startDirY, 0);

        for (int i = 0; i < mBoltCount; i++)
        {
            Bolt newBolt = mPool.GetFromPool();
            newBolt.transform.position = pos;
            newBolt.transform.rotation = dir;
            mCurrentFireRate = mFireRate;
            pos.x += mBoltGap;
            dir.eulerAngles += new Vector3(0, mBoltDir * 2f, 0);
        }
        mSoundController.PlayEffectSound((int)eSoundtype.FirePla);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (mEffectPool == null)
            {
                mEffectPool = GameObject.FindGameObjectWithTag("EffectPool").GetComponent<EffectPool>();
            }
            Timer effect = mEffectPool.GetFromPool((int)mEffecttype.Player);
            effect.transform.position = transform.position;

            Debug.Log("ExpPlayer");
            mSoundController.PlayEffectSound((int)eSoundtype.ExpPla);

            mGameController.GameOver();

            other.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("GetBolt"))
        {
            mBoltCount++;
            Debug.Log("BoltItem");
        }
    }
}
