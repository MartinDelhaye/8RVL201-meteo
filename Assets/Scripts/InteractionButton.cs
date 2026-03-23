using UnityEngine;

public class InteractionButton : MonoBehaviour
{
    public void NextDay()
    {
        GestionDate.Instance.AddDay(1);
    }
}