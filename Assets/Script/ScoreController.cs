using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour,IAddScore
{
    [SerializeField, Tooltip("スコア")]
    Text _scoerText;
    int _score;
    // Start is called before the first frame update
    void Start()
    {
        _scoerText = _scoerText.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddScore(int score)
    {
        int tempScore = _score; // 追加前のスコア
        _score += score;
        DOTween.To(() => tempScore,
             x => _scoerText.text = tempScore.ToString("00000000"),
             _score,
             2)
             .OnComplete(() => _scoerText.text = _score.ToString("00000000"));
    }
}
