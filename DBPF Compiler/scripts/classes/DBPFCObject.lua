DBPFCObject = {
    ClassName = "DBPFCObject"
}
DBPFCObject.__index = DBPFCObject

function DBPFCObject.new()
    return setmetatable({}, DBPFCObject)
end

function DBPFCObject:__tostring()
    return self.ClassName
end
