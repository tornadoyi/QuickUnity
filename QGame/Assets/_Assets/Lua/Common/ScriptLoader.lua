

local function LoadScripts(path)
	local root = path .. "/"
	
	local function _Root(file) return root .. file end
	local function _Framework(file) return _Root("Framework/" .. file) end	
	local function _ThirdParty(file) return _Root("ThirdParty/" .. file) end	
	local function _Network(file) return _Root("Network/" .. file) end
	local function _Common(file) return _Root("Common/" .. file) end
	local function _Scene(file) return _Root("Scene/" .. file) end
	local function _Test(file) return _Root("Test/" .. file) end	
	
	local function _DataInfo(file) return _Root("DataInfo/" .. file) end
	local function _GameData(file) return _Root("GameData/" .. file) end
	local function _Data(file) return _Root("Data/" .. file) end
	local function _GameLogic(file) return _Root("GameLogic/" .. file) end	
	local function _Input(file) return _Root("Input/" .. file) end		
	
	local scripts = {
	
		-- Framework
		_Framework("Class"),
		_Framework("Log"),
		_Framework("STL"),
		_Framework("UserData"),
		_Framework("Serial"),
		_Framework("Callbacker"),
		_Framework("Utility"),
		_Framework("Extension"),
		_Framework("LuaBehaviour"),
		_Framework("DataManager"),
		_Framework("LuaTask"),
		_Framework("GameSceneManager"),

		-- Common
		_Common("Debug"),
		_Common("GameDefine"),
		_Common("Formula"),
		_Common("EventDefine"),
		_Common("AssetLoader"),
		_Common("GameManager"),

		-- Game Logic
		_GameLogic("Common/GameController"),
		_GameLogic("Common/PageController"),

		-- Pages
		_GameLogic("Pages/TestPage"),

		_Test("TestBehaviour"),
		
	}
	
	local function load(path)		
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
	
	for i=1, #scripts do load(scripts[i]) end

end	

return LoadScripts