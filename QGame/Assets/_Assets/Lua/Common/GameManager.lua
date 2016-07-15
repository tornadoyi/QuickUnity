GameManager = class("GameManager", LuaBehaviour)

local instance = nil
function GameManager.Instance()
	if instance == nil then
		local node = GameObject.Find("QuickUnity")
		if node == nil then
			GError("Can not find QuickUnity")
			return
		end	
		LuaBehaviour.AddToGameObject(node, GameManager)
	end
	return instance	
end

function GameManager:Awake( ... )
	instance = self
end