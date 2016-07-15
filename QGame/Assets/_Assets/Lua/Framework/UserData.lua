
UserData = class("UserData")

function UserData:ctor()
	self.map = {}
end

function UserData:Get( key, default )
	if key == nil then GError("Invalid key") return default end
	local v = self.map[key]
	return v ~= nil and v or default
end

function UserData:Set( key, value )
	if key == nil then GError("Invalid key") return end
	self.map[key] = value
end

function UserData:DeleteAll() self.map = {} end