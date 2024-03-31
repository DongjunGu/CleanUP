using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeButton : MonoBehaviour
{
    public GameObject bridgeButton;
    private bool bridgeActivated = false;
    public Animator animator;
    public GameObject bridgeBlock;
    
    bool isActive = false;
    void Start()
    {

    }
    void Update()
    {
        ActiveBridge();
        StartAnimation();
    }

    void ActiveBridge()
    {
        if (bridgeActivated)
        {
            return;
        }

        if (Pushable.allBlockSet)
        {
            BoxCollider boxcoll = GetComponent<BoxCollider>();
            boxcoll.enabled = true;
            bridgeButton.SetActive(true);
            
            bridgeActivated = true;
        }
    }
    void StartAnimation()
    {
        if(isActive)
        {
            animator.SetBool("playerIn", true);
            Animator bridgeAnim = bridgeBlock.GetComponent<Animator>();
            bridgeAnim.enabled = true;
        }
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isActive = true;
        }
    }
    //void OnTriggerExit(Collider other)
    //{
    //    Debug.Log("Exit");
    //    animator.SetBool("playerIn", false);
    //}
}
