using System;
using _Scripts;
using Character;
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
      AudioManager.Instance.StopMusic();
      DOVirtual.DelayedCall(0.2f, delegate
      {
         AudioManager.Instance.PlayMusicBG();
      });
      state = StateGame.WaitingChoiceLevel;
   }
   public void PlayGame(int indexLevel)
   {
      AnimationTranslate.Instance.StartLoading(delegate
      {
         level = indexLevel;
         AnimationTranslate.Instance.DisplayLoading(false);
         spawnLevel.SpawmLevel(indexLevel - 1);
         state = StateGame.Playing;
         _Scripts.UI.UIController.Instance.UIInGame.ShowDisPlayGame();
         AudioManager.Instance.StopMusic();
         DOVirtual.DelayedCall(0.5f, delegate
         {
            AudioManager.Instance.PlayInGameMusic();
         });
      });
   }

   public void BackHome()
   {
      AudioManager.Instance.PlaySoundButtonClick();
      state = StateGame.WaitingChoiceLevel;
      _Scripts.UI.UIController.Instance.UIWin.DisplayWin(false, delegate
      {
         AnimationTranslate.Instance.StartLoading(delegate
         {
            AnimationTranslate.Instance.DisplayLoading(false);
            _Scripts.UI.UIController.Instance.UIInGame.ShowDisplayHome();
            spawnLevel.DestroyMap();
            AudioManager.Instance.StopMusic();
            DOVirtual.DelayedCall(0.5f, delegate
            {
               AudioManager.Instance.PlayMusicBG();
            });
         });
      });
     
   }
   public void NextLevel()
   {
      AudioManager.Instance.PlaySoundButtonClick();
      state = StateGame.WaitingChoiceLevel;
      _Scripts.UI.UIController.Instance.UIWin.DisplayWin(false, delegate
      {
         AnimationTranslate.Instance.StartLoading(delegate
         {
            /*UIController.Instance.UIInGame.ShowDisplayHome();*/
            spawnLevel.SpawmLevel(level - 1);
            AnimationTranslate.Instance.DisplayLoading(false);
            state = StateGame.Playing;
         });
      });
   }

   public void Replay()
   {
      AudioManager.Instance.PlaySoundButtonClick();
      _Scripts.UI.UIController.Instance.UIWin.DisplayWin(false, delegate
      {
         AnimationTranslate.Instance.StartLoading(delegate
         {
            AnimationTranslate.Instance.DisplayLoading(false);
            spawnLevel.SpawmLevel(level - 1);
         });
      });
      
   }

   public void Win()
   {
      state = StateGame.Win;
      if (PlayerController.Instance != null)
      {
         PlayerController.Instance.gameObject.SetActive(false);
      }
      _Scripts.UI.UIController.Instance.UIWin.DisplayWin(true);
      level += 1;
      MapLevelManager.Instance.ListBtn[level].IsLock = true;
      AudioManager.Instance.PlaySoundWin();
   }
   
}
