

Callbacker = class("Callbacker.lua")

function Callbacker:ctor()
	self._list = {}
end

function Callbacker:Add(f)
	if not f then GError("Invalid arguments") return end
	local id = Util.CreateInstanceID()
	self._list[id] = f
	return id
end

function Callbacker:Del(id)
	if not id then GError("Invalid arguments") return end
	self._list[id] = nil
end

function Callbacker:Invoke(...)
	for id, f in pairs(self._list) do
		f(...)
	end
end
