using UnityEngine;

public class BackGround : MonoBehaviour
{
    public Camera mainCamera;
    // 控制视差效果的比例（0～1之间）
    public float parallaxFactor = 0.5f;
    private Vector3 startCameraPos;
    private Vector3 startBackgroundPos;

    void Start()
    {
        if (mainCamera != null)
        {
            // 记录初始位置
            startCameraPos = mainCamera.transform.position;
            startBackgroundPos = transform.position;
        }

    }

    void Update()
    {
        if (mainCamera != null)
        {
            // 计算相机在 X 方向上的移动距离
            float deltaX = mainCamera.transform.position.x - startCameraPos.x;
            // 计算新的 X 坐标，加上视差效果
            float newX = startBackgroundPos.x + deltaX * parallaxFactor;
            // 直接使用摄像机的 Y 坐标，确保背景高度始终覆盖视野
            float newY = mainCamera.transform.position.y;
            // Z 轴保持原始位置
            transform.position = new Vector3(newX, newY, startBackgroundPos.z);
        }

    }
}
