using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    [SerializeField] private Renderer rend;

    public void SetupEnemy(EnemyData data)
    {
        rend.material.mainTexture = data.icon.texture;
        rend.material.SetTexture("_BumpMap", data.normalMap);
    }
}
