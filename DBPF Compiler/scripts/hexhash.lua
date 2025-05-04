--- Returns a hash in the format "0xXXXXXXXX"
-- @param s string Input string
-- @param[opt="all"] reg string Registry of names to search ("all"|"fnv"|"file"|"property"|"type"|"simulator")
-- @return string Hash in string format "0xXXXXXXXX"
-- @usage
--	hexhash("Hello world!")		  --> "0x8A01B99C"
--	hexhash("Что-то на русском")  --> "0xBC8DBE07"
--	hexhash("animations~")		  --> "0x00000000"
--	hexhash("animations~", "fnv") --> "0x04C89ED2"
local function hexhash(s, reg)
	return string.format("0x%08X", hash(s, reg))
end

return hexhash
