Key = DBPFCObject.new("Key")
Key.__index = Key

function Key.new(instanceId, typeId, groupId)
    return setmetatable({
        instanceID = instanceId,
        typeID = typeId,
        groupID = groupId
    }, Key)
end

function Key:__tostring()
	local str = nil

    if type(self.instanceID) == "number" then
        str = FNVHash.ToString(self.instanceID)
    else
        str = tostring(self.instanceID)
    end
    if self.typeID then
        str = str .. "."
        if type(self.typeID) == "number" then
            str = str .. FNVHash.ToString(self.typeID)
        else
            str = str .. tostring(self.typeID)
        end
    end
    if self.groupID then
        local id = nil
        if type(self.groupID) == "number" then
            id = FNVHash.ToString(self.groupID)
        else
            id = tostring(self.groupID)
        end
        str = id .. "!" .. str
    end

    return str
end

function Key:hash()
	local key = Key.new(self.instanceID, self.typeID, self.groupID)

    if type(self.instanceID) ~= "number" then
        key.instanceID = hash(self.instanceID, "file")
    end
    if key.typeID and type(key.typeID) ~= "number" then
        key.typeID = hash(key.typeID, "type")
    end
    if key.groupID and type(key.groupID) ~= "number" then
        key.groupID = hash(key.groupID, "file")
    end

    return key
end
