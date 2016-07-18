

-- Load all scripts
local loader = require "Assets/_Assets/Lua/Common/ScriptLoader"
loader("Assets/_Assets/Lua")

-- Class define
Config = class("Config")

-- Path define
Config.Path = {
	--Data 			= "Assets/DynamicAssets/Data",
	--Data_Common		= "Assets/DynamicAssets/Data/Common",
	--Protobuf		= "Assets/DynamicAssets/Protobuf",
}

-- Asset Bundle Name Define
Config.AssetBundleName = {
	--Data_Common		= "Data/Common",
	--Story			= "Story",
	--Protobuf		= "Protobuf",
}


