using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace OOP41Itog.Legacy
{
    public interface IAirportManager
    {
        string ProcessEverything(string operation, string param1, string param2, int param3, double param4);
    }

    // НЕДОСТАТОК 1: God Object (класс который делает всё)
    // НЕДОСТАТОК 2: Нарушение инкапсуляции (public поля)
    // НЕДОСТАТОК 3: Одержимость примитивами
    public class AirportManager : IAirportManager
    {
        public List<string> airports;
        public List<string> airlines;
        public List<string> flights;
        public List<string> passengers;
        public List<string> crew;
        public List<string> luggage;
        public int totalRevenue;
        public int totalPassengers;
        public int totalFlights;
        public bool isOperational;
        public string currentStatus;
        public DateTime lastUpdate;

        public AirportManager()
        {
            airports = new List<string>();
            airlines = new List<string>();
            flights = new List<string>();
            passengers = new List<string>();
            crew = new List<string>();
            luggage = new List<string>();
            totalRevenue = 0;
            totalPassengers = 0;
            totalFlights = 0;
            isOperational = true;
            currentStatus = "Active";
            lastUpdate = DateTime.Now;
        }

        // НЕДОСТАТОК 4: Длинный метод - 150+ строк
        // НЕДОСТАТОК 5: Условная сложность (вложенные if)
        // НЕДОСТАТОК 6: Магические константы
        // НЕДОСТАТОК 7: Повторяющийся код
        // НЕДОСТАТОК 8: Смешение ответственностей
        public string ProcessEverything(string operation, string param1, string param2, int param3, double param4)
        {
            string result = "";

            if (operation == "addAirport")
            {
                // Дублирование валидации
                if (param1 != null && param1 != "" && param1.Length > 0)
                {
                    if (param1.Length > 3)
                    {
                        if (!airports.Contains(param1))
                        {
                            airports.Add(param1);
                            result = "Airport added: " + param1;
                            totalFlights++; // Ошибка: должно быть airports.Count
                            lastUpdate = DateTime.Now;
                        }
                        else
                        {
                            result = "Airport already exists";
                        }
                    }
                    else
                    {
                        result = "Airport code too short";
                    }
                }
                else
                {
                    result = "Invalid airport name";
                }
            }
            else if (operation == "addAirline")
            {
                // Дублирование той же валидации
                if (param1 != null && param1 != "" && param1.Length > 0)
                {
                    if (param1.Length > 3)
                    {
                        if (!airlines.Contains(param1))
                        {
                            airlines.Add(param1);
                            result = "Airline added: " + param1;
                            lastUpdate = DateTime.Now;
                        }
                        else
                        {
                            result = "Airline already exists";
                        }
                    }
                    else
                    {
                        result = "Airline code too short";
                    }
                }
                else
                {
                    result = "Invalid airline name";
                }
            }
            else if (operation == "createFlight")
            {
                // НЕДОСТАТОК 9: Цепочки сообщений
                if (param1 != null && param2 != null && param3 > 0)
                {
                    string flightId = param1 + "-" + param2 + "-" + param3.ToString();
                    flights.Add(flightId);

                    // Магические константы
                    double basePrice = 150.50;
                    double taxRate = 0.18;
                    double fuelSurcharge = 45.75;
                    double serviceFee = 12.00;
                    double totalPrice = basePrice + (basePrice * taxRate) + fuelSurcharge + serviceFee;
                    totalRevenue += (int)totalPrice;
                    totalFlights++;
                    result = "Flight created: " + flightId + ", Price: " + totalPrice.ToString("F2");
                    lastUpdate = DateTime.Now;
                }
                else
                {
                    result = "Invalid flight parameters";
                }
            }
            else if (operation == "addPassenger")
            {
                if (param1 != null && param2 != null && param3 > 0)
                {
                    // НЕДОСТАТОК 10: Нечитаемый код
                    string p = param1 + "|" + param2 + "|" + param3.ToString();
                    passengers.Add(p);
                    totalPassengers++;

                    // Дублирование расчета цены
                    double basePrice = 150.50;
                    double taxRate = 0.18;
                    double fuelSurcharge = 45.75;
                    double serviceFee = 12.00;
                    double totalPrice = basePrice + (basePrice * taxRate) + fuelSurcharge + serviceFee;

                    // НЕДОСТАТОК 11: Временное поле
                    string tempTicket = DateTime.Now.Ticks.ToString();
                    result = "Passenger added: " + param1 + ", Ticket: " + tempTicket;
                    lastUpdate = DateTime.Now;
                }
                else
                {
                    result = "Invalid passenger data";
                }
            }
            else if (operation == "processLuggage")
            {
                if (param1 != null && param4 > 0)
                {
                    // НЕДОСТАТОК 12: Ленивый класс (вложенная логика)
                    bool isOverweight = param4 > 23.0;
                    bool isFragile = param2 == "fragile";
                    bool isSpecial = param2 == "special";
                    double fee = 0;
                    if (isOverweight)
                    {
                        fee += (param4 - 23.0) * 10.0; // Магическая константа
                    }
                    if (isFragile)
                    {
                        fee += 50.0; // Магическая константа
                    }
                    if (isSpecial)
                    {
                        fee += 100.0; // Магическая константа
                    }
                    string luggageId = param1 + "-" + luggage.Count.ToString();
                    luggage.Add(luggageId + ":" + param4.ToString() + ":" + fee.ToString());
                    result = "Luggage processed: " + luggageId + ", Fee: " + fee.ToString("F2");
                    lastUpdate = DateTime.Now;
                }
                else
                {
                    result = "Invalid luggage data";
                }
            }
            else if (operation == "securityCheck")
            {
                // НЕДОСТАТОК 13: Большой метод с вложенными циклами
                bool allClear = true;
                for (int i = 0; i < passengers.Count; i++)
                {
                    string passenger = passengers[i];
                    string[] parts = passenger.Split('|');
                    if (parts.Length >= 3)
                    {
                        // НЕДОСТАТОК 14: Неэффективный алгоритм
                        for (int j = 0; j < luggage.Count; j++)
                        {
                            string lugg = luggage[j];
                            if (lugg.StartsWith(parts[0]))
                            {
                                // Проверка безопасности (упрощенная)
                                if (parts[0].Contains("BAD"))
                                {
                                    allClear = false;
                                    result = "Security threat detected!";
                                    break;
                                }
                            }
                        }
                    }
                }
                if (allClear)
                {
                    result = "Security check passed";
                }
                lastUpdate = DateTime.Now;
            }
            else if (operation == "generateReport")
            {
                // НЕДОСТАТОК 15: Смешение UI и бизнес-логики
                Console.WriteLine("=== AIRPORT REPORT ===");
                Console.WriteLine("Total Airports: " + airports.Count);
                Console.WriteLine("Total Airlines: " + airlines.Count);
                Console.WriteLine("Total Flights: " + totalFlights);
                Console.WriteLine("Total Passengers: " + totalPassengers);
                Console.WriteLine("Total Revenue: " + totalRevenue);
                Console.WriteLine("Last Update: " + lastUpdate);
                Console.WriteLine("=====================");
                result = "Report generated";
            }
            else
            {
                result = "Unknown operation";
            }

            return result;
        }

        // НЕДОСТАТОК 16: Дублирование метода (почти идентичный)
        public string ProcessEverythingV2(string operation, string param1, string param2, int param3, double param4)
        {
            // Копия предыдущего метода с небольшими изменениями
            string result = "";
            if (operation == "addAirport")
            {
                if (param1 != null && param1 != "" && param1.Length > 0)
                {
                    if (param1.Length > 3)
                    {
                        if (!airports.Contains(param1))
                        {
                            airports.Add(param1);
                            result = "Airport added successfully: " + param1;
                            lastUpdate = DateTime.Now;
                        }
                        else
                        {
                            result = "Airport already exists in system";
                        }
                    }
                    else
                    {
                        result = "Airport code is too short";
                    }
                }
                else
                {
                    result = "Invalid airport name provided";
                }
            }
            //остальные операции скопированы
            return result;
        }

        // НЕДОСТАТОК 17: Некорректные наименования
        public void DoStuff(int x, string y, double z)
        {
            if (x > 0)
            {
                if (y != null)
                {
                    if (z > 0.0)
                    {
                        // Какая-то логика
                        Console.WriteLine("Processing: " + x + ", " + y + ", " + z);
                    }
                }
            }
        }

        // НЕДОСТАТОК 18: Мертвый код
        public void UnusedMethod()
        {
            int temp = 0;
            for (int i = 0; i < 100; i++)
            {
                temp += i;
            }
        }

        // НЕДОСТАТОК 19: Теоретическая общность (ненужная абстракция)
        public interface IProcessable
        {
            void Process();
            void Validate();
            void Execute();
            void Finalize();
            void Cleanup();
        }

        public class DummyProcessor : IProcessable
        {
            public void Process() { }
            public void Validate() { }
            public void Execute() { }
            public void Finalize() { }
            public void Cleanup() { }
        }

        // НЕДОСТАТОК 20: Неправильная обработка исключений
        public void RiskyOperation()
        {
            try
            {
                int result = 0;
            }
            catch (Exception)
            {
                // Пустой catch - проглатываем исключение
            }
        }
    }

    public enum LuggageType
    {
        HandLuggage,
        CheckedLuggage,
        SpecialLuggage
    }

    public abstract class Luggage
    {
        public string OwnerPassport { get; set; }
        public double Weight { get; set; }
        public LuggageType Type { get; set; }
        public string Description { get; set; }
        public bool IsSecurityRisk { get; set; }
    }

    public class HandLuggage : Luggage
    {
        private const double MAX_WEIGHT = 10.0;
        private const double MAX_SIZE = 55.0;
        public double Size { get; set; }
        public HandLuggage()
        {
            Type = LuggageType.HandLuggage;
        }
        public bool IsWithinLimits()
        {
            return Weight <= MAX_WEIGHT && Size <= MAX_SIZE;
        }
        public override string ToString()
        {
            return $"Ручная кладь: {Description}, Вес: {Weight}кг, Размер: {Size}см";
        }
    }

    public class CheckedLuggage : Luggage
    {
        public bool IsFragile { get; set; }
        public bool NeedsSpecialHandling { get; set; }
        public double HandlingFee { get; set; }
        public CheckedLuggage()
        {
            Type = LuggageType.CheckedLuggage;
        }
        public double CalculateFee()
        {
            double fee = HandlingFee;
            if (IsFragile) fee += 50.0;
            if (NeedsSpecialHandling) fee += 100.0;
            if (Weight > 23.0) fee += (Weight - 23.0) * 10.0;
            return fee;
        }

        public override string ToString()
        {
            return $"Регистрируемый багаж: {Description}, Вес: {Weight}кг, Хрупкий: {IsFragile}, Сбор: {CalculateFee()}руб";
        }
    }

    // НЕДОСТАТОК 21: Параллельные иерархии наследования
    public abstract class Vehicle
    {
        public string Model;
        public int Capacity;
    }

    public class PassengerVehicle : Vehicle
    {
        public int SeatCount;
    }

    public class CargoVehicle : Vehicle
    {
        public double MaxWeight;
    }

    public abstract class VehicleValidator
    {
        public abstract bool Validate(Vehicle vehicle);
    }

    public class PassengerVehicleValidator : VehicleValidator
    {
        public override bool Validate(Vehicle vehicle)
        {
            if (vehicle is PassengerVehicle pv)
            {
                return pv.SeatCount > 0;
            }
            return false;
        }
    }

    public class CargoVehicleValidator : VehicleValidator
    {
        public override bool Validate(Vehicle vehicle)
        {
            if (vehicle is CargoVehicle cv)
            {
                return cv.MaxWeight > 0;
            }
            return false;
        }
    }

    // НЕДОСТАТОК 22: Посредник
    public class FlightService
    {
        private readonly IAirportManager _manager;

        public FlightService(IAirportManager manager)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));
        }

        public string AddFlight(string flightNumber, string destination, int capacity)
        {
            return _manager.ProcessEverything("createFlight", flightNumber, destination, capacity, 0);
        }

        public string AddPassenger(string name, string passport, int flightId)
        {
            return _manager.ProcessEverything("addPassenger", name, passport, flightId, 0);
        }
    }

    // НЕДОСТАТОК 23: Расходящиеся модификации
    public class Flight
    {
        public string FlightNumber;
        public string Destination;
        public DateTime DepartureTime;
        public DateTime ArrivalTime;
        public int PassengerCount;
        public double TotalRevenue;
        public string Status;
        public bool IsDelayed;
        public string DelayReason;
        public int DelayMinutes;
        public string Gate;
        public string Terminal;
        public List<string> Crew;
        public List<string> Passengers;
        public void UpdateDepartureTime(DateTime newTime)
        {
            DepartureTime = newTime;
        }

        public void UpdateArrivalTime(DateTime newTime)
        {
            ArrivalTime = newTime;
        }

        public void SetStatus(string newStatus)
        {
            Status = newStatus;
        }

        public void SetDelay(bool isDelayed, string reason, int minutes)
        {
            IsDelayed = isDelayed;
            DelayReason = reason;
            DelayMinutes = minutes;
        }

        public void AddPassenger(string passenger)
        {
            if (Passengers == null) Passengers = new List<string>();
            Passengers.Add(passenger);
            PassengerCount++;
        }

        public void RemovePassenger(string passenger)
        {
            if (Passengers != null && Passengers.Contains(passenger))
            {
                Passengers.Remove(passenger);
                PassengerCount--;
            }
        }

        public void AddCrew(string crewMember)
        {
            if (Crew == null) Crew = new List<string>();
            Crew.Add(crewMember);
        }
    }

    // НЕДОСТАТОК 24: Завистливые функции
    public class FlightReporter
    {
        public string GenerateDetailedReport(Flight flight)
        {
            // Класс знает слишком много о внутренностях Flight
            string report = "";
            report += "Flight: " + flight.FlightNumber + "\n";
            report += "Destination: " + flight.Destination + "\n";
            report += "Departure: " + flight.DepartureTime + "\n";
            report += "Arrival: " + flight.ArrivalTime + "\n";
            report += "Passengers: " + flight.PassengerCount + "\n";
            report += "Revenue: " + flight.TotalRevenue + "\n";
            report += "Status: " + flight.Status + "\n";
            if (flight.IsDelayed)
            {
                report += "DELAYED: " + flight.DelayReason + " (" + flight.DelayMinutes + " min)\n";
            }
            report += "Gate: " + flight.Gate + "\n";
            report += "Terminal: " + flight.Terminal + "\n";
            if (flight.Passengers != null)
            {
                report += "Passenger List:\n";
                foreach (var p in flight.Passengers)
                {
                    report += "  - " + p + "\n";
                }
            }
            return report;
        }
    }

    // НЕДОСТАТОК 25: Группы данных
    public class BookingSystem
    {
        // Одна и та же группа параметров передается везде
        public bool CreateBooking(string passengerName, string passportNumber, string flightNumber,
                                   string seatClass, int luggageCount, double totalWeight,
                                   bool hasSpecialNeeds, string contactEmail, string contactPhone)
        {
            // Валидация
            if (string.IsNullOrEmpty(passengerName)) return false;
            if (string.IsNullOrEmpty(passportNumber)) return false;
            if (string.IsNullOrEmpty(flightNumber)) return false;
            Console.WriteLine("Booking created for: " + passengerName);
            return true;
        }

        public bool UpdateBooking(string passengerName, string passportNumber, string flightNumber,
                                   string seatClass, int luggageCount, double totalWeight,
                                   bool hasSpecialNeeds, string contactEmail, string contactPhone)
        {
            // Та же валидация
            if (string.IsNullOrEmpty(passengerName)) return false;
            if (string.IsNullOrEmpty(passportNumber)) return false;
            if (string.IsNullOrEmpty(flightNumber)) return false;
            Console.WriteLine("Booking updated for: " + passengerName);
            return true;
        }

        public bool CancelBooking(string passengerName, string passportNumber, string flightNumber,
                                   string seatClass, int luggageCount, double totalWeight,
                                   bool hasSpecialNeeds, string contactEmail, string contactPhone)
        {
            // Та же валидация
            if (string.IsNullOrEmpty(passengerName)) return false;
            if (string.IsNullOrEmpty(passportNumber)) return false;
            if (string.IsNullOrEmpty(flightNumber)) return false;
            Console.WriteLine("Booking cancelled for: " + passengerName);
            return true;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Система управления аэропортом\n");
            var manager = new AirportManager();
            manager.ProcessEverything("addAirport", "SVOO", "Sheremetyevo", 0, 0);
            manager.ProcessEverything("createFlight", "SU100", "Istanbul", 180, 0);
            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}