
PageController = class("PageController", GameController) 

-- Virtual
function PageController.GetPrefabName() end


-- Eevents
function PageController:Prepare()
	self.luaPage = self:GetComponent("LuaPage")
	return self:OnPrepare()
end

function PageController:PrepareAssetBundleCore( ... )
	self:OnPrepareAssetBundleCore()
end

function PageController:Teardown()
	self:OnTeardown()
	self:Disable()
end


-- Virtual
function PageController:OnPrepare() end
function PageController:OnPrepareAssetBundleCore() end
function PageController:OnTeardown() end



-- Interface
function PageController:Enable() return self.luaPage:Enable() end
function PageController:Disable() return self.luaPage:Disable() end
function PageController:Prepared() return self.luaPage:Prepared() end

function PageController:UseHeader() return self.luaPage:UseHeader() end
function PageController:HideHeader() return self.luaPage:HideHeader() end

function PageController:UseFooter() return self.luaPage:UseFooter() end
function PageController:HideFooter() return self.luaPage:HideFooter() end

function PageController:UseSubHeader( title ) return self.luaPage:UseSubHeader(title) end
function PageController:HideSubHeader() return self.luaPage:HideSubHeader() end

function PageController:UseSubFooter() return self.luaPage:UseSubFooter() end
function PageController:HideSubFooter() return self.luaPage:HideSubFooter() end

function PageController:UseBackButton() return self.luaPage:UseBackButton() end

function PageController:UseBGM( name ) return self.luaPage:UseBGM(name) end

function PageController:UseBackground( name ) return self.luaPage:UseBackground(name) end

function PageController:UpdateNavigationPanels() return self.luaPage:UpdateNavigationPanels() end

function PageController:StartPrepareAssetBundleTask( ... ) return self.luaPage:StartPrepareAssetBundleTask() end