using UnityEngine;

public enum PlayerEmotions
{
    Win,    // 탈출구역 접근시
    Shock,
    GameOver,   // 게임오버 됐을 때. 타임아웃&잡힘
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
