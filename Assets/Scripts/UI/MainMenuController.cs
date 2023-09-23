using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void OnUpdateMenuDisplay(bool showMenu)
    {
        gameObject.SetActive(showMenu);
    }
}
