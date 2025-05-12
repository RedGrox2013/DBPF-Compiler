Property = DBPFCObject.new("Property")
Property.__index = Property

function Property.new(propType, value)
	return setmetatable({
		propertyType = string.lower(tostring(propType)),
		value = value
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
		return Property.new(instanceId.className, instanceId)
	end

	return Property.new("key", Key.new(instanceId, typeId, groupId))
end
function Property.newVector2(x, y)
	if x.className == "Vector2" then
		return Property.new(x.className, x)
	end

	return Property.new("vector2", Vector2.new(x, y))
end

-- доделать!

function Property:__tostring()
	if type(self.value) == "number" and self.propertyType == "uint32" and self.value > 100000 then
		return FNVHash.ToString(self.value)
	end
	if self.propertyType == "string8" or self.propertyType == "string16" then
		return '"' .. tostring(self.value) .. '"'
	end
	if string.sub(self.propertyType, -1) == "s" then
		local str = "\n"
		for _, v in ipairs(self.value) do
			local s = tostring(v)
			if self.propertyType == "string8s" or self.propertyType == "string16s" then
				s = '"' .. s .. '"'
			end

			str = str .. "\t" .. s .. "\n"
		end

		return str .. "end"
	end

	return tostring(self.value)
end
