

AssetLoader = {}



function AssetLoader.InitLoadConfig()

	--[[
		Loading
	]]
	AssetLoader.Loading = {
		Page			= _ID2RES("PAGE_LOADING"),
		Background		= _ID2RES("BATTLE_BG_2_2"),
	}

	--[[
		Battle for loaded
	]]
	AssetLoader.Battle = {
		BGBlock 		= _ID2RES("SCENE_BG_BLOCK"),
		SSViewer		= _ID2RES("SS_VIEWER"),
		TargetSelect	= _ID2RES("EFF_TARGET_SELECT"),
		UITargetSelect	= _ID2RES("EFF_UI_TARGET_SELECT"),
	}

end



function AssetLoader.LoadAssetWithRetry( behaviour, assetName )
	if  behaviour == nil or 
		string.empty(assetName) then 
		GError("Invalid arguments")
		return
	end

	local co = nil
	co = behaviour:StartTask(function ( ... )
		-- Load
		local t = AssetManager.LoadAssetAsync(assetName)
		co:Yield(t:WaitForDone())

		-- Sucess
		if t.result then return end

		-- Fiald to show error message and retry
		--local sigal = SignalTask.new()


	end)

	return co
end

function AssetLoader.UnloadUnusedAssetBundles() return AssetLoader.UnloadUnusedAssets(AssetUnloadLevel.AssetBundles) end

function AssetLoader.UnloadUnusedAssets( level )
	local co = nil
	co = LuaBehaviour.Global():StartTask(function ()
		if level == nil then level = AssetUnloadLevel.Default end
		local t = AssetManager.UnloadUnusedResources(level)
		co:Yield(t:WaitForDone())
		collectgarbage("collect")
	end)
	return co
end