Prop = {}
Prop.__index = Prop

function Prop.new(...)
    local args = table.pack(...)
    if args.n == 1 and type(args[1]) == "table" then
        return setmetatable(args[1], Prop)
    end

    args.n = nil
    return setmetatable(args, Prop)
end

function Prop.add(p, propertyType, propertyName, value)
    local property = Prop.newProperty(propertyType, propertyName, value)
    if typeof(p) == "PropertyList" then
        p:Add(property)
    else
        p[propertyName] = property
    end
end

function Prop.newProperty(propType, propName, value)
    local p = Property(propName);
    p.PropertyType = propType
    p.Value = value
    -- TODO: Arrays

    return p
end