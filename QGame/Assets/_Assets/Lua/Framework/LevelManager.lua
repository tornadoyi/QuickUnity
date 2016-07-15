

LevelManager = {
	curLevelName = Application.loadedLevelName,
	curLevelParm = nil
}

LevelManager.__index = LevelManager

function LevelManager.LoadLevel(name, parm)
	local self = LevelManager
	if not name then GError("invalid arguments") return end
	self.curLevelName = name
	self.curLevelParm = parm
	Application.LoadLevel(name)
end

function LevelManager.GetLevelParm() return LevelManager.curLevelParm end 
