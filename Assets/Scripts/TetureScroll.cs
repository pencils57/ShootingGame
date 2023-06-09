using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetureScroll : MonoBehaviour
{
    private Renderer mRend;
    private Material mMat;
    [SerializeField]
    private float mSpeed;
    private float mOffset;
    // Start is called before the first frame update
    void Start()
    {
        mRend= GetComponent<Renderer>();
        mMat = mRend.material;
        mOffset = 0;
    }

    // Update is called once per frame
    void Update()
    {
        mOffset += Time.deltaTime * mSpeed;
        mMat.SetTextureOffset("_MainTex", new Vector2(0, mOffset));
    }
}
