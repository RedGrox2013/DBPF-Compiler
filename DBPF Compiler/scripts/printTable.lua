-- Функция для просмотра всех кдючей и значений таблицы
local function printTable(t)
    for k, v in pairs(t) do
        print(k .. ": " .. tostring(v))
    end
end

return printTable
