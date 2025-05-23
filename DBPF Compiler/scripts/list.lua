local list = {}
list.__index = list

function list.new(type)
    return newGeneric("System.Collections.Generic.List`1", {type})
end

function list.totable(list)
    if typeof(list) ~= "List`1" then
        return nil
    end
    
    local t = {}
    for i = 0, list.Count - 1 do
        t[i + 1] = list[i]
    end

    return t
end

function list.fromtable(table, listType)
    if type(table) ~= "table" then
        return nil
    end

    local l = list.new(listType or "System.Object")
    for _, v in ipairs(table) do
        l:Add(v)
    end

    return l
end

return list