

public class Cut0 : Cut
{
    public override void ShowText(string sceneName)//id로 실행되면 자동으로 텍스트 나오게 됨
    {
        switch (sceneName)
        {
            case "scene0":
                cameraTween.endValueColor = cameraBackground[0];
                CutManager.Instance.ShowText(texts[0]);
                break;
            case "scene1":
                cameraTween.endValueColor = cameraBackground[1];
                CutManager.Instance.ShowText(texts[1]);
                break;
            
        }
        cameraTween.CreateTween(true, true);
    }
    
}
