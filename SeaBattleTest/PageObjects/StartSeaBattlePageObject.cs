using Aquality.Selenium.Browsers;
using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using OpenQA.Selenium;

namespace SeaBattleTest.PageObjects
{
    /// <summary>
    /// Стартовая страница игры.
    /// </summary>
    public class StartSeaBattlePageObject : Form
    {
        #region приватные поля

        private readonly string _emptyCellLocator = "//div[@Class='battlefield battlefield__rival']//td[@Class='battlefield-cell battlefield-cell__empty']//div[@data-y='{0}' and @data-x='{1}']";
        private readonly string _hitCellLocator = "//div[@class='battlefield battlefield__rival']//td[contains(@Class,'battlefield-cell battlefield-cell__hit')]//div[@data-y='{0}' and @data-x='{1}']";
        private readonly string _destroyedShipLocator = "//td[contains(@Class,'battlefield-cell__hit battlefield-cell__done')]//div[@data-y='{0}' and @data-x='{1}']";
        private readonly ITextBox _shuffleShipsText = ElementFactory.GetTextBox(By.CssSelector(".placeships-variant.placeships-variant__randomly"), "Разместить корабли случайным образом");
        private readonly IButton _startBattleBtn = ElementFactory.GetButton(By.CssSelector(".battlefield-start-button"), "Старт игры");
        private readonly ITextBox _randomEnemy = ElementFactory.GetTextBox(By.XPath("//ul[@class='battlefield-start-choose_rival-variants']//a[@class='battlefield-start-choose_rival-variant-link']"), "Случайный оппонент");
        private readonly ITextBox _notificationGameOver = ElementFactory.GetTextBox(By.XPath("//body[contains(@Class, 'body body__17820590200 body__ru body__with-pointerevents body__game')]"), "Конец игры");
        private readonly ITextBox _notificationMoveOn = ElementFactory.GetTextBox(By.XPath("//div[contains(@class, 'move-on')]"), "Начало хода");
        private readonly ITextBox _startedGame = ElementFactory.GetTextBox(By.XPath("//div[contains(@Class, 'notification__game-started')]"), "Начало игры");
        private readonly ITextBox _chooseEnemy = ElementFactory.GetTextBox(By.XPath("//ul[@class='battlefield-start-choose_rival-variants']//a[@class='battlefield-start-choose_rival-variant-link battlefield-start-choose_rival-variant-link__connect']"), "Оппонент по ссылке");
        private readonly ITextBox _notifcation = ElementFactory.GetTextBox(By.XPath("//div[@class = 'notification-message']"), "Результат игры.");

        #endregion

        public StartSeaBattlePageObject() : base(By.CssSelector(".notification.notification__init"), "Стартовая страницы игры морской бой")
        {
        }

        #region методы

        /// <summary>
        /// Текст результат игры.
        /// </summary>
        /// Текст результат игры.
        public string GetTextNotification() => _notifcation.Text;

        /// <summary>
        /// Поиск игры(5 минут).
        /// </summary>
        /// <returns>True, если игра нашлась, иначе false.</returns>
        public bool GameStarted() => _startedGame.State.WaitForDisplayed(TimeSpan.FromMinutes(5));

        /// <summary>
        /// Игра закончилась.
        /// </summary>
        /// <returns>True, если игра закончилась, иначе false.</returns>
        public bool GameEnded() => _notificationGameOver.State.WaitForDisplayed(TimeSpan.FromSeconds(1));

        /// <summary>
        /// Ваш ход.
        /// </summary>
        /// <returns>True, если игра ваш ход, иначе false.</returns>
        public bool MoveOn() => _notificationMoveOn.State.WaitForDisplayed(TimeSpan.FromMinutes(5));

        /// <summary>
        /// Расположить корабли случайным образом.
        /// </summary>
        public void ShuffleShips()
        {
            _shuffleShipsText.Click();
        }

        /// <summary>
        /// Начать игру.
        /// </summary>
        public void StartGame()
        {
            _startBattleBtn.Click();
        }

        /// <summary>
        /// Выбрать случайного опоонента.
        /// </summary>
        public void SelectRandomEnemy()
        {
            _randomEnemy.Click();
        }

        /// <summary>
        /// Стреляет по вражеским кораблям, по координатам.
        /// </summary>
        /// <param name="y">Координата Y(строка).</param>
        /// <param name="x">Координата X(столбец).</param>
        /// <returns>True, если есть попадание по кораблю, иначе false.</returns>
        public bool AttackEnemyCell(int y, int x)
        {
            var cell = ElementFactory.GetTextBox(By.XPath(string.Format(_emptyCellLocator, y, x)), $"Клетка по координатам  y - {y}, x - {x}");
            cell.Click();

            return CheckAttack(y, x);
        }

        /// <summary>
        /// Проверят было ли попадание по кораблю, по координатам. 
        /// </summary>
        /// <param name="y">Координата Y(строка).</param>
        /// <param name="x">Координата X(столбец).</param>
        /// <returns>True, если было подание, иначе false</returns>
        private bool CheckAttack(int y, int x) => ElementFactory.GetTextBox(By.XPath(string.Format(_hitCellLocator, y, x)), $"Клетка по координатам  y - {y}, x - {x}").State.WaitForDisplayed(TimeSpan.FromSeconds(1));

        /// <summary>
        /// Проверят уничтожен ли корабль по координатам.
        /// </summary>
        /// <param name="y">Координата Y(строка).</param>
        /// <param name="x">Координата X(столбец).</param>
        /// <returns>True, если корабль уничтожен, иначе false</returns>
        public bool KilledShip(int y, int x) => ElementFactory.GetTextBox(By.XPath(string.Format(_destroyedShipLocator, y, x)), "Уничтоженный корабль").State.WaitForDisplayed(TimeSpan.FromSeconds(2));

        #endregion
    }
}
