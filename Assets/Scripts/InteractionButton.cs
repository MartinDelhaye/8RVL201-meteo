using UnityEngine;

public class InteractionButton : MonoBehaviour
{
    public void NextDay()
    {
        RecupDate.Instance.AddDay(1);
    }
}