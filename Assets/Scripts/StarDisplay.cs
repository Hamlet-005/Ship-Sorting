using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StarDisplay : MonoBehaviour
{
    public Image star1;
    public Image star2;
    public Image star3;

    public Sprite starSprite;

    public int threeStarMoves = 10; 
    public int twoStarMoves = 5;   

    public int CalculateStars(int movesLeft)
    {
        if (movesLeft >= threeStarMoves) return 3;
        if (movesLeft >= twoStarMoves) return 2;
        return 1;
    }

    public void ShowStars(int starCount)
    {
        Image[] stars = { star1, star2, star3 };

        foreach (Image s in stars)
        {
            s.sprite = starSprite;
            s.color = new Color(0.3f, 0.3f, 0.3f, 1f);
            s.transform.localScale = Vector3.zero;
        }

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.4f);

        for (int i = 0; i < 3; i++)
        {
            int index = i;
            Image star = stars[i];

            seq.AppendCallback(() =>
            {
                if (index < starCount)
                    star.color = Color.white;
            });
            seq.Append(star.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack));
            seq.AppendInterval(0.15f);
        }
    }
}