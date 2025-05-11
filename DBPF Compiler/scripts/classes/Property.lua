Property = DBPFCObject.new("Property")
Property.__index = Property

function Property.new(propType, value)
	return setmetatable({
		PropertyType = propType,
		Value = value
	}, Property)
end

function Property.newInt32(value)
	return Property.new("int32", value)
end
function Property.newUInt32(value)
	return Property.new("uint32", value)
end
function Property.newFloat(value)
	return Property.new("float", value)
end
function Property.newBool(value)
	return Property.new("bool", value)
end
function Property.newString8(value)
	return Property.new("string8", value)
end
function Property.newString16(value)
	return Property.new("string16", value)
end
function Property.newKey(instanceId, typeId, groupId)
	if instanceId.className == "Key" then
		return Property.new("key", instanceId)
	end

	return Property.new("key", Key.new(instanceId, typeId, groupId))
end

-- доделать!