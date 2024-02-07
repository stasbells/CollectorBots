using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ScoreBoard : MonoBehaviour
{
    [SerializeField] GameObject _value;

    private readonly int Change = Animator.StringToHash(nameof(Change));

    private TMP_Text _text;
    private Base _base;
    private int _score;

    private void Awake()
    {
        _score = 0;
        _text = GetComponent<TMP_Text>();
        _base = GetComponentInParent<Base>();
    }

    private void OnEnable()
    {
        _base.ScoreChanged += ChangeScoreTo;
    }

    private void OnDisable()
    {
        _base.ScoreChanged -= ChangeScoreTo;
    }

    private void ChangeScoreTo(int value)
    {
        ShowChange(value);

        _score += value;
        _text.text = _score.ToString();
    }

    private void ShowChange(int score)
    {
        if (score > 0)
            _value.GetComponentInChildren<TMP_Text>().text = $"+{score}";

        if (score <= 0)
            _value.GetComponentInChildren<TMP_Text>().text = score.ToString();

        _value.GetComponent<Animator>().SetTrigger(Change);
    }
}