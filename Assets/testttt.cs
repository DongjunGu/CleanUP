using UnityEngine;
using UnityEngine.AI;

public class testttt : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        NewPlayerController.stage = 4;
        Debug.Log("Enter" + NewPlayerController.stage);

    }

}
