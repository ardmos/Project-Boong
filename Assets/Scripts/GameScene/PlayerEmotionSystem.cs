using UnityEngine;

public enum PlayerEmotions
{
    Win,    // Ż�ⱸ�� ���ٽ�
    Shock,
    GameOver,   // ���ӿ��� ���� ��. Ÿ�Ӿƿ�&����
    Happy
}

public class PlayerEmotionSystem : MonoBehaviour
{
    public Transform emotionBubbleCanvas;
    public GameObject emotionBubblePrefab;

    public void CreateEmotionBubble(PlayerEmotions emotion)
    {
        EmotionBubbleController emotionBubbleController = Instantiate(emotionBubblePrefab, emotionBubbleCanvas).GetComponent<EmotionBubbleController>();
        emotionBubbleController.InitEmotionBubble(emotion);
    }
}
