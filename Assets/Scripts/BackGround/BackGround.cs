using UnityEngine;

public class BackGround : MonoBehaviour
{
    public Camera mainCamera;
    // �����Ӳ�Ч���ı�����0��1֮�䣩
    public float parallaxFactor = 0.5f;
    private Vector3 startCameraPos;
    private Vector3 startBackgroundPos;

    void Start()
    {
        if (mainCamera != null)
        {
            // ��¼��ʼλ��
            startCameraPos = mainCamera.transform.position;
            startBackgroundPos = transform.position;
        }

    }

    void Update()
    {
        if (mainCamera != null)
        {
            // ��������� X �����ϵ��ƶ�����
            float deltaX = mainCamera.transform.position.x - startCameraPos.x;
            // �����µ� X ���꣬�����Ӳ�Ч��
            float newX = startBackgroundPos.x + deltaX * parallaxFactor;
            // ֱ��ʹ��������� Y ���꣬ȷ�������߶�ʼ�ո�����Ұ
            float newY = mainCamera.transform.position.y;
            // Z �ᱣ��ԭʼλ��
            transform.position = new Vector3(newX, newY, startBackgroundPos.z);
        }

    }
}
