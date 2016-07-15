TestPage = class("TestPage", PageController)

function TestPage:OnPrepare( ... )
	self:UseSubHeader("Sub header");
    self:UseBGM("mypage_bgm");
    self:UseBackground("MyPage/PreparationBG");
    --self:HideSubFooter();
    self:UseBackButton()
    self:UpdateNavigationPanels()

    co = GameManager.Instance():StartTask(function( )
    	co:Yield( self:StartPrepareAssetBundleTask():WaitForDone() )
    	self:Prepared()
    	self:Enable()
    end)
end

function TestPage:OnPrepareAssetBundleCore()
	--ResourceService.Instance.WillFetch();
end	


function TestPage:OnClickOK()
    print("On click ok")
end    