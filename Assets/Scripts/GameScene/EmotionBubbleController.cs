using UnityEngine;
using UnityEngine.UI;

public class EmotionBubbleController : MonoBehaviour
{
    public Sprite[] iconEmotionSprites;
    public Image iconEmotion;

    // PlayerEmotion에 맞춰서 알맞은 감정 아이콘으로 교체.
    // 시작하면 1초 후에 폭발. 
    public void InitEmotionBubble(PlayerEmotions emotion)
    {
        iconEmotion.sprite = iconEmotionSprites[(int)emotion];
        Destroy(gameObject, 1f);
    }
}
