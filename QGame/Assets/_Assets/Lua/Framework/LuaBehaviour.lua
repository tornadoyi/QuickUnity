

LuaBehaviour = class("LuaBehaviour")

local global = nil
function LuaBehaviour.Global()
	if global ~= nil then return global end
	global = LuaBehaviour.AddToGameObject(QuickManager.gameObject, LuaBehaviour)
	return global
end

function LuaBehaviour:GetLuaBehaviour(class) return LuaBehaviour.FindLuaBehaviour(self.gameObject, class) end
function LuaBehaviour:AddLuaBehaviour(class, parms) return LuaBehaviour.AddToGameObject(self.gameObject, class, parms) end



function LuaBehaviour.FindLuaBehaviour(gameObject, class)
	if not gameObject or not class then GError("Invalid arguments") return end

	local cls = class
	if type(class) == "string" then cls = _G[class] end
	if cls == nil then GError("Invalid class, can not find") return end
	local coms = gameObject:GetComponents(LuaComponent)
	for _, com in pairs(coms) do
		local behaviour = com.luaClassObject
		if behaviour ~= nil then
			-- Check same name
			if behaviour.__fullname == cls.__fullname then return behaviour end
			
			-- Check inherit
			if behaviour.subclass(cls) then return behaviour end
		end
	end
end


-- 1: update 2: LateUpdate 3: FixedUpdate
local t = {
	[0] = LuaComponent,
	[1] = LuaComponent_Update,
	[10] = LuaComponent_LateUpdate,
	[11] = LuaComponent_Update_Late,
	[100] = LuaComponent_FixedUpdate,
	[101] = LuaComponent_Update_Fixed,
	[110] = LuaComponent_Late_Fixed,
	[111] = LuaComponent_Update_Late_Fixed,
}

function LuaBehaviour.AddToGameObject(gameObject, class, parms)
	if gameObject == nil or class == nil then GError("Invalid arguments") return end
	
	local b0 = class.Update and 1 or 0
	local b1 = class.LateUpdate and 1 or 0
	local b2 = class.FixedUpdate and 1 or 0
	local sum = b2 * 100 + b1 * 10 + b0
	
	local comType = t[sum]
	local com = gameObject:AddComponent(comType)

	-- Connect to lua
	com:ConnectLuaClass(class.__fullname)
	local behaviour = com.luaClassObject
	
	-- Set parms
	if parms then
		for k, v in pairs(parms) do
			behaviour[k] = v
		end
	end
	
	-- Awake 
	--behaviour:Awake()
	return behaviour
end


function LuaBehaviour:RegisterEventDelegateWithMap(t)
	if not t then return end
	local f_delegate = function(eventID, parms)
		local f = t[eventID]
		if not f then GError("Why event %s can not be deal", eventID) return end
		f(unpack(parms))
	end
	
	local events = {}
	for id, _ in pairs(t) do table.insert(events, id) end
	self:RegisterEventDelegate(f_delegate, unpack(events))
	
	return f_delegate
end


--[[
	Task Functions
--]]
function LuaBehaviour:StartTask( f, ... ) 
	local task = LuaTask(self, f)
	return task:Start(...) 
end	

function LuaBehaviour:StopTask( task ) 
	if task == nil then GError("Invalid task") return end
	task:Stop()
end	



--[[
-- C# -- Component
function LuaBehaviour:GetComponent( ... ) return self.__com:GetComponent(...) end
function LuaBehaviour:GetComponentInChildren( ... ) return self.__com:GetComponentInChildren(...) end
function LuaBehaviour:GetComponentInParent( ... ) return self.__com:GetComponentInParent(...) end
function LuaBehaviour:GetComponents( ... ) return self.__com:GetComponents(...) end
function LuaBehaviour:GetComponentsInChildren( ... ) return self.__com:GetComponentsInChildren(...) end
function LuaBehaviour:GetComponentsInParent( ... ) return self.__com:GetComponentsInParent(...) end



-- C# -- QuickBehaviour
function LuaBehaviour:Schedule( ... ) return self.__com:Schedule(...) end
function LuaBehaviour:ScheduleOnce( ... ) return self.__com:ScheduleOnce(...) end
function LuaBehaviour:UnSchedule( ... ) return self.__com:UnSchedule(...) end

--]]