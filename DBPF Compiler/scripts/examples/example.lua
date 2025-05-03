-- функция для конвартации строки в хеш в формате 0x00000000
local hexhash = require("./hexhash")

-- пример функции (возвращает факториал числа)
local function factorial(n)
	if n == 1 then
		return n
	end

	return n * factorial(n - 1)
end

-- print выводит что-нибудь
print("Hello world!")
print(factorial(5))

-- trace работает точно также, как print
trace("Привет мир!")

-- write не делает перенос на новую строку
write("Введи число: ")
-- для ввода используется readline (всегда возвращает строку)
local num = tonumber(readline()) -- для конвертации строки в число используй tonumber
-- num = io.read("n") -- не работает!!!
print(num .. "! = " .. factorial(num))

-- выводить можно ещё так
io.write("Введи своё имя: ")
-- вводить можно ещё так (тоже всегда возвращает строку)
local name = io.read()
print(name .. " -> " .. hash(name) .. " -> " .. hexhash(name))

-- табличка
local cat = {
	name = "Arsik",
	age = 4
}
print(cat.name .. " (" .. cat.age .. ")")

-- массив случайных чисел
local arr = {}
for i = 1, 20 do
	arr[i] = math.random(-1488, 1488)
end
trace "Случайные числа:"
print(table.concat(arr, ", "))
