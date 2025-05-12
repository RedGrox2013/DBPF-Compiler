Vector2 = class("Vector2")

function Vector2:ctor(x, y)
    local ok = false
    ok, x = pcall(tonumber, x)
    if not ok then x = 0 end
    ok, y = pcall(tonumber, y)
    if not ok then y = 0 end

    self.x = x or 0
    self.y = y or 0
end

function Vector2:__tostring()
    return "(" .. tostring(self.x) .. ", " .. tostring(self.y) .. ")"
end
