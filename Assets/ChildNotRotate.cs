using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildNotRotate : MonoBehaviour
{
    Quaternion rotation;
 
 void Awake(){
    
 }
 
 void Update()
 {
    rotation = Quaternion.LookRotation(Vector3.up , Vector3.forward);
    transform.rotation = rotation;
 }

}
