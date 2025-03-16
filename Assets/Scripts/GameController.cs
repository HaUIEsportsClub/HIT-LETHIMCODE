using System;
using DG.Tweening;
using UnityEngine;

public class GameController : Singleton<GameController>
{
   public enum StateGame
   {
      ShowTutorial,
      WaitingChoiceLevel,
      Playing,
      Win,
      Losse
   }
   protected override void Awake()
   {
      base.KeepAlive(false);
      base.Awake();
   }

   private StateGame state = StateGame.WaitingChoiceLevel;
   public StateGame State => state;

   [SerializeField] private SpawnLevel spawnLevel;
   [SerializeField] private AnimationTranslate animLoading;
   private int level;
   public SpawnLevel SpawnLevel => spawnLevel;



   public bool IsFirstPlay
   {
      get => PlayerPrefs.GetInt("Tut", 1) == 1;
      set => PlayerPrefs.SetInt("Tut", value ? 1 : 0);
   }
   private void Start()
   {
      level = PlayerPrefs.GetInt("Level_" + level, 1);
      if (IsFirstPlay)
      {
         state = StateGame.ShowTutorial;
         return;
      }

      state = StateGame.WaitingChoiceLevel;
   }
   public void PlayGame()
   {
      animLoading.StartLoading(delegate
      {
         animLoading.DisplayLoading(false);
         spawnLevel.SpawmLevel(level);
         state = StateGame.Playing;
         _Scripts.UI.UIController.Instance.UIInGame.ShowDisPlayGame();
      });
   }

   public void BackHome()
   {
      state = StateGame.WaitingChoiceLevel;
      animLoading.StartLoading(delegate
      {
         animLoading.DisplayLoading(false);
         _Scripts.UI.UIController.Instance.UIInGame.ShowDisplayHome();
      });
   }
   public void NextLevel()
   {
      level += 1;
      state = StateGame.WaitingChoiceLevel;
      animLoading.StartLoading(delegate
      {
         /*UIController.Instance.UIInGame.ShowDisplayHome();*/
         MapLevelManager.Instance.ListBtn[level].IsLock = false;
         spawnLevel.SpawmLevel(level);
         animLoading.DisplayLoading(false);
      });
   }

   public void Replay()
   {
      animLoading.StartLoading(delegate
      {
         animLoading.DisplayLoading(false);
         spawnLevel.SpawmLevel(level);
      });
   }
   
}
