local function hexhash(s)
	return string.format("0x%08X", hash(s))
end

return hexhash
