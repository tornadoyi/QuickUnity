
LuaTask = class("LuaTask")

function LuaTask:ctor( mb, func )
	-- body
	self.mb = mb
	self.itask = CustomTask()
	self.co = nil
	self.func = func
	self.name = ""
	self.result = true
	self.errorCode = ""
end

function LuaTask:Start( ... )
	-- Check
	if self.func == nil then GError("Invalid task function") return false end

	-- Async excute
	if not self.itask.ready then GError("Task has ran") return end
	self.itask:Start()
	local parms = {...}
	SafeLuaCoroutine.NextFrame(self.mb, function ()
		if self:IsDone() then return end
		self.co = coroutine.create(function ( ... )
			self.func(...)
			--GInfo("Task %s Done !!!", self.name)
			self.itask:Done()
			if not self.result then GError(self.errorCode) end
		end)
		coroutine.resume(self.co, unpack(parms))
	end)
	
	return true
end

function LuaTask:Stop(  )
	-- body
	if self.itask.done then return end
	--GInfo("Task %s Done", self.name)
	self.itask:Done()
end

function LuaTask:WaitForDone() return  self.itask:WaitForDone() end

function LuaTask:Yield(e)
	-- Check
	local co = coroutine.running()
	if co == nil then GError("Yield can not call in main thread") return end
	if self.co ~= co then GError("Yield is used in another task") return end
	if coroutine.status(self.co) ~= "running" then GError("Invalid coroutine status") return end
	
	-- Wait for callback
	if e ~= nil then 
		SafeLuaCoroutine.StartCoroutine(self.mb, e, function ( ... )
			-- Check task is done because of some reasons
			if self.itask.done then return end

			-- Resume
			--GInfo("Task %s Yield end !!!!", self.name)
			self:Resume()
		end)
	end

	-- yield
	--GInfo("Task %s Yied", self.name)
	coroutine.yield()
end

function LuaTask:Resume()
	-- Check coroutime
	if self.co == nil then GError("Task coroutine is nil") return end

	-- Check coroutine is suspend
	local status = coroutine.status(self.co)
	if status ~= "suspended" then
		GError("Coroutine staus is invalid, expect suspended, current %s", status)
		return
	end	

	-- Check task is done because of some reasons
	if self.itask.done then return end

	-- Resume
	--GInfo("Task %s Resume", self.name)
	coroutine.resume(self.co)
end

function LuaTask:IsDone( ... ) return self.itask.done end

function LuaTask:SetName(name) self.name = name or "" end

function LuaTask:SetResultFailed(format, ...) 
	self.result = false
	self.errorCode = string.format(format, ...)
end	

--[[
	SignalTask
]]

SignalTask = class("SignalTask", LuaTask)

function SignalTask:ctor( mb )
	LuaTask.ctor(self, mb)
end

function SignalTask:Done( ... ) self.itask:Done() end

-- Override 
function SignalTask:Start() GError("Signal has no start") end
function SignalTask:Stop() GError("Signal has no Stop") end
function SignalTask:Yield() GError("Signal has no Yield") end
function SignalTask:Resume() GError("Signal has no Resume") end