using OpenQA.Selenium;
using SeaBattleTest.PageObjects;

namespace SeaBattleTest.Utilities
{
    /// <summary>
    /// Взаимодействие со страницой игры.
    /// </summary>
    public class Steps
    {
        private StartSeaBattlePageObject _startSeaBattlePage;

        /// <summary>
        /// Создаёт экземпляр.
        /// </summary>
        /// <param name="startSeaBattlePage">Страница с игрой.</param>
        /// <exception cref="ArgumentNullException">Ссылка не указывает на экземпляр класса.</exception>
        public Steps(StartSeaBattlePageObject startSeaBattlePage)
        {
            _startSeaBattlePage = startSeaBattlePage ?? throw new ArgumentNullException(nameof(startSeaBattlePage), "Ссылка не указывает на экземпляр класса.");
        }

        /// <summary>
        /// Стреляет по вражеским кораблям, по координатам.
        /// </summary>
        /// <param name="y">Координата Y(строка).</param>
        /// <param name="x">Координата X(столбец).</param>
        /// <returns>True, если есть попадание по кораблю, иначе false.</returns>
        /// <exception cref="NoSuchElementException">Элемент не найден.</exception>
        public bool Fire(int y, int x)
        {
            if (!_startSeaBattlePage.MoveOn())
            {
                if (_startSeaBattlePage.GameEnded())
                {
                    throw new NoSuchElementException(GetNotification());
                }

                throw new NoSuchElementException("Игрок не сходил.");
            }

            return _startSeaBattlePage.AttackEnemyCell(y, x);
        }

        /// <summary>
        /// Уничтожен ли корабль по координатам.
        /// </summary>
        /// <param name="y">Координата Y(строка).</param>
        /// <param name="x">Координата X(столбец).</param>
        /// <returns>True, если уничтожен, иначе false.</returns>
        public bool FinishedShip(int y, int x) => _startSeaBattlePage.KilledShip(y, x);

        /// <summary>
        /// Результат игры.
        /// </summary>
        /// <returns>Результат игры.</returns>
        public string GetNotification() => _startSeaBattlePage.GetTextNotification();
    }
}
