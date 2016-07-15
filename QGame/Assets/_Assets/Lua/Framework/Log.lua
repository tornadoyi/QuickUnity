
local DEF_LOG_LEVEL = {
	DEBUG = 0,
	INFO = 1,
	WARN = 2,
	ERROR = 3,
	FATAL = 4,
}

local CURRENT_LEVEL = DEF_LOG_LEVEL.DEBUG

function SetGLogLevel(level)  
	if not level then return end
	CURRENT_LEVEL = level
end



local function trace(level)
	local info = debug.getinfo(level)
	local path = info.short_src
	local file = nil
	
	local c1 = string.byte("/")
	local c2 = string.byte("\\")
	local i=string.len(path)
	while i > 0 do
		local c = string.byte(path,i)
		if  c == c1 or c == c2 then
			file = string.sub(path, i+1, string.len(path))
			break
		end
		i = i - 1
	end
	if not file then file = path end
	return file, info.currentline, info.name
end	

local function GLog(level, traceLevel, format, ...)
	-- filter low level log
	if level < CURRENT_LEVEL then return end
	
	local function traceInfo(upLevel)
		local src,line,func = trace(5+upLevel)
		if not func then func = "GLOBAL" end
		return src,line,func
	end
	
	-- base error
	local src, line, func = traceInfo(0)
	local content = string.format(format, ...)
	local base = string.format("[%s:%d %s]: %s",src, line, func, content)
	
	-- trace back
	local traceStr = "\nstack traceback:\n"
	for i=1, traceLevel do
		local src, line, func = traceInfo(i)
		local s = string.format("	%s:%d:in function '%s' \n",src, line, func)
		traceStr = traceStr .. s
	end
	
	local log = base
	if traceLevel > 0 then log = log .. traceStr end
	
	if level < DEF_LOG_LEVEL.ERROR then
		Debug.Log(log)
	else
		Debug.LogError(log)
	end
end

function GDebug(...) GLog(DEF_LOG_LEVEL.DEBUG, 0,  ...) end
function GInfo(...) GLog(DEF_LOG_LEVEL.INFO, 0, ...) end
function GWarn(...) GLog(DEF_LOG_LEVEL.WARN, 0, ...) end
function GError(...) GLog(DEF_LOG_LEVEL.ERROR, 0, ...) end
function GFatal(...) GLog(DEF_LOG_LEVEL.FATAL, 0, ...) end
function GErrorTrace(traceLevel, ...) GLog(DEF_LOG_LEVEL.ERROR, traceLevel, ...) end

function GTitle(title) GLog(DEF_LOG_LEVEL.INFO, 0, "=================== %s ===================", title) end