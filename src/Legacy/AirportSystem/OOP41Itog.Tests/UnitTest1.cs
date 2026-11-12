using System;
using Xunit;
using Moq;
using OOP41Itog.Legacy;

namespace OOP41Itog.Tests
{
    public class CheckedLuggageMagicNumbersTests
    {
        // ТЕСТ 1: Покрывает магическое число 23.0 (лимит веса)
        // Проблема: в коде написано "if (Weight > 23.0)" - непонятно, откуда 23.0
        [Fact]
        public void CalculateFee_MagicNumber_23kgLimit_NoSurcharge()
        {
            var luggage = new CheckedLuggage
            {
                Weight = 23.0,
                IsFragile = false,
                NeedsSpecialHandling = false,
                HandlingFee = 0.0
            };
            double fee = luggage.CalculateFee();
            Assert.Equal(0.0, fee);
        }

        // ТЕСТ 2: Покрывает магическое число 10.0 (ставка за перевес)
        // Проблема: в коде написано "(Weight - 23.0) * 10.0" - непонятно, откуда 10.0
        [Fact]
        public void CalculateFee_MagicNumber_10perKg_OverweightSurcharge()
        {
            var luggage = new CheckedLuggage
            {
                Weight = 24.0,
                IsFragile = false,
                NeedsSpecialHandling = false,
                HandlingFee = 0.0
            };
            double fee = luggage.CalculateFee();
            Assert.Equal(10.0, fee, precision: 2);
        }

        // ТЕСТ 3: Покрывает магическое число 50.0 (доплата за хрупкость)
        // Проблема: в коде написано "if (IsFragile) fee += 50.0" - непонятно, откуда 50.0
        [Fact]
        public void CalculateFee_MagicNumber_50fragile_HandlingFee()
        {
            var luggage = new CheckedLuggage
            {
                Weight = 20.0,
                IsFragile = true,
                NeedsSpecialHandling = false,
                HandlingFee = 0.0
            };
            double fee = luggage.CalculateFee();
            Assert.Equal(50.0, fee, precision: 2);
        }

        // ТЕСТ 4: Покрывает магическое число 100.0 (доплата за спецобработку)
        // Проблема: в коде написано "if (NeedsSpecialHandling) fee += 100.0" - непонятно, откуда 100.0
        [Fact]
        public void CalculateFee_MagicNumber_100special_HandlingFee()
        {
            var luggage = new CheckedLuggage
            {
                Weight = 20.0,
                IsFragile = false,
                NeedsSpecialHandling = true,
                HandlingFee = 0.0
            };
            double fee = luggage.CalculateFee();
            Assert.Equal(100.0, fee, precision: 2);
        }

        // ТЕСТ 5: Покрывает комбинацию всех магических чисел
        // Проблема: все 4 магических числа (23.0, 10.0, 50.0, 100.0) работают вместе
        [Fact]
        public void CalculateFee_AllMagicNumbers_CombinedFees()
        {
            var luggage = new CheckedLuggage
            {
                Weight = 25.0,
                IsFragile = true,
                NeedsSpecialHandling = true,
                HandlingFee = 0.0
            };
            double fee = luggage.CalculateFee();
            Assert.Equal(170.0, fee, precision: 2);
        }

        // ТЕСТ 6: Покрывает граничное значение магического числа 23.0
        // Проблема: на границе магического числа часто бывают баги (> vs >=)
        [Fact]
        public void CalculateFee_BoundaryMagicNumber_23kg_SmallSurcharge()
        {
            var luggage = new CheckedLuggage
            {
                Weight = 23.1,
                IsFragile = false,
                NeedsSpecialHandling = false,
                HandlingFee = 0.0
            };
            double fee = luggage.CalculateFee();
            Assert.Equal(1.0, fee, precision: 2);
        }

        // ТЕСТ 7: Покрывает магическое число в HandlingFee
        // Проблема: базовый сбор тоже может быть магическим числом
        [Fact]
        public void CalculateFee_MagicNumber_HandlingFee_Included()
        {
            var luggage = new CheckedLuggage
            {
                Weight = 20.0,
                IsFragile = false,
                NeedsSpecialHandling = false,
                HandlingFee = 25.0
            };
            double fee = luggage.CalculateFee();
            Assert.Equal(25.0, fee, precision: 2);
        }

        // ТЕСТ 8: Покрывает отсутствие магических чисел при стандартном весе
        // Проблема: даже когда доплаты нет, магические числа всё равно присутствуют в коде
        [Fact]
        public void CalculateFee_NoMagicNumbersTriggered_StandardWeight()
        {
            var luggage = new CheckedLuggage
            {
                Weight = 15.0,
                IsFragile = false,
                NeedsSpecialHandling = false,
                HandlingFee = 0.0
            };
            double fee = luggage.CalculateFee();
            Assert.Equal(0.0, fee);
        }
    }

    public class AirportManagerGodObjectTests
    {
        // ТЕСТ 9: Покрывает ответственность за аэропорты в God Object
        // Проблема: AirportManager управляет аэропортами, хотя должен быть отдельный класс
        [Fact]
        public void ProcessEverything_GodObject_AddAirportResponsibility()
        {
            var manager = new AirportManager();
            string result = manager.ProcessEverything("addAirport", "SVOO", "Sheremetyevo", 0, 0);
            Assert.Equal("Airport added: SVOO", result);
            Assert.Contains("SVOO", manager.airports);
        }

        // ТЕСТ 10: Покрывает ответственность за валидацию в God Object
        // Проблема: валидация размазана по всему большому классу
        [Fact]
        public void ProcessEverything_GodObject_ValidationResponsibility()
        {
            var manager = new AirportManager();
            string result = manager.ProcessEverything("addAirport", "", "Sheremetyevo", 0, 0);
            Assert.Equal("Invalid airport name", result);
            Assert.Empty(manager.airports);
        }

        // ТЕСТ 11: Покрывает ответственность за рейсы в God Object
        // Проблема: AirportManager управляет рейсами, хотя это отдельная сущность
        [Fact]
        public void ProcessEverything_GodObject_CreateFlightResponsibility()
        {
            var manager = new AirportManager();
            string result = manager.ProcessEverything("createFlight", "SU100", "Istanbul", 180, 0);
            Assert.Contains("Flight created:", result);
            Assert.Contains("SU100-Istanbul-180", manager.flights);
            Assert.Equal(1, manager.totalFlights);
        }

        // ТЕСТ 12: Покрывает ответственность за пассажиров в God Object
        // Проблема: AirportManager управляет пассажирами, хотя должен быть отдельный класс
        [Fact]
        public void ProcessEverything_GodObject_AddPassengerResponsibility()
        {
            var manager = new AirportManager();
            string result = manager.ProcessEverything("addPassenger", "Ivan Ivanov", "4510123456", 1, 0);
            Assert.Contains("Passenger added:", result);
            Assert.Single(manager.passengers);
            Assert.Equal(1, manager.totalPassengers);
        }

        // ТЕСТ 13: Покрывает ответственность за багаж в God Object
        // Проблема: AirportManager управляет багажом, хотя это отдельная сущность
        [Fact]
        public void ProcessEverything_GodObject_ProcessLuggageResponsibility()
        {
            var manager = new AirportManager();
            string result = manager.ProcessEverything("processLuggage", "Passenger1", "standard", 0, 25.0);
            Assert.Contains("Luggage processed:", result);
            Assert.Single(manager.luggage);
        }

        // ТЕСТ 14: Покрывает ответственность за безопасность в God Object
        // Проблема: AirportManager проверяет безопасность, хотя это отдельная задача
        [Fact]
        public void ProcessEverything_GodObject_SecurityCheckResponsibility()
        {
            var manager = new AirportManager();
            manager.ProcessEverything("addPassenger", "Ivan Ivanov", "4510123456", 1, 0);
            string result = manager.ProcessEverything("securityCheck", "", "", 0, 0);
            Assert.Equal("Security check passed", result);
        }

        // ТЕСТ 15: Покрывает ответственность за отчёты в God Object
        // Проблема: AirportManager генерирует отчёты, смешивая UI и бизнес-логику
        [Fact]
        public void ProcessEverything_GodObject_GenerateReportResponsibility()
        {
            var manager = new AirportManager();
            manager.ProcessEverything("addAirport", "SVOO", "Sheremetyevo", 0, 0);
            string result = manager.ProcessEverything("generateReport", "", "", 0, 0);
            Assert.Equal("Report generated", result);
        }

        // ТЕСТ 16: Покрывает проверку взаимодействия через Moq
        // Проблема: God Object сложно тестировать, поэтому использую интерфейс
        [Fact]
        public void FlightService_MockGodObject_IsolatedTesting()
        {
            var mockManager = new Mock<IAirportManager>();
            mockManager.Setup(m => m.ProcessEverything("createFlight", "SU100", "Istanbul", 180, 0))
                       .Returns("Flight created successfully")
                       .Verifiable();
            var flightService = new FlightService(mockManager.Object);
            string result = flightService.AddFlight("SU100", "Istanbul", 180);
            mockManager.Verify(m => m.ProcessEverything("createFlight", "SU100", "Istanbul", 180, 0), Times.Once);
            Assert.Equal("Flight created successfully", result);
        }
    }
}