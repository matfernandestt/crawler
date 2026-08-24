using UnityEngine;
using UnityEngine.UI;

public class EnemyVisual : MonoBehaviour
{
    [SerializeField] private Image image;

    public void SetupEnemy(EnemyData data)
    {
        image.sprite = data.icon;
    }
}
