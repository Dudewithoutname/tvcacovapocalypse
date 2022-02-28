public class TabPlay : MenuTab
{
    protected override void onButtonClicked(int id)
    {
        switch (id)
        {
            case 0:
                MenuController.Singleton.ChangeTab(2); // Singlep
                break;
            case 1:
                MenuController.Singleton.ChangeTab(3); // Multip
                break;
        }
    }
}