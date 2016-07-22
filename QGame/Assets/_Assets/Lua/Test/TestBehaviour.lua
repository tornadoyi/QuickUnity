TestBehaviour = class("TestBehaviour", LuaBehaviour)


function TestBehaviour:Start( ... )
	local function CreatePage( ... )
		print("create page")
		AssetManager.LoadGameObjectAsync("Assets/_Assets/Pages/TestPage.prefab"):Finish(function ( t )
			if t.fail then
				print(t.error)
				CreatePage()
				return
			end	

			local canvas = GameObject.Find("Canvas").transform
			local page = Util.Instantiate(t.gameObject, GameObject)
			page.transform:SetParent(canvas, false)
		end)
	end
	CreatePage()
end


TestPage = class("TestPage", LuaBehaviour)

function TestPage:ctor( ... )
	self.loading = false
	self.caches = {}

	-- Config from inspector
	self.panel = nil
end

function TestPage:Show( ... )
	print("Show")
	if self.loading then return end
	self.loading = true

	local st = Vector3(-310, 330, 0)
	local dx = 80
	local dy = 100
	local lc = 8

	local co = nil
	co = self:StartTask(function ( ... )
		local rootPath = "Assets/_Assets/Icons"
		for i=1, 30 do 
			local name = string.format("%s/%s.png", rootPath, i)
			local t = AssetManager.LoadSpriteAsync(name)
			co:Yield( t:WaitForFinish() )
			if t.success then
				local go = GameObject(i)
				go.transform:SetParent(self.panel, false)
				go.transform.localPosition = Vector3(st.x + math.ceil(i % lc) * dx, st.y - math.ceil(i / lc) * dy, 0)
				local image = go:AddComponent(Image)
				image.sprite = t.sprite
				table.insert(self.caches, go)
			else
				print("load " .. name .. " fail, error:" .. t.error)
			end	
		end	
		self.loading = false
	end)
end

function TestPage:Clean( ... )
	print("Clean")
	if self.loading then return end
	for k,v in pairs(self.caches) do
		Object.Destroy(v)
	end	
	self.caches = {}
end

function TestPage:Restart( ... )
	print("Restart")
end