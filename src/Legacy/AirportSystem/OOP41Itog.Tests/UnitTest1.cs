using System;
using Xunit;
using Moq;
using OOP41Itog.Legacy;

namespace OOP41Itog.Tests
{
    public class CodeSmellsTests
    {
        [Fact]
        public void CalculateFee_StandardWeight_ReturnsZero()
        {
            //Классы эквивалентности
            var luggage = new CheckedLuggage { Weight = 20.0, IsFragile = false, NeedsSpecialHandling = false };
            double fee = luggage.CalculateFee();
            Assert.Equal(0.0, fee);
        }

        [Fact]
        public void CalculateFee_ExactlyAtLimit_ReturnsZero()
        {
            //Граничные значения (ровно 23.0 кг)
            var luggage = new CheckedLuggage { Weight = 23.0, IsFragile = false, NeedsSpecialHandling = false };
            double fee = luggage.CalculateFee();
            Assert.Equal(0.0, fee);
        }

        [Fact]
        public void CalculateFee_SlightlyOverweight_AddsCorrectSurcharge()
        {
            //Граничные значения (чуть выше границы: 23.1 кг)
            var luggage = new CheckedLuggage { Weight = 23.1, IsFragile = false, NeedsSpecialHandling = false };
            double fee = luggage.CalculateFee();

            // (23.1 - 23.0) * 10.0 = 1.0
            Assert.Equal(1.0, fee, precision: 2);
        }

        [Fact]
        public void CalculateFee_FragileAndSpecial_AddsBothFees()
        {
            // Комбинация условий
            var luggage = new CheckedLuggage { Weight = 25.0, IsFragile = true, NeedsSpecialHandling = true };
            double fee = luggage.CalculateFee();

            // Перевес: 2 * 10 = 20. Хрупкий: 50. Спец: 100. Итого: 170.
            Assert.Equal(170.0, fee, precision: 2);
        }

        [Fact]
        public void ProcessEverything_AddValidAirport_UpdatesState()
        {
            //Проверка состояния
            var manager = new AirportManager();
            string result = manager.ProcessEverything("addAirport", "SVOO", "Sheremetyevo", 0, 0);
            Assert.Equal("Airport added: SVOO", result);
            Assert.Contains("SVOO", manager.airports);
            Assert.Equal(1, manager.totalFlights); // Да, в плохом коде тут баг,поэтому документирую её тестом!
        }

        [Fact]
        public void ProcessEverything_EmptyAirportName_ReturnsInvalidMessage()
        {
            //Негативное тестирование
            var manager = new AirportManager();
            string result = manager.ProcessEverything("addAirport", "", "Sheremetyevo", 0, 0);
            Assert.Equal("Invalid airport name", result);
            Assert.Empty(manager.airports);
        }

        [Fact]
        public void ProcessEverything_DuplicateAirport_ReturnsExistsMessage()
        {
            //Проверка бизнес-правил
            var manager = new AirportManager();
            manager.ProcessEverything("addAirport", "SVOO", "Sheremetyevo", 0, 0);
            string result = manager.ProcessEverything("addAirport", "SVOO", "Sheremetyevo", 0, 0);
            Assert.Equal("Airport already exists", result);
        }

        [Fact]
        public void FlightService_AddFlight_CallsManagerWithCorrectParameters()
        {
            //Создаю мок интерфейса
            var mockManager = new Mock<IAirportManager>();

            //Метод должен быть вызван 1 раз с конкретными параметрами
            mockManager.Setup(m => m.ProcessEverything("createFlight", "SU100", "Istanbul", 180, 0))
                       .Returns("Flight created successfully")
                       .Verifiable();

            // Внедряю мок в тестируемый класс
            var flightService = new FlightService(mockManager.Object);
            string result = flightService.AddFlight("SU100", "Istanbul", 180);
            mockManager.Verify(m => m.ProcessEverything("createFlight", "SU100", "Istanbul", 180, 0), Times.Once);
            Assert.Equal("Flight created successfully", result);
        }

        [Fact]
        public void BookingSystem_CreateBooking_ValidData_ReturnsTrue()
        {
            //Интеграционный тест на уровне модуля
            var bookingSystem = new BookingSystem();
            bool result = bookingSystem.CreateBooking(
                "Иван Иванов", "4510123456", "SU100", "Economy", 2, 25.5,
                false, "ivan@test.com", "+79991234567"
            );

            Assert.True(result);
        }
    }
}