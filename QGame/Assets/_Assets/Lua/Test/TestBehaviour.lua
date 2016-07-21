TestBehaviour = class("TestBehaviour", LuaBehaviour)

function TestBehaviour:Start( ... )
	
	local t = nil
	t = self:StartTask(function ( ... )
		print("enter task 1")
		t:Yield(WaitForSeconds(3.0))
		t:SetFail("only test fail")
	end)


	local co = nil
	co = self:StartTask(function ( ... )
			
		print("enter task 2")
		co:Yield(WaitForSeconds(1.0))
		
		print("after 1 seconds, wait task 1 ...")

		co:Yield( t:WaitForFinish() )

		print("task 1 finish, result ", t.error)

		co:Yield(WaitForSeconds(2.0))

		print("task 2 finish")

	end)

end