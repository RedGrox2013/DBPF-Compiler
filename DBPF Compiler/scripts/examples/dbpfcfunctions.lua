--[[
    
    All functions, classes and enumerations that DBPFC adds
    For input/output see ioexample.lua

    Все функции, классы и перечисления, которые добавляет DBPFC
    Ввод/вывод см. в ioexample.lua

]]

local hexhash = require("./hexhash")

-- вычисляет FNV-хеш из строки 
local h = hash("Hello world!")
print(string.format("%u (%s)", h, type(h)))
-- тоже самое, но возвращает строку в формате 0xXXXXXXXX
local sh = hexhash("Hello world!")
print(string.format("%s (%s)", sh, type(sh)))

-- Поиск имени по хешу
print(hashtoname(0x00000000, "file")) -- animations~
print(hashtoname(0x00000000))         -- GrobEncounter (первое что нашёл)
print(hashtoname(123456789))          -- если хеш не известен, вернёт его в формате 0xXXXXXXXX

-- функция для вывода перечислений
local function printenum(enum)
    for k, v in pairs(enum) do
        print(string.format("\t %s:\t0x%08X", k, v))
    end
end

-- Все перечисления
print("TypeIDs:")
printenum(TypeIDs)
print("GroupIDs:")
printenum(GroupIDs)

-- Получить путь к директории программы:
print(getProgramDirectory())
