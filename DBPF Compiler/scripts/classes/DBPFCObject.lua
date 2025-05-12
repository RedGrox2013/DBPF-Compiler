DBPFCObject = {
	className = "DBPFCObject"
}
DBPFCObject.__index = DBPFCObject

-- function DBPFCObject.new(className)
-- 	return setmetatable({className = className or DBPFCObject.className}, DBPFCObject)
-- end

function class(className, child, parent)
	child = setmetatable(child or {}, parent or DBPFCObject)
	child.__index = child
	child.className = className or DBPFCObject.className

	function child.new(...)
		local obj = setmetatable({}, child)
		if obj.ctor then
			obj:ctor(...)
		end

		return obj
	end

	return child
end

function DBPFCObject:__tostring()
	return self.className
end

function DBPFCObject:hash(regName)
	return hash(tostring(self), regName)
end
