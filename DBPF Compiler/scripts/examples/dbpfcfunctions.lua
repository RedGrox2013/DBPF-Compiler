--[[
    
    All functions, classes and enumerations that DBPFC adds
    For input/output see ioexample.lua

    ��� �������, ������ � ������������, ������� ��������� DBPFC
    ����/����� ��. � ioexample.lua

]]

local hexhash = require("./hexhash")

-- ��������� FNV-��� �� ������ 
local h = hash("Hello world!")
print(string.format("%u (%s)", h, type(h)))
-- ���� �����, �� ���������� ������ � ������� 0xXXXXXXXX
local sh = hexhash("Hello world!")
print(string.format("%s (%s)", sh, type(sh)))

-- ����� ����� �� ����
print(hashtoname(0x00000000, "file")) -- animations~
print(hashtoname(0x00000000))         -- GrobEncounter (������ ��� �����)
print(hashtoname(123456789))          -- ���� ��� �� ��������, ����� ��� � ������� 0xXXXXXXXX

-- ������� ��� ������ ������������
local function printenum(enum)
    for k, v in pairs(TypeIDs) do
        print(string.format("\t %s:\t0x%08X", k, v))
    end
end

-- ��� ������������
print("TypeIDs:")
printenum(TypeIDs)
print("GroupIDs:")
printenum(GroupIDs)
