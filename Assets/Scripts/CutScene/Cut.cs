
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class Cut : MonoBehaviour
{
    [SerializeField] protected DOTweenAnimation cameraTween;
    [SerializeField] protected List<Color> cameraBackground;
    
    [SerializeField] protected List<string> texts;


    public abstract void ShowText(string sceneName);

}
