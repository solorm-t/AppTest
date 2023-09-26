[![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=KarelianGhost_AppTest)](https://sonarcloud.io/summary/new_code?id=KarelianGhost_AppTest)
# AppTest
Игра-бродилка, в которой игроку предстоит пройти через процедурно генерируемый лабиринт за определённое число ходов, забрать на своём пути сокровище и добраться до выхода.

## Описание функций программы
### public void GameInit(GameData data)
Данная функция инициализирует состояние игры, заполняя объект данных необходимыми данными для начала.
### public bool SetMapValue(Map map, MapLocation pos, int value)
Данная функция заполняет указанную ячейку карты указанным значением. Возвращает True, если данные были записаны в ячейку и False, если была совершена попытка записать данные за пределы карты.
### public int GetMapValue(Map map, MapLocation pos)
Данная функция получает значение с указанной ячейки карты. Возвращает значение ячейки карты. В случае, если была совершена попытка получить данные за пределами карты возвращает -1.
### public void MovePlayer(Direction dir, GameData data)
Данная функция осуществляет перемещение игрока на основе полученного направления.
### public bool ChangePlayerPosition(MapLocation targetPos, GameData data)
Данная функция осуществляет попытку перемещения игрока в указанную ячейку. Возвращает True, если перемещение было совершено и False, если переместиться в ячейку не удалось.
### public void SetExitPosition(MapLocation targetPos, GameData data)
Данная функция устанавливает позицию выхода из лабиринта.
### public void SetRandomTreasurePosition(GameData data)
Данная функция осуществляет попытки установить сокровище в случайную свободную ячейку лабиринта.
### public void CheckTreasure(GameData data)
Данная функция проверяет, находится ли сокровище в той же ячейке, что и игрок.
### public void CheckExit(GameData data)
Данная фукнция проверяет, находится ли выход в той же ячейке, что и игрок.
### public void GenerateMaze(GameData data)
Данная функция осуществляет процедурную генерацию случайного лабиринта алгоритмом Эллера.
### public int CalculateShortestDistanceBetween(MapLocation start, MapLocation target)
Данная функция расчитывает длину кратчайшего пути между двумя точками. Возвращает длину пути в ячейках.
### public void BeginSearch(MapLocation start, MapLocation goal, GameData data)
Данная функция осуществляет необходимую подготовку перед началом поиска пути.
### public void Search(Node thisNode, GameData data)
Данная функция осуществляет одну итерацию поиска кратчайшего пути алгоритмом A*
### public bool isClosed(MapLocation pos, GameData data)
Данная функция проверяет, находится ли указанная ячейка в списке закрытых ячеек поиска пути. Возвращает True, если в списке закрытых есть такая ячейка и False, если такой ячейки нет.
### public bool UpdateMarker(MapLocation pos, float g, float h, float f, Node prt, GameData data)
Данная функция осуществляет обновление данных ячейки из списка открытых. Возвращает True, если данные были обновлены и False, если данные не были обновлены.
### public List\<Node> GetPath(MapLocation start, MapLocation target, GameData data)
Данная функция осуществляет поиск кратчайшего пути алгоритмом A*. Возвращает список вершин пути.
### public void GenerateLevel(GameData data)
Данная функция осуществляет генерацию игрового уровня
### public void SetMapSize(GameData data, int level)
Данная функция выставляет размер игрового поля
### public bool WithdrawMove(GameData data)
Данная функция осуществляет списание ходов. Возвращает True, если расходуются бонусные ходы и False, если расходуются основные ходы.
### public void SetRandomExitPosition(GameData data)
Данная функция осуществляет размещение перехода на следующий уровень в случайной точке на игровом поле
### public void SetRandomBonusPosition(GameData data, int amount)
Данная функция осуществляет размещение бонусов в случайных точках на игровом поле
### public void CheckBonus(GameData data)
Данная функция проверяет, находится ли игрок в той же ячейке, что и бонус и начисляет бонусные ходы
### public bool CheckGameOver(GameData data)
Данная функция проверяет, закончились ли основные и бонусные ходы


## Аттестационное тестирование

Тест 1:
Начальное состояние: игрок запустил игру
Действие: пользователь нажимает клавиши влево, вправо, вниз, вверх
Ожидаемый результат: перемещение игрока
Тест проверяет возможность перемещение игрока

Тест 2:
Начальное состояние: игрок запустил игру
Действие: пользователь нажимает клавиши влево, вправо, вниз, вверх
Ожидаемый результат: уменьшение числа ходов при перемещениях
Тест проверяет уменьшение числа ходов

Тест 3:
Начальное состояние: игрок запустил игру
Действие: пользователь перемещается в ячейку с сокровищем
Ожидаемый результат: сокровище исчезает
Тест проверяет подбор сокровища

Тест 4:
Начальное состояние: игрок запустил игру
Действие: пользователь нажимает клавиши влево, вправо, вниз, вверх при условии что там стенки
Ожидаемый результат: игрок не перемещается
Тест проверяет невозможность перемещение игрока туда, где есть стенка

Тест 5:
Начальное состояние: игрок запустил игру
Действие: пользователь нажимает клавиши влево, вправо, вниз, вверх и перемещается на ячейку выхода за отведённое число ходов
Ожидаемый результат: Сообщение о победе
Тест проверяет работу программы при прохождении игры

## Модульное тестирование

### Тест GetMapValueFilled(Позитивный)[Вячеслав]:
Описание: Получение значения ячейки карты, если поле было заполнено ранее
Функция: public int GetMapValue(Map map, MapLocation pos)
Входные данные: game.data.map - карта для проверки, game.data.map.gridArray[1, 2] - значение установленной ячейки(1,2), new MapLocation(1, 2) - ссылка на поле карты(1,2)
Ожидаемый результат: Значение равно ячейке (1,2)

### Тест GetMapValueEmpty(Позитивный)[Вячеслав]:
Описание: Получение значения ячейки карты, если поле не было заполнено ранее
Функция: public int GetMapValue(Map map, MapLocation pos)
Входные данные: game.data.map - карта для проверки, game.data.map.gridArray[1, 2] - значение установленной ячейки(1,2), new MapLocation(1, 1) - ссылка на поле карты(1,1)
Ожидаемый результат: Значения равно 0

### Тест GetMapValueOutsideBounds(Негативный)[Вячеслав]:
Описание: Получение значения ячейки карты, если поле оказалось за пределами карты
Функция: public int GetMapValue(Map map, MapLocation pos)
Входные данные: game.data.map - карта для проверки, game.data.map.gridArray[1, 2] - значение установленной ячейки(1,2), new MapLocation(100, 100) - ссылка на поле карты(100,100)
Ожидаемый результат: Значение равно -1

### Тест SetMapValueInsideBounds(Позитивный)[Вячеслав]:
Описание: Установка значения ячейки карты, если поле оказалось в пределах карты
Функция: public bool SetMapValue(Map map, MapLocation pos, int value)
Входные данные: game.data.map - карта для проверки, game.data.map.gridArray[1, 2] - значение установленной ячейки(1,2), new MapLocation(1, 2) - ссылка на поле карты(1,2)
Ожидаемый результат: Значение в ячейке (1,2) равно 2, функция вернула значение True

### Тест SetMapValueOutsideBounds(Негативный)[Вячеслав]:
Описание: Установка значения ячейки карты, если поле оказалось за пределами карты
Функция: public bool SetMapValue(Map map, MapLocation pos, int value)
Входные данные: game.data.map - карта для проверки, game.data.map.gridArray[1, 2] - значение установленной ячейки(1,2), new MapLocation(100, 100) - ссылка на поле карты(100,100)
Ожидаемый результат: Функция вернула значение False

### Тест ClosedNodeExists(Позитивный)[Дмитрий]:
Описание: Проверка наличия узла в списке закрытых, если узел существует
Функция: public bool isClosed(MapLocation pos, GameData data)
Входные данные: game.data.map - карта для проверки, game.data.closed - ожидаемый список закрытых узлов
Ожидаемый результат: Функция вернула значение True

### Тест ClosedNodeNotExists(Негативный)[Дмитрий]:
Описание: Проверка наличия узла в списке закрытых, если узел не существует
Функция: public bool isClosed(MapLocation pos, GameData data)
Входные данные: game.data.map - карта для проверки, game.data.closed - ожидаемый список закрытых узлов
Ожидаемый результат: Функция вернула значение False

### Тест WithdrawBonusTurn(Позитивный)[Вячеслав]:
Описание: Проверка списания бонусных ходов, в случае, если основные ходы закончились
Функция: public bool WithdrawMove(GameData data)
Входные данные: game.data.turnAmount - число основных ходов, game.data.turnAmount - число бонусных ходов
Ожидаемый результат: Функция вернула значение True

### Тест WithdrawBaseTurn(Негативный)[Вячеслав]:
Описание: Проверка списания основных ходов
Функция: public bool WithdrawMove(GameData data)
Входные данные: game.data.turnAmount - число основных ходов, game.data.turnAmount - число бонусных ходов
Ожидаемый результат: Функция вернула значение False

### Тест SetMapSize1(Позитивный)[Вячеслав]:
Описание: Проверка выставления размера карты
Функция: public void SetMapSize(GameData data, int level)
Входные данные: game.data.level - уровень карты
Ожидаемый результат: Значение размера карты в ширину и в длину равно 7 + Уровень * 2

### Тест SetMapSize2(Позитивный)[Вячеслав]:
Описание: Проверка выставления размера карты
Функция: public void SetMapSize(GameData data, int level)
Входные данные: game.data.level - уровень карты
Ожидаемый результат: Значение размера карты в ширину и в длину равно 7 + Уровень * 2

### Тест NotGameOverBoth(Негативный)[Вячеслав]:
Описание: Проверка на конец игры, в случае, если бонусные и основные ходы не закончились
Функция: public bool CheckGameOver(GameData data)
Входные данные: game.data.turnAmount - число основных ходов, game.data.turnAmount - число бонусных ходов
Ожидаемый результат: Функция вернула значение False

### Тест NotGameOverBonus(Негативный)[Вячеслав]:
Описание: Проверка на конец игры, в случае, если бонусные ходы не закончились
Функция: public bool CheckGameOver(GameData data)
Входные данные: game.data.turnAmount - число основных ходов, game.data.turnAmount - число бонусных ходов
Ожидаемый результат: Функция вернула значение False

### Тест NotGameOverBase(Негативный)[Вячеслав]:
Описание: Проверка на конец игры, в случае, если основные ходы не закончились
Функция: public bool CheckGameOver(GameData data)
Входные данные: game.data.turnAmount - число основных ходов, game.data.turnAmount - число бонусных ходов
Ожидаемый результат: Функция вернула значение False

### Тест GameOver(Позитивный)[Вячеслав]:
Описание: Проверка на конец игры, в случае, если ходы закончились
Функция: public bool CheckGameOver(GameData data)
Входные данные: game.data.turnAmount - число основных ходов, game.data.turnAmount - число бонусных ходов
Ожидаемый результат: Функция вернула значение True

## Интеграционное тестирование

### Тест GetPathAny(Позитивный)[Вячеслав]:
Описание: Получение пути до указанной ячейки карты
Функция: public List<Node> GetPath(MapLocation start, MapLocation target, GameData data)
Входные данные: game.data.map - карта для проверки, new MapLocation(0, 1) - стартовая позиция на поле карты(0,1), new MapLocation(0, 2) - целевая позиция на поле карты(0,2)
Ожидаемый результат: Возвращённый список узлов совпадает ожидаемому 

### Тест GetPathShortest(Позитивный)[Вячеслав]:
Описание: Получение кратчайшего пути до указанной ячейки карты
Функция: public List<Node> GetPath(MapLocation start, MapLocation target, GameData data)
Входные данные: game.data.map - карта для проверки, new MapLocation(0, 1) - стартовая позиция на поле карты(0,1), new MapLocation(4, 1) - целевая позиция на поле карты(4,1)
Ожидаемый результат: Возвращённый список узлов совпадает ожидаемому 

### Тест CalculateShortestDistanceBetween1(Позитивный)[Вячеслав]:
Описание: Получение длины кратчайшего пути между двумя ячейками карты
Функция: public int CalculateShortestDistanceBetween(MapLocation start, MapLocation target)
Входные данные: game.data.map - карта для проверки, new MapLocation(0, 0) - стартовая позиция на поле карты(0,0), new MapLocation(0, 2) - целевая позиция на поле карты(0,2)
Ожидаемый результат: Возвращённое значение равно 6

### Тест CalculateShortestDistanceBetween2(Позитивный)[Вячеслав]:
Описание: Получение длины кратчайшего пути между двумя ячейками карты
Функция: public int CalculateShortestDistanceBetween(MapLocation start, MapLocation target)
Входные данные: game.data.map - карта для проверки, new MapLocation(0, 0) - стартовая позиция на поле карты(0,0), new MapLocation(4, 4) - целевая позиция на поле карты(4,4)
Ожидаемый результат: Возвращённое значение равно 16

### Тест MovePlayerNone(Позитивный)[Вячеслав]:
Описание: Передвижение игрока при не заданном направлении
Функция: public void MovePlayer(Direction dir, GameData data)
Входные данные: game.data.map - карта для проверки, new MapLocation(1, 1) - стартовая позиция игрока на поле карты(1,1), new MapLocation(1, 1) - целевая позиция на поле карты(1,1)
Ожидаемый результат: Значение поля карты в целевой позиции принимает значение CellType.player. game.data.playerPos принимает значение целевой позиции на поле карты

### Тест MovePlayerLeft(Позитивный)[Вячеслав]:
Описание: Передвижение игрока при заданном направлении влево
Функция: public void MovePlayer(Direction dir, GameData data)
Входные данные: game.data.map - карта для проверки, new MapLocation(1, 1) - стартовая позиция игрока на поле карты(1,1), new MapLocation(0, 1) - целевая позиция на поле карты(0,1)
Ожидаемый результат: Значение поля карты в целевой позиции принимает значение CellType.player. game.data.playerPos принимает значение целевой позиции на поле карты

### Тест MovePlayerRight(Позитивный)[Вячеслав]:
Описание: Передвижение игрока при заданном направлении вправо
Функция: public void MovePlayer(Direction dir, GameData data)
Входные данные: game.data.map - карта для проверки, new MapLocation(1, 1) - стартовая позиция игрока на поле карты(1,1), new MapLocation(2, 1) - целевая позиция на поле карты(2,1)
Ожидаемый результат: Значение поля карты в целевой позиции принимает значение CellType.player. game.data.playerPos принимает значение целевой позиции на поле карты

### Тест MovePlayerUp(Позитивный)[Вячеслав]:
Описание: Передвижение игрока при заданном направлении вверх
Функция: public void MovePlayer(Direction dir, GameData data)
Входные данные: game.data.map - карта для проверки, new MapLocation(1, 1) - стартовая позиция игрока на поле карты(1,1), new MapLocation(1, 0) - целевая позиция на поле карты(1,0)
Ожидаемый результат: Значение поля карты в целевой позиции принимает значение CellType.player. game.data.playerPos принимает значение целевой позиции на поле карты

### Тест MovePlayerDown(Позитивный)[Вячеслав]:
Описание: Передвижение игрока при заданном направлении вниз
Функция: public void MovePlayer(Direction dir, GameData data)
Входные данные: game.data.map - карта для проверки, new MapLocation(1, 1) - стартовая позиция игрока на поле карты(1,1), new MapLocation(1, 2) - целевая позиция на поле карты(1,2)
Ожидаемый результат: Значение поля карты в целевой позиции принимает значение CellType.player. game.data.playerPos принимает значение целевой позиции на поле карты

### Тест ChangePlayerPositionToEmptyCell(Позитивный)[Дмитрий]:
Описание: Смена позиции игрока при попытке перейти в пустую ячейку
Функция: public bool ChangePlayerPosition(MapLocation targetPos, GameData data)
Входные данные: game.data.map - карта для проверки, game.data.map.gridArray[1, 1] - значение установленной ячейки(1,1), game.data.playerPos - значение позиции игрока, int oldPosCell - позиция поля карты до перемещения, int newPosCell - позиция поля карты после перемещения, game.data.turnAmount - число ходов.
Ожидаемый результат: oldPosCell принимает значение CellType.empty, newPosCell принимает значение CellType.player, функция вернула значение True, game.data.turnAmount принимает значение game.data.turnAmount - 1

### Тест ChangePlayerPositionToWallCell(Негативный)[Дмитрий]:
Описание: Смена позиции игрока при попытке перейти в ячейку со стеной
Функция: public bool ChangePlayerPosition(MapLocation targetPos, GameData data)
Входные данные: game.data.map - карта для проверки, game.data.map.gridArray[1, 1] - значение установленной ячейки(1,1), game.data.map.gridArray[1, 0] - значение установленной ячейки(1,0), game.data.playerPos - значение позиции игрока, int oldPosCell - позиция поля карты до перемещения, int newPosCell - позиция поля карты после перемещения, game.data.turnAmount - число ходов.
Ожидаемый результат: oldPosCell принимает значение CellType.player, newPosCell принимает значение CellType.wall, функция вернула значение False, game.data.turnAmount принимает значение game.data.turnAmount

### Тест ChangePlayerPositionFromExitCell(Позитивный)[Дмитрий]:
Описание: Смена позиции игрока при попытке перейти из ячейки с выходом
Функция: public bool ChangePlayerPosition(MapLocation targetPos, GameData data)
Входные данные: game.data.map - карта для проверки, game.data.map.gridArray[1, 1] - значение установленной ячейки(1,1), game.data.map.gridArray[1, 0] - значение установленной ячейки(1,0), game.data.playerPos - значение позиции игрока, int oldPosCell - позиция поля карты до перемещения, int newPosCell - позиция поля карты после перемещения, game.data.turnAmount - число ходов.
Ожидаемый результат: oldPosCell принимает значение CellType.exit, newPosCell принимает значение CellType.player, функция вернула значение False, game.data.turnAmount принимает значение game.data.turnAmount

### Тест SearchIteration(Позитивный)[Дмитрий]:
Описание: Итерация поиска пути
Функция: public void Search(Node thisNode, GameData data)
Входные данные: game.data.map - карта для проверки, game.data.map.gridArray[1, 1] - значение установленной ячейки(1,1), game.data.map.gridArray[0, 1] - значение установленной ячейки(0,1), game.data.startNode - стартовый узел пути, game.data.goalNode - конечный узел пути
Ожидаемый результат: Списки открытых и закрытых узлов совпадают с ожидаемыми

### Тест CheckTreasurePlacement(Позитивный)[Дмитрий]:
Описание: Установка сокровища в случайную точку на карте
Функция: public void SetRandomTreasurePosition(GameData data)
Входные данные: game.data.map - карта для проверки
Ожидаемый результат: Одна из клеток поля карты принимает значение CellType.treasure

### Тест CheckExitPlacement(Позитивный)[Дмитрий]:
Описание: Установка выхода в случайную точку на карте
Функция: public void SetRandomExitPosition(GameData data)
Входные данные: game.data.map - карта для проверки
Ожидаемый результат: Одна из клеток поля карты принимает значение CellType.exit

### Тест CheckBonusPlacement1(Позитивный)[Дмитрий]:
Описание: Установка бонуса в случайную точку на карте
Функция: public void SetRandomBonusPosition(GameData data, int amount)
Входные данные: game.data.map - карта для проверки, int amount - количество бонусов
Ожидаемый результат: Одна из клеток поля карты принимает значение CellType.bonus

### Тест CheckBonusPlacement2(Позитивный)[Дмитрий]:
Описание: Установка бонуса в случайную точку на карте
Функция: public void SetRandomBonusPosition(GameData data, int amount)
Входные данные: game.data.map - карта для проверки, int amount - количество бонусов
Ожидаемый результат: Одна из клеток поля карты принимает значение CellType.bonus

### Тест CheckPlayerNotOnExit(Негативный)[Дмитрий]:
Описание: Проверка выхода в случае, если игрок не находится в точке выхода
Функция: public void CheckExit(GameData data)
Входные данные: game.data.map - карта для проверки, new MapLocation(0, 0) - стартовая позиция игрока на поле карты(0,0), new MapLocation(1, 0) - позиция выхода на поле карты(1,0)
Ожидаемый результат: Функция вернула значение False

### Тест CheckTreasureNotCollected(Негативный)[Дмитрий]:
Описание: Проверка выхода в случае, если игрок находится в точке выхода и сокровище не собрано
Функция: public void CheckExit(GameData data)
Входные данные: game.data.isTreasureCollected - подобрано ли сокровище, game.data.map - карта для проверки, new MapLocation(0, 0) - стартовая позиция игрока на поле карты(0,0), new MapLocation(0, 0) - позиция выхода на поле карты(0,0)
Ожидаемый результат: Функция вернула значение False
  
### Тест CheckTreasureCollected(Позитивный)[Дмитрий]:
Описание: Проверка выхода в случае, если игрок находится в точке выхода и сокровище собрано
Функция: public void CheckExit(GameData data)
Входные данные: game.data.isTreasureCollected - подобрано ли сокровище, game.data.map - карта для проверки, new MapLocation(0, 0) - стартовая позиция игрока на поле карты(0,0), new MapLocation(0, 0) - позиция выхода на поле карты(0,0)
Ожидаемый результат: Функция вернула значение True
