using UnityEngine.SceneManagement;

namespace SpaceShooter
{
    public class LevelSequenceController : SingletonBase<LevelSequenceController>
    {
        public static string MainMenuSceneNickname = "main_menu";

        public Episode CurrentEpisode { get; private set; }

        public int CurrentLevel { get; private set; }

        public bool LastLevelResult { get; private set; }

        public PlayerStatistics LevelStatistics { get; private set; }

        public static SpaceShip PlayerShip { get; set; }

        public void StartEpisode (Episode e)
        {
            CurrentEpisode = e;
            CurrentLevel = 0;

            /// <summary>
            /// Cбрасываем статы перед началом эпизода.
            /// </summary>

            LevelStatistics = new PlayerStatistics();
            LevelStatistics.Reset();

            SceneManager.LoadScene(e.Levels[CurrentLevel]);
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
        }
        public void FinishCurrentLevel (bool success)
        {
            LastLevelResult = success;
            CalculateLevelStatistic();

            ResultPanelController.Instance.ShowResults(LevelStatistics, success);
        }

        public void AdvanceLevel()
        {
            LevelStatistics.Reset();

            CurrentLevel ++;
            if (CurrentEpisode.Levels.Length <= CurrentLevel)
            {
                SceneManager.LoadScene(MainMenuSceneNickname);
            }
            else
            {
                SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
            }
        }

        private void CalculateLevelStatistic()
        {
            LevelStatistics.score = Player.Instance.Score;
            LevelStatistics.numkills = Player.Instance.NumKills;
            LevelStatistics.time = (int)LevelController.Instance.LevelTime;
        }
    }
}