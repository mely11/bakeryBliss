using UnityEngine;

public class GameController : MonoBehaviour
{

    public MainMenuController mainMenu;

    private bool showMainCanvas = true;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            ToggleMainMenu(!showMainCanvas);
        }
    }

    private void OnEnable()
    {
        ToggleMainMenu(showMainCanvas);
    }

    public void ToggleMainMenu(bool show)
    {
        if (show)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        mainMenu.OnUpdateMenuDisplay(show);
        showMainCanvas = show;
    }
}
