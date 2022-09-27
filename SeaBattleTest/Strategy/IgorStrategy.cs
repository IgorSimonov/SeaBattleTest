using OpenQA.Selenium;
using SeaBattleTest.Utilities;

namespace SeaBattleTest.Strategy
{
    /// <summary>
    /// Стратегия игры в морской бой.
    /// </summary>
    public class IgorStrategy
    {
        #region поля

        private const int Empty = 0;
        private const int Miss = 1;
        private const int Hit = 2;
        private const int Destroyed = 3;
        private int[,] _table = new int[10, 10];
        private List<Tuple<int, int>> _coordinates;
        private List<Tuple<int, int>> _currentSteps = new();
        private Steps _steps;

        #endregion

        /// <summary>
        /// Создаёт экземпляр.
        /// </summary>
        /// <param name="steps">Провайдер к старнице.</param>
        /// <exception cref="ArgumentNullException">Ссылка не указывает на экземпляр класса.</exception>
        public IgorStrategy(Steps steps)
        {
            _steps = steps ?? throw new ArgumentNullException(nameof(steps), "Ссылка не указывает на экземпляр класса.");
            InitializeStrategy();
        }

        /// <summary>
        /// Играет в морской бой.
        /// </summary>
        /// <returns>Результат игры.</returns>
        public string Play()
        {
            string res = default;

            try
            {
                foreach (var (y, x) in _coordinates)
                {
                    if (!AttackShip(y, x))
                    {
                        PrintTable();
                        continue;
                    }

                    PrintTable();
                    FinishShip();
                    PrintTable();
                }
            }
            catch (NoSuchElementException e)
            {
                res = e.Message;
            }

            if (string.IsNullOrEmpty(res))
            {
                res = _steps.GetNotification();
            }

            return res;
        }

        #region приватные методы

        private void PrintTable()
        {
            for (int i = 0; i < _table.GetLength(0); i++)
            {
                for (int j = 0; j < _table.GetLength(1); j++)
                {
                    Console.Write($"{_table[i, j]}\t");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        private bool CheckCoordinates(int y, int x) => (x >= 0 && x < _table.GetLength(0) && y >= 0 && y < _table.GetLength(1));

        private bool AddMiss(int y, int x)
        {
            if (!CheckCoordinates(y, x))
            {
                return false;
            }

            _table[y, x] = Miss;
            return true;
        }

        private bool AttackShip(int y, int x)
        {
            if (_table[y, x] != Empty)
            {
                return false;
            }

            if (!_steps.Fire(y, x))
            {
                AddMiss(y, x);
                return false;
            }

            _table[y, x] = Hit;
            _currentSteps.Add(new Tuple<int, int>(y, x));

            return true;
        }

        private void FinishShip()
        {
            var x = _currentSteps[^1].Item2;
            var y = _currentSteps[^1].Item1;

            bool destroyed = _steps.FinishedShip(y, x);

            bool locationShipAsixY = false;

            if (!destroyed)
            {
                locationShipAsixY = CheckLocationShipAxisY(y, x);

                destroyed = _steps.FinishedShip(y, x);
            }

            while (!destroyed)
            {
                _currentSteps.Sort();

                if (locationShipAsixY)
                {
                    if (ShootFromLast(_currentSteps[^1].Item1 + 1, _currentSteps[^1].Item2, out destroyed))
                    {
                        continue;
                    }

                    if (ShootFromFirst(_currentSteps[0].Item1 - 1, _currentSteps[0].Item2, out destroyed))
                    {
                        continue;
                    }
                }

                if (ShootFromLast(_currentSteps[^1].Item1, _currentSteps[^1].Item2 + 1, out destroyed))
                {
                    continue;
                }

                ShootFromFirst(_currentSteps[0].Item1, _currentSteps[0].Item2 - 1, out destroyed);
            }

            foreach (var item in _currentSteps)
            {
                _table[item.Item1, item.Item2] = Destroyed;

                if (!_currentSteps.Contains(new Tuple<int, int>(item.Item1 + 1, item.Item2 + 1)))
                {
                    AddMiss(item.Item1 + 1, item.Item2 + 1);
                }

                if (!_currentSteps.Contains(new Tuple<int, int>(item.Item1 + 1, item.Item2 - 1)))
                {
                    AddMiss(item.Item1 + 1, item.Item2 - 1);
                }

                if (!_currentSteps.Contains(new Tuple<int, int>(item.Item1 - 1, item.Item2 - 1)))
                {
                    AddMiss(item.Item1 - 1, item.Item2 - 1);
                }

                if (!_currentSteps.Contains(new Tuple<int, int>(item.Item1 - 1, item.Item2 + 1)))
                {
                    AddMiss(item.Item1 - 1, item.Item2 + 1);
                }

                if (!_currentSteps.Contains(new Tuple<int, int>(item.Item1 + 1, item.Item2)))
                {
                    AddMiss(item.Item1 + 1, item.Item2);
                }

                if (!_currentSteps.Contains(new Tuple<int, int>(item.Item1 - 1, item.Item2)))
                {
                    AddMiss(item.Item1 - 1, item.Item2);
                }

                if (!_currentSteps.Contains(new Tuple<int, int>(item.Item1, item.Item2 - 1)))
                {
                    AddMiss(item.Item1, item.Item2 - 1);
                }

                if (!_currentSteps.Contains(new Tuple<int, int>(item.Item1, item.Item2 + 1)))
                {
                    AddMiss(item.Item1, item.Item2 + 1);
                }
            }

            _currentSteps.Clear();
        }

        private bool CheckLocationShipAxisY(int y, int x)
        {
            return CheckLocationShipAxis(y - 1, x) || CheckLocationShipAxis(y + 1, x);
        }

        private bool CheckLocationShipAxis(int y, int x)
        {
            if (CheckCoordinates(y, x))
            {
                if (AttackShip(y, x))
                {
                    return true;
                }
            }

            return false;
        }

        private bool ShootFromLast(int y, int x, out bool destroyed)
        {
            if (CheckCoordinates(y, x))
            {
                if (AttackShip(y, x))
                {
                    destroyed = _steps.FinishedShip(_currentSteps[^1].Item1, _currentSteps[^1].Item2);
                    return true;
                }
            }

            destroyed = false;
            return false;
        }

        private bool ShootFromFirst(int y, int x, out bool destroyed)
        {
            if (CheckCoordinates(y, x))
            {
                if (AttackShip(y, x))
                {
                    destroyed = _steps.FinishedShip(_currentSteps[0].Item1, _currentSteps[0].Item2);
                    return true;
                }
            }

            destroyed = false;
            return false;
        }

        private void InitializeStrategy()
        {
            _coordinates = new()
            {
                new Tuple<int, int>(2, 0),
                new Tuple<int, int>(3, 1),
                new Tuple<int, int>(6, 0),
                new Tuple<int, int>(7, 1),
                new Tuple<int, int>(9, 2),
                new Tuple<int, int>(8, 3),
                new Tuple<int, int>(9, 6),
                new Tuple<int, int>(8, 7),
                new Tuple<int, int>(7, 9),
                new Tuple<int, int>(6, 8),
                new Tuple<int, int>(3, 9),
                new Tuple<int, int>(2, 8),
                new Tuple<int, int>(0, 7),
                new Tuple<int, int>(1, 6),
                new Tuple<int, int>(0, 3),
                new Tuple<int, int>(1, 2),
                new Tuple<int, int>(2, 4),
                new Tuple<int, int>(3, 5),
                new Tuple<int, int>(4, 3),
                new Tuple<int, int>(5, 2),
                new Tuple<int, int>(6, 4),
                new Tuple<int, int>(7, 5),
                new Tuple<int, int>(5, 6),
                new Tuple<int, int>(4, 7),
                new Tuple<int, int>(0, 2),
                new Tuple<int, int>(2, 2),
                new Tuple<int, int>(3, 3),
                new Tuple<int, int>(4, 0),
                new Tuple<int, int>(5, 1),
                new Tuple<int, int>(6, 2),
                new Tuple<int, int>(7, 3),
                new Tuple<int, int>(8, 0),
                new Tuple<int, int>(9, 4),
                new Tuple<int, int>(8, 5),
                new Tuple<int, int>(9, 8),
                new Tuple<int, int>(7, 7),
                new Tuple<int, int>(5, 4),
                new Tuple<int, int>(4, 5),
                new Tuple<int, int>(5, 9),
                new Tuple<int, int>(4, 8),
                new Tuple<int, int>(3, 7),
                new Tuple<int, int>(2, 6),
                new Tuple<int, int>(1, 9),
                new Tuple<int, int>(0, 5),
                new Tuple<int, int>(1, 4),
                new Tuple<int, int>(1, 0),
                new Tuple<int, int>(1, 1),
                new Tuple<int, int>(2, 3),
                new Tuple<int, int>(4, 2),
                new Tuple<int, int>(6, 3),
                new Tuple<int, int>(8, 1),
                new Tuple<int, int>(8, 2),
                new Tuple<int, int>(9, 1),
                new Tuple<int, int>(7, 4),
                new Tuple<int, int>(6, 5),
                new Tuple<int, int>(7, 6),
                new Tuple<int, int>(8, 8),
                new Tuple<int, int>(8, 9),
                new Tuple<int, int>(5, 7),
                new Tuple<int, int>(3, 6),
                new Tuple<int, int>(2, 5),
                new Tuple<int, int>(3, 4),
                new Tuple<int, int>(0, 8),
                new Tuple<int, int>(1, 8),
                new Tuple<int, int>(1, 7),
                new Tuple<int, int>(0, 0),
                new Tuple<int, int>(0, 2),
                new Tuple<int, int>(0, 4),
                new Tuple<int, int>(0, 6),
                new Tuple<int, int>(0, 9),
                new Tuple<int, int>(1, 3),
                new Tuple<int, int>(1, 5),
                new Tuple<int, int>(2, 1),
                new Tuple<int, int>(2, 9),
                new Tuple<int, int>(2, 7),
                new Tuple<int, int>(3, 0),
                new Tuple<int, int>(3, 2),
                new Tuple<int, int>(3, 8),
                new Tuple<int, int>(4, 1),
                new Tuple<int, int>(4, 4),
                new Tuple<int, int>(4, 6),
                new Tuple<int, int>(4, 9),
                new Tuple<int, int>(5, 0),
                new Tuple<int, int>(5, 3),
                new Tuple<int, int>(5, 5),
                new Tuple<int, int>(6, 1),
                new Tuple<int, int>(6, 7),
                new Tuple<int, int>(6, 9),
                new Tuple<int, int>(7, 0),
                new Tuple<int, int>(7, 2),
                new Tuple<int, int>(7, 8),
                new Tuple<int, int>(8, 4),
                new Tuple<int, int>(8, 6),
                new Tuple<int, int>(9, 0),
                new Tuple<int, int>(9, 3),
                new Tuple<int, int>(9, 5),
                new Tuple<int, int>(9, 7),
                new Tuple<int, int>(9, 9)
            };
        }

        #endregion

    }
}
