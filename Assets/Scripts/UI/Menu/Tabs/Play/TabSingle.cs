using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TabSingle : MenuTab
{
    public Dropdown DifficultySelector;
    
    protected override void onOpen()
    {
        WaveManager.Difficulty = Difficulties.List[(DifficultyType)DifficultySelector.value];
        DifficultySelector.onValueChanged.AddListener(selected => WaveManager.Difficulty = Difficulties.List[(DifficultyType)selected]);    
    }

    protected override void onClose()
    {
        DifficultySelector.onValueChanged.RemoveAllListeners();
    }
    
    protected override void onButtonClicked(int id)
    {
        switch (id)
        {
            case 0:
                StartCoroutine(Load());
                break;
        }
    }

    private IEnumerator Load()
    {
        yield return SceneManager.LoadSceneAsync(1);
    }
}