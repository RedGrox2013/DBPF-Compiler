DBPFCObject = {
    className = "DBPFCObject"
}
DBPFCObject.__index = DBPFCObject

function DBPFCObject.new(className)
    return setmetatable({className = className or DBPFCObject.className}, DBPFCObject)
end

function DBPFCObject:__tostring()
    return self.className
end

function DBPFCObject:hash(regName)
	return hash(self.className)
end
