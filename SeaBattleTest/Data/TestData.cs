namespace SeaBattleTest.Data
{
    /// <summary>
    /// Тестовые данные.
    /// </summary>
    public static class TestData
    {
        /// <summary>
        /// Юрл.
        /// </summary>
        public const string Url = "http://ru.battleship-game.org/";

        /// <summary>
        /// Минимальное числовое значение для шафла кораблей.
        /// </summary>
        public const int MinValue = 1;

        /// <summary>
        /// Максималное числовое значение для шафла кораблей.
        /// </summary>
        public const int MaxValue = 15;

        /// <summary>
        /// Ожидаемый результат игры..
        /// </summary>
        public const string ExpectedResultGame = "Игра закончена. Поздравляем, вы победили!";

        /// <summary>
        /// Стартовая страница не открылась.
        /// </summary>
        public const string MessageStartPageNotOpen = "Стартовая страница морского боя не открылась.";

        /// <summary>
        /// Игра не началась.
        /// </summary>
        public const string MessageGameNotStarted = "Игра не началась.";
    }
}
