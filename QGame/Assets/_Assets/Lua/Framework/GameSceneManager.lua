
GameSceneManager = { 
	sceneList = {}, 
	firstSceneName = nil, 
	parms = nil, 
	sTransiting = false ,
	loadingLanucher = nil
}
GameSceneManager.__index = GameSceneManager

function GameSceneManager.Register(sceneName, sceneController, prefab)
	if  string.empty(sceneName) or 
		sceneController == nil or 
		string.empty(prefab) then

		GError("Invalid arguments")
		return
	end	
	local self = GameSceneManager
	if self.sceneList[sceneName] then GError("Repeated register for %s", sceneName) return end
	self.sceneList[sceneName] = {name = sceneName, controller = sceneController, prefab = prefab}
end

function GameSceneManager.SetFirstSceneName( firstSceneName )
	local self = GameSceneManager
	self.firstSceneName = firstSceneName
end


function GameSceneManager.TransitAsync(sceneName, parms)
	local self = GameSceneManager

	-- Check
	if string.empty(sceneName) then GError("Invalid scene name") return end
	if self.isTransiting then GError("Scene manager is busy") return end

	-- Find lanucher
	local sceneData = self.sceneList[sceneName]
	if  sceneName ~= self.firstSceneName and
		sceneData == nil then
		GError("No lanucher for scene %s", sceneName) 
		return
	end
	
	-- Transit
	self.isTransiting = true
	local co = nil
	co = LuaBehaviour.Global():StartTask(function ()
		-- To first scene
		if sceneName == self.firstSceneName then
			local async = SceneManager.LoadSceneAsync(sceneName)
			co:Yield(async)

		-- To new scene
		else 
			-- Load scene root
			local scenePrefab = nil
			repeat
				local t = AssetManager.LoadGameObjectAsync(sceneData.prefab)
				co:Yield(t:WaitForDone())
				if not t.result then GFatal("Load scene root failed, error:%s", t.error) return end
				scenePrefab = t.gameObject
			until true	

			-- Create new scene
			local lanucher = sceneData.lanucher
			local oldScene = SceneManager.GetActiveScene()
			local newScene = SceneManager.CreateScene(sceneName)
			SceneManager.SetActiveScene(newScene)

			-- Create scene root
			local go = Util.Instantiate(scenePrefab, GameObject)
			go.name = sceneName
			local behaviour = LuaBehaviour.FindLuaBehaviour(go, sceneData.controller)
			
			-- Prepare
			local t_prepare = behaviour:Prepare(parms)
			if t_prepare ~= nil then co:Yield( t_prepare:WaitForDone() ) end

			-- Unload old scene
			SceneManager.UnloadScene(oldScene.name)

			-- Show
			local t_show = behaviour:Show()
			if t_show ~= nil then co:Yield( t_show:WaitForDone() ) end

		end

		self.isTransiting = false

	end)
	
	return co	
	
end




SceneLanucher = class("SceneLanucher", LuaBehaviour)
function SceneLanucher:Prepare(parms) end
function SceneLanucher:Show() end





function SceneLanucher:Show()
	local root = GameObject.Find("Battle").transform
	root:Find("UI/Camera").gameObject:SetActive(true)
	root:Find("UI/EventSystem").gameObject:SetActive(true)
	root:Find("Battlefield/Camera").gameObject:SetActive(true)
end
