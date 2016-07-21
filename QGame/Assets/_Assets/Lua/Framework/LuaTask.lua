
LuaTask = class("LuaTask", QuickUnity.LuaTask)

function LuaTask:ctor( mb, func)
	self.mb = mb
	self.co = nil
	self.func = func
	self.name = ""
	print("here", mb, func)
end	

function LuaTask:Start( ... )
	-- Check
	if self.func == nil then GError("Can not start task, none task function") return self end

	local parms = { ... }
	self.__base:Start( function() self:OnStart(parms) end)
	return self
end	


function LuaTask:OnStart( parms )
	SafeLuaCoroutine.NextFrame(self.mb, function ()
		if self.finish then return end

		-- create coroutine
		self.co = coroutine.create(function ( ... )
			self.func(...)
			self.__base:SetFinish()
			if self.fail then GError(self.error) end
		end)

		-- run coroutine
		coroutine.resume(self.co, unpack(parms))
	end)
end


function LuaTask:Yield(e)
	-- Check
	local co = coroutine.running()
	if co == nil then GError("Task can not yield, task has not start") return end
	if self.co ~= co then GError("Yield is used in another task") return end
	if coroutine.status(self.co) ~= "running" then GError("Invalid coroutine status") return end
	
	-- Wait for callback
	if e ~= nil then 
		SafeLuaCoroutine.StartCoroutine(self.mb, e, function ( ... )
			-- Check task is finish because of some reasons
			if self.finish then return end

			-- Resume
			self:Resume()
		end)
	end

	-- Lua yield
	coroutine.yield()
end

function LuaTask:Resume()
	-- Check coroutime
	if self.co == nil then GError("Task can not resume, task has not start") return end

	-- Check coroutine is suspend
	local status = coroutine.status(self.co)
	if status ~= "suspended" then
		GError("Coroutine staus is invalid, expect suspended, current %s", status)
		return
	end	

	-- Check task is done because of some reasons
	if self.finish then return end

	-- Resume lua coroutine
	coroutine.resume(self.co)
end



