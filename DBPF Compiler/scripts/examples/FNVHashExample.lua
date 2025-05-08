local printTable = require("./printTable")

-- Всё, что есть в FNVHash
printTable(FNVHash)

-- Строка для тестов
local str = "Привет мир!"
-- Вычислить хеш
local h = FNVHash.Compute(str)
print(h)
-- Важно! В отличие от функции hash, FNVHash.Compute ВСЕГДА вычисляет хеш 
local h2 = hash(str) -- вернёт тоже самое
print(h2)
print(h == h2) -- true

-- Ещё пример
local animations = FNVHash.Compute("animations~")
print(FNVHash.ToString(animations)) -- строка в формате 0xXXXXXXXX
local animations2 = hash("animations~") -- вернёт 0
print(FNVHash.ToString(animations2))
print(animations == animations2) -- false

-- Парсим хеш
print(FNVHash.Parse("0x12345678"))
-- Если мы не уверены, что входящая строка будет корректной
local ok, result = FNVHash.TryParse("0xA85F45DB")
if ok then
	print(result)
else
	print("Error")
end

ok, result = FNVHash.TryParse("некорректная строка")
if ok then
	print(result)
else
	print("Error");
end

