using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToPlayer : MonoBehaviour
{
    
    GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        player = GameObject.Find("CharacterSphere(Clone)");
           
        transform.parent = player.transform;
        
    }
}
