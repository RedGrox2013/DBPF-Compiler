--[[

    Examples of working with hashes

    To calculate a hash from a string, DBPFC has a built-in function hash.
    It takes a string and returns a 32-bit integer (the calculated hash)
    The second argument can be a registry of names if we want to get a hash of an alias (aliases end in ~)

    To get a hash in the format "0xXXXXXXXX" we can use the function hexhash
    It takes the same arguments as hash, but returns a string in the format "0xXXXXXXXX"

    --------------------------------------------------------------------------------------------------------------------

    Примеры работы с хешами

    Для вычисления хеша из строки в DBPFC есть встроенная функция hash.
    Она принимает строку и возвращает 32-битное целое число (вычисленный хеш)
    Вторым аргументом можно указать реестр имён, если мы хотим получить хеш псевдонима (псевдонимы заканчиваются на ~)

    Для получения хеша в формате "0xXXXXXXXX" можно использовать функцию hexhash
    Она принимает те же аргументы, что и hash, но возвращает строку в формате "0xXXXXXXXX"

    Мне лень всё описывать...

]]

local hexhash = require("./hexhash")

--[[ For example, let's create a function that will calculate a hash and immediately print it
     Для примера создадим функцию, которая будет вычислять хеш и сразу выводить его]]
local function printhash(s, reg)
    print(s .. " ----[" .. (reg or "all") .. "]----> " ..
        hash(s, reg) .. " -> " .. hexhash(s, reg))
end

print("Пример без красивого вывода: " .. hash("Hello world!") .. " -> " .. hexhash("Hello world!")) -- 0x8A01B99C

printhash("Hello world!")		-- Hello world! ----[all]----> 2315368860 -> 0x8A01B99C
printhash("Что-то на русском")	-- Что-то на русском ----[all]----> 3163405831 -> 0xBC8DBE07

printhash("animations~")		-- animations~ ----[all]----> 0 -> 0x00000000
printhash("animations~", "fnv")	-- animations~ ----[fnv]----> 80256722 -> 0x04C89ED2

--[[ Getting name from hash
     Получение имени из хеша]]
print(hashtoname(0x00000000, "file")) -- animations~

-- Также можно использовать некоторые перечисления
print(hashtoname(TypeIDs.prop) .. " -> " .. TypeIDs.prop)
print(hashtoname(GroupIDs.PlannerThumbnails) .. " -> " .. GroupIDs.PlannerThumbnails)

-- Все значения TypeIDs и GroupIDs:
local function printenum(enum)
    for k, v in pairs(TypeIDs) do
        print(string.format("\t %s:\t0x%08X", k, v))
    end
end

print("TypeIDs:")
printenum(TypeIDs)
print("GroupIDs:")
printenum(GroupIDs)
