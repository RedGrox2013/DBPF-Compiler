Key = class("Key")

function Key:ctor(instanceId, typeId, groupId)
    self.instanceID = instanceId or 0
    self.typeID     = typeId
    self.groupID    = groupId
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

    if type(key.instanceID) ~= "number" then
        key.instanceID = hash(key.instanceID, "file")
    end
    if key.typeID and type(key.typeID) ~= "number" then
        key.typeID = hash(key.typeID, "type")
    end
    if key.groupID and type(key.groupID) ~= "number" then
        key.groupID = hash(key.groupID, "file")
    end

    return key
end
