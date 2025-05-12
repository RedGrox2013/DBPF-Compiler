PropertyList = class("PropertyList")

function PropertyList:ctor(list)
    if not list then return end

    for k, v in pairs(list) do
        if v.className == "Property" then
            self[k] = v
        end
    end
end

function PropertyList:add(name, property, value)
    if property.className == "Property" then
        self[name] = property
        return
    end

    self[name] = Property.new(property, value)
end

function PropertyList:__tostring()
    local str = ""
    for k, v in pairs(self) do
        if v.className == "Property" then
            str = str .. v.propertyType .. " " ..
                tostring(k) .. " " .. tostring(v) .. "\n"
        end
    end

    return str
end
