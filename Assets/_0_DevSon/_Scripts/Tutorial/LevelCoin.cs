using System;
using _Scripts.Event;
using _Scripts.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Scripts.Tutorial
{
    public class LevelCoin : MonoBehaviour
    {


        public GameObject Booster;
        public TMP_Text textScore;
        public UnityEvent EnableEvent;
        public UnityEvent DisableEvent;
        
        public int _scoreEat;
        public void OnEnable()
        {
            ManagerSound.Instance.SetValueLevelCoint(true);
            EnableEvent?.Invoke();
            _scoreEat = 0;
            textScore.text = $"{_scoreEat}";
            LevelCointEvent.OnStartLevelCoin?.Invoke();
            ItemEvent.OnAddScore += OnAddScore;
            LevelCointEvent.OnLevelCoinGet += CoinGet;
            Booster.SetActive(false);
        }

        private int CoinGet()
        {
            return _scoreEat;
            
        }

        private void OnAddScore(int score)
        {
            _scoreEat += score;
            textScore.text = $"{_scoreEat}";
            ManagerSound.Instance?.PlayEffectSound(EnumEffectSound.CollectionCoin);
        }


        public void OnDisable()
        {
            LevelCointEvent.OnEndLevelCoin?.Invoke();
            DisableEvent?.Invoke();
            ItemEvent.OnAddScore -= OnAddScore;
            LevelCointEvent.OnLevelCoinGet -= CoinGet;
            Booster.SetActive(true);
            ManagerSound.Instance?.SetValueLevelCoint(false);
        }
    }
}