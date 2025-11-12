-- создание списка свойств
local propList = Prop.new({
	-- добавляем свойства
	Prop.newProperty(PropertyType.int32, "testInt32Property", 666)
})

-- можно ещё так добавить
propList:add(PropertyType.string16, "testString16Property", "Hello world!")

-- конвертируем таблицу в DBPF_Compiler.FileTypes.Prop.PropertyList
local result = propList:toPropList()

-- теперь у result доступны методы класса PropertyList
print(result:SerializeToJson()) -- выведет список свойств в json

-- обязательно возвращаем конвертированный список свойств!
return result