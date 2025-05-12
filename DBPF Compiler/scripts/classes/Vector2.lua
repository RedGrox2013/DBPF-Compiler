Vector2 = DBPFCObject.new("Vector2")
Vector2.__index = Vector2

function Vector2.new(x, y)
    local ok = false
    ok, x = pcall(tonumber, x)
    if not ok then x = 0 end
    ok, y = pcall(tonumber, y)
    if not ok then y = 0 end

    return setmetatable({
        x = x or 0,
        y = y or 0
    }, Vector2)
end

function Vector2:__tostring()
    return "(" .. tostring(self.x) .. ", " .. tostring(self.y) .. ")"
end
