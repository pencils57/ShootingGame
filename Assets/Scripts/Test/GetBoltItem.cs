using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetBoltItem : MonoBehaviour
{
    private Rigidbody mRB;
    [SerializeField]
    private int mSpeed;

    // Start is called before the first frame update
    void Awake()
    {
        mRB = gameObject.GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void OnEnable()
    {
        mRB.velocity = Vector3.back * mSpeed;
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            gameObject.SetActive(false);
        }
    }
}
