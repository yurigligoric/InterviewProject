using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpSystem : MonoBehaviour
{
    
    public GameObject popUpBox;
    public Animator animator;
    public TMP_Text popUpText;
    public string text;
    int randomText;


    public void PopUp(int dialogue)
    {
        popUpBox.SetActive(true);

        randomText = dialogue;

        if (randomText == 1)
        {
            text = "You Got Me";
        }
        if (randomText == 2)
        {
            text = "I'm a red bal";
        }
        if (randomText == 3)
        {
            text = "I'm a shiny red ball who can move around and talk";
        }
        if (randomText == 4)
        {
            text = "Ha";
        }
        popUpText.text = text;
        animator.SetTrigger("pop");
        StartCoroutine(ClosePop());
    }    
    
    IEnumerator ClosePop()
    {
        yield return new WaitForSeconds(2);
        animator.SetTrigger("idle");
    }
    
    


}
