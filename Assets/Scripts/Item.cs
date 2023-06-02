using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eItemType{
    Bolt,
    Supporter
}
public class Item : MonoBehaviour
{
    [SerializeField]
    private eItemType mID;
    private Rigidbody mRB;
    [SerializeField]
    private float mSpeed;
    [SerializeField]
    private Vector3 mTorque;
    // Start is called before the first frame update
    private void Awake()
    {
        mRB = gameObject.GetComponent<Rigidbody>();
        mRB.velocity = Vector3.back * mSpeed;
        mRB.angularVelocity = mTorque;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")){
            other.gameObject.SendMessage("GetItem", mID );  //SendMessageOptions.DontRequireReceiver 아무것도 안넘겨 줄때
            gameObject.SetActive(false);
        }
    }
}
