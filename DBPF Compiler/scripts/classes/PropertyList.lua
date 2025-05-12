PropertyList = DBPFCObject.new("PropertyList")
PropertyList.__index = PropertyList

function PropertyList.new(list)
    return setmetatable(list or {}, PropertyList)
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
