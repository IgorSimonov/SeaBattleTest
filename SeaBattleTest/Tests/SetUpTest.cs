using Aquality.Selenium.Browsers;

namespace SeaBattleTest
{
    /// <summary>
    /// Тестовые настройки.
    /// </summary>
    public class SetUpTest
    {
        /// <summary>
        /// Настройки перед тестом.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            AqualityServices.Logger.Info("Тестирование игры морской бой началось");
            AqualityServices.Browser.Maximize();
        }

        /// <summary>
        /// Настройки после тестов.
        /// </summary>
        [TearDown]
        public void After()
        {
            AqualityServices.Browser.Driver.Quit();
            AqualityServices.Logger.Info("Тестирование морской бой закончилось");
        }
    }
}