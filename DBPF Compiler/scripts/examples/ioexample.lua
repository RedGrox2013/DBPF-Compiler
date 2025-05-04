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
print(name)
