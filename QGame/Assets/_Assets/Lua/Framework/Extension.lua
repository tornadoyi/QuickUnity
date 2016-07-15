

-- ================================= String ================================= --
function string.split(str, delimiter)
	if str==nil or str=='' or delimiter==nil then
		return nil
	end
	
    local result = {}
    local endTag = string.sub(delimiter, 1, 1)
    local tag = string.format("[%s]", delimiter)
    for match in (str..endTag):gmatch("(.-)"..tag) do
        table.insert(result, match)
    end
    return result
end

function string.empty(str)
	if str == nil then return true end
	return string.len(str) <= 0
end


-- ================================= Table ================================= --
function table.keys(t, value)
	if t == nil then return {} end
	local keys = {}
	for k, v in pairs(t) do
		if v == value then table.insert(keys, k) end
	end	
	return keys
end

function table.key(t, value)
	if t == nil then return end
	local key = nil
	for k, v in pairs(t) do
		if v == value then
			key = k
			break
		end	
	end	
	return key
end


-- ================================= Math ================================= --
function math.clamp(value, min, max)
	if value < min then return min end
	if value > max then return max end
	return value
end

function math.clamp01(value) return math.clamp(value, 0 , 1) end


function math.lerp(from, to, t)
	t = math.clamp01( t )
	return from*t + to*(1-t)
end

