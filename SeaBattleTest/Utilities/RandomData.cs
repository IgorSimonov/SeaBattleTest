namespace SeaBattleTest.Utilities
{
    /// <summary>
    /// Предоставляет рандомные данные для тестов.
    /// </summary>
    public static class RandomData
    {
        /// <summary>
        /// Предоставляет рандомное число.
        /// </summary>
        /// <param name="minValue">Минимальное значение числа.</param>
        /// <param name="maxValue">Максимальное значение числа.</param>
        /// <returns>Рандомное число.</returns>
        public static int GetRandomData(int minValue, int maxValue) => new Random().Next(minValue, maxValue);
    }
}
