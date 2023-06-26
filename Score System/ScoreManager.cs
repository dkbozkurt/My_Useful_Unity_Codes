// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using CpiTemplate.Playable.Scripts.Helpers;
using TMPro;
using UnityEngine;

namespace Score_System
{
	public class ScoreManager : SingletonBehaviour<ScoreManager>
	{
		public static event Action OnScoreTargetAchieved;
		
		[Header("Score Properties")]
		[SerializeField] private TextMeshProUGUI _scoreText;
		[SerializeField] private int _startingScore = 0;
		[SerializeField] private int _scoreMultiplier = 1;
		[Space] [SerializeField] private bool _checkForTargetScore = false;
		[SerializeField] private int _targetScore = 9999;
		
		public int GetScore => _score;
		
		private int _score;
		
		private void Awake()
		{
			_score = _startingScore;
		}

		private void OnEnable()
		{
        
		}
	
		private void OnDisable()
		{
        
		}	
	
		private void Start()
		{
        
		}
		
		public void Increase()
		{
			_score += _scoreMultiplier;
			SetText();
			
			CheckIfAchieved();
		}

		public void Decrease()
		{
			_score -= _scoreMultiplier;
			SetText();
			
			CheckIfAchieved();
		}

		public void Set(int targetScore)
		{
			_score = targetScore;
			SetText();

			CheckIfAchieved();
		}
		
		public void IncreaseWithExternalValue(int value)
		{
			_score += value;
			SetText();
			
			CheckIfAchieved();
		}
		
		public void DecreaseWithExternalValue(int value)
		{
			_score -= value;
			SetText();
			
			CheckIfAchieved();
		}
		
		private void SetText()
		{
			_scoreText.text = _score.ToString();
		}
		
		private void SetTextExternal(int scoreToSet)
		{
			SetTextExternal(scoreToSet.ToString());	
		}
		
		private void SetTextExternal(string scoreToSet)
		{
			_scoreText.text = scoreToSet;
		}
		
		public void Reset()
		{
			_score = 0;
			SetText();
		}

		public void ResetToInitial()
		{
			_score = _startingScore;
		}

		private void CheckIfAchieved()
		{
			if(!_checkForTargetScore) return;

			if (_score >= _targetScore)
			{
				OnScoreTargetAchieved?.Invoke();
			}
		}
		
		protected override void OnAwake()
		{ }
	}
}
