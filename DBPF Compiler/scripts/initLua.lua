-- Настройка Lua для DBPFC

local _trace, _write, _hash = __trace__, __write__, __hash__
__trace__, __write__, __hash__ = nil, nil, nil

local _tostring = tostring;
tostring = function(v)
    if type(v) == "userdata" and v.ToString then
        return v:ToString()
    end

    return _tostring(v)
end

function write(v) _write(tostring(v)) end

function trace(...)
	local args = table.pack(...)
	if args.n == 1 then
		_trace(tostring(args[1]))
		return
	end

	for i = 1, args.n do
		write(args[i])
		_write('\t')
	end
	_write('\n')
end

function hash(v, regName)
	if not v then
		return 0
	end
	if type(v) == "table" and v.hash then
		return v:hash(regName)
	end
	if type(v) == "number" then
		return v >= 0 and v or 0xFFFFFFFF + v + 1
	end

	return _hash(tostring(v), regName)
end


-- для лучшей совместимости с DBPFC некоторые функции будут переопределены
print    = trace
io.write = write
io.read  = readline
os.exit  = nil -- добавить потом норм метод

-- чтобы не было путаницы с регистром имён функций
readLine   = readline
hashToName = hashtoname
