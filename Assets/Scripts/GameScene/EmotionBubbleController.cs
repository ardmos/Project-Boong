using UnityEngine;
using UnityEngine.UI;

public class EmotionBubbleController : MonoBehaviour
{
    public Sprite[] iconEmotionSprites;
    public Image iconEmotion;

    // PlayerEmotion�� ���缭 �˸��� ���� ���������� ��ü.
    // �����ϸ� 1�� �Ŀ� ����. 
    public void InitEmotionBubble(PlayerEmotions emotion)
    {
        iconEmotion.sprite = iconEmotionSprites[(int)emotion];
        Destroy(gameObject, 1f);
    }
}
