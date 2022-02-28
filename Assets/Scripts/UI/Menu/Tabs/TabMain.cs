using UnityEngine;

public class TabMain : MenuTab
{
    protected override void onButtonClicked(int id)
    {
        switch (id)
        {
            case 0:
                MenuController.Singleton.ChangeTab(1); // play
                break;
            case 1:
                MenuController.Singleton.ChangeTab(2); // character
                break;
            case 2:
                MenuController.Singleton.ChangeTab(3); // achievements
                break;
            case 3:
                MenuController.Singleton.ChangeTab(4); // settings
                break;
            case 4:
                Application.Quit();
                break;
        }
    }
}