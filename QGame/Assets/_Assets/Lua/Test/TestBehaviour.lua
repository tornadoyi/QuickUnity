TestBehaviour = class("TestBehaviour", LuaBehaviour)

function TestBehaviour:Start( ... )
	local x = self:GetComponent("Transform")
	
	local go = GameObject()
	local com = go:AddComponent(QuickBehaviour)

	MyMove = class("MyMove", Action.MoveTo)

	local seq = Action.Sequence()
	local mv = MyMove(1.0, {1, 2, 3})
	print(mv)
	seq:Add(mv)
	com.action:RunAction(seq)

	local t = CustomTask()
	--t.xxx= 1
	print(Vector3.up)
	Vector3.up = Vector3(2,2,2)
	print(Vector3.up)

	
end