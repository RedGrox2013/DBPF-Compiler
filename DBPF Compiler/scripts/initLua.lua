-- ��������� Lua ��� DBPFC

local versionmt = {
    __tostring = function(self)
        return self.major .. '.' .. self.minor .. '.' .. self.build .. '.' .. self.revision
    end
}
versionmt.__index = versionmt
DBPFC_VERSION = setmetatable({}, versionmt)

local _trace, _write, _hash = __trace__, __write__, __hash__
__trace__, __write__, __hash__ = nil, nil, nil
function write(v) _write(tostring(v)) end
function hash (v) return _hash (tostring(v)) end
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

-- ��� ������ ������������� � DBPFC ��������� ������� ����� ��������������
print    = trace
io.write = write
io.read  = readline
os.exit  = nil -- �������� ����� ���� �����

-- ����� �� ���� �������� � ��������� ��� �������
readLine   = readline
hashToName = hashtoname
