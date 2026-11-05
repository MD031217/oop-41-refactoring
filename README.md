# Система управления аэропортом (OOP 4.1)

> **Лабораторная работа по дисциплине "Рефакторинг"**  
> **Студент:** Долматова Мария Алексеевна
> **Группа:** ПИ41б
> **Дата:** Ноябрь 2026

## Описание проекта

Консольное приложение, моделирующее работу аэропорта: управление рейсами, пассажирами, багажом, экипажем и проверкой безопасности.

**Цель работы:** провести обратное проектирование, обнаружить недостатки кода (code smells) и выполнить их рефакторинг.

## Структура проекта

## Найденные недостатки кода

В ходе анализа обнаружено **12+ недостатков** различных типов. Каждый задокументирован в отдельном Issue.

| № | Тип недостатка | Примеры в коде | Issue |
|---|---------------|---------------|-------|
| 1 | **Параллельные иерархии наследования** | `Vehicle`/`VehicleValidator` | [#1](https://github.com/MD031217/oop-41-refactoring/issues/1) |
| 2 | **Большой класс + Длинный метод** | `AirportManager`, `ProcessEverything()` | [#2](https://github.com/MD031217/oop-41-refactoring/issues/2) |
| 3 | **Одержимость примитивами + Магические константы** | `double` вместо `Money`, числа 23.0, 50.0 | [#3](https://github.com/MD031217/oop-41-refactoring/issues/3) |
| 4 | **Повторяющийся код** | Дублирование валидации в 5+ методах | [#4](https://github.com/MD031217/oop-41-refactoring/issues/4) |
| 5 | **Расходящиеся модификации** | Класс `Flight` | [#5](https://github.com/MD031217/oop-41-refactoring/issues/5) |
| 6 | **Завистливые функции** | `FlightReporter.GenerateDetailedReport()` | [#6](https://github.com/MD031217/oop-41-refactoring/issues/6) |
| 7 | **Группы данных** | `BookingSystem` (9 параметров в каждом методе) | [#7](https://github.com/MD031217/oop-41-refactoring/issues/7) |
| 8 | **Посредник** | `FlightService` | [#8](https://github.com/MD031217/oop-41-refactoring/issues/8) |
| 9 | **Прочие** | Мертвый код, пустые catch, плохие имена | [#9](https://github.com/MD031217/oop-41-refactoring/issues/9) |

**Итого за анализ:** ~5.25 баллов

## Диаграммы классов

Диаграмма "БЫЛО" (до рефакторинга) доступна в папке `docs/`:
- [PlantUML исходник](docs/DIAGRAMS.md)
- [PNG изображение](docs/diagram_before.png)

---

## Инструкция по запуску

### Требования
- **Visual Studio 2022** (17.0+) или **VS Code** + .NET SDK 8.0
- **.NET 8.0 SDK**

### Запуск
```bash
# 1. Клонировать репозиторий
git clone https://github.com/MD031217/oop-41-refactoring.git
cd oop-41-refactoring

# 2. Открыть решение в Visual Studio
start src/Legacy/AirportSystem/AirportSystem.sln

# 3. Запустить (F5 или Ctrl+F5)
```
