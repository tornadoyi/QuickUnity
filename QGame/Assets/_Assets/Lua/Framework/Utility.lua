
Utility = class("Utility", LuaUtility)
Util = Utility

function Utility.LoadScript(path)
	local function load(name)		
		-- load
		local script    = "function openLua() return require " .. "\"" .. path .. "\" end"	
		local func, err = loadstring(script)
		if err then 
            GError(err) 
            return 
        end
		func()
		
		return openLua()
	end	
	
	return load(path)
end


local serial = 1
function Utility.CreateInstanceID()
	if serial > 65535 then serial = 0 end
	serial = serial + 1
	return string.format("%s_%s", math.ceil(Time.realtimeSinceStartup), serial)
end


function Utility.Probability(p)
	if p >=1 then return true end
	if p <= 0 then return false end
	return math.random(1, 100) <= p * 100
end


function Utility.Instantiate(prefab, expectType)
	if slua.isnull(prefab) then GError("Invalid prefab") return end
	local obj = Object.Instantiate(prefab)
	if slua.isnull(obj) then return end
	if expectType ~= nil then obj = slua.as(obj, expectType) end
	return obj
end


function Utility.Invoke(f, ...)
	if f == nil then return end
	local ret, err = pcall(f, ...)
	if ret == true then return end
	print(err)
end


