using Aquality.Selenium.Browsers;
using NUnit.Framework.Internal;
using SeaBattleTest.Data;
using SeaBattleTest.PageObjects;
using SeaBattleTest.Strategy;
using SeaBattleTest.Utilities;

namespace SeaBattleTest.Tests
{
    /// <summary>
    /// Тесты сайта с игрой Морской бой.
    /// </summary>
    [TestFixture]
    public class SeaBattleTest : SetUpTest
    {
        /// <summary>
        /// Тест игры морской бой.
        /// </summary>
        [Test]
        public void SeaBattleGameTest()
        {
            AqualityServices.Browser.GoTo(TestData.Url);
            StartSeaBattlePageObject startGamePage = new();
            Assert.IsTrue(startGamePage.State.IsDisplayed, TestData.MessageStartPageNotOpen);

            startGamePage.SelectRandomEnemy();
            var shufleShipsCount = RandomData.GetRandomData(TestData.MinValue, TestData.MaxValue);

            for (int i = 0; i < shufleShipsCount; i++)
            {
                startGamePage.ShuffleShips();
            }

            startGamePage.StartGame();
            Assert.IsTrue(startGamePage.GameStarted(), TestData.MessageGameNotStarted);

            Steps steps = new Steps(startGamePage);
            IgorStrategy igorStrategy = new(steps);
            Assert.AreEqual(TestData.ExpectedResultGame, igorStrategy.Play());
        }
    }
}
