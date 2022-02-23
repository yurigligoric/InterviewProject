using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    
    Rigidbody rb;
    public GameObject popUpBox;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GameObject.Find("CharacterSphere(Clone)").GetComponent<Rigidbody>();


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(Vector3.left);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(Vector3.right);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(Vector3.up);
        }
        else if (Input.GetKey(KeyCode.S))
        {
             rb.AddForce(Vector3.down);
        }
    }

    void onCollisoStay(Collision col)
    {
        if(col.gameObject.tag == ("box"))
        {
            ShowFloatingText();
        }
    }

    void ShowFloatingText()
    {
      //  Instatiate(popUpBox, transform.position, Quaternion.identity, transform);
    }

}
