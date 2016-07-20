TestBehaviour = class("TestBehaviour", LuaBehaviour)

function TestBehaviour:Start( ... )
	local x,y = self.__com:GetComponent("Transform")
	print(Transform)
	local s1 = slua.tostring(x)
	print(s1, s2)
	local t = slua.as(x, Transform)
	print(t)
end