
-- ====================== DataInfoManager ====================== --
DataInfoManager           = class("DataInfoManager")
DataInfoManager.__index   = DataInfoManager

function DataInfoManager:ctor()
    self._infolist = {}
end	

function DataInfoManager:GetInfoList()
    local i = 0
    for k, v in pairs(self._infolist) do i = i + 1 end
    return self._infolist
end

function DataInfoManager:SaveData(info) end


function DataInfoManager:InsertData(key, data)
    if not key or self._infolist[key] then
        return
    end
    self._infolist[key] = data
end

function DataInfoManager:GetInfo(key)
    if key == nil then return nil end
    return self._infolist[key]
end	 

-- ====================== LCDataInfo ====================== --


DataInfo          = class("DataInfo")
DataInfo.__index  = DataInfo

--function DataInfo:LoadData(row) end


-- ====================== Data loader ====================== --
DataLoader = {}
DataLoader.__index = DataLoader

function DataLoader.LoadString(row, key, def)
    if not row or not key then
        return def
    end

    local lowKey    = string.lower(key)
    local val       = row[lowKey]
    if not val then
        return def
    end
    if string.len(val) == 0 then
        return def
    end
    return val or def
end	

function DataLoader.LoadNumber(row, key, def)
    if not row or not key then
        return def
    end

    local lowKey = string.lower(key)
    local val = row[lowKey]
    if val then
        return tonumber(val)
    end
    return def
end

function DataLoader.LoadBoolean(row, key, def)
    if not row or not key then
        return def
    end

    local lowKey = string.lower(key)
    local val = row[lowKey]
    if val then
        return val == 1
    end
    return def
end

function DataLoader.LoadAndSplit(row, key)
	if not row or not key then
        GError("Invalid arguments")
        return {}
    end
	
	local lowKey = string.lower(key)
    local val = row[lowKey]
	if val == nil then return {} end
	val = string.gsub(val, " ", "", string.len(val))
	return string.split(val, ",|")
end

function DataLoader.LoadArrayAndSplit(row, key)
   if not row or not key then
        GError("Invalid arguments")
        return {}
    end
    local lowKey = string.lower(key)
    local val = row[lowKey]
    if val == nil then return {} end

    -- Clean space
    val = string.gsub(val, " ", "", string.len(val))

    -- Make array by split ( )
    val = "(" .. val .. ")"
    local t = {}
    for match in val:gmatch("%(.-%)") do
        match = string.gsub(match, "[%(%)]", "")
        local vt = string.split(match, ",|")
        table.insert(t, vt)
    end
    return t
end

function DataLoader.LoadNumbers( row, key )
    local rawlist = DataLoader.LoadAndSplit(row, key)
    if #rawlist == 0 then return list end
    local list = {}
    for _, data in pairs(rawlist) do table.insert(list, tonumber(data)) end
    return list
end

function DataLoader.LoadVector2(row, key)
    local t = DataLoader.LoadAndSplit(row, key)
    if #t < 2 then GError("Can not parse Vector2 for %s", key) return Vector2.Zero end
    return Vector2(tonumber(t[1]), tonumber(t[2]))
end

function DataLoader.LoadVector3(row, key)
    local t = DataLoader.LoadAndSplit(row, key)
    if #t < 3 then GError("Can not parse Vector3 for %s", key) return Vector3.Zero end
    return Vector3( tonumber(t[1]), tonumber(t[2]), tonumber(t[3]) )
end

function DataLoader.LoadVector4(row, key)
    local t = DataLoader.LoadAndSplit(row, key)
    if #t < 3 then GError("Can not parse Vector4 for %s", key) return Vector4.Zero end
    return Vector4(tonumber(t[1]), tonumber(t[2]),  tonumber(t[3]), tonumber(t[4]))
end

function DataLoader.LoadArrayFormat(row, key, f )
    local datas = DataLoader.LoadArrayAndSplit(row, key)
    local t = {}
    for _, vt in pairs(datas) do
        local item = f(vt)
        table.insert(t, item)
    end
    return t
end




DataManager = 
{   
	_dataPath	= nil,
    _dataMap    = {},
    _dataList   = {},
	debugOnOff = true,
} 

function DataManager.SetRelativeDataPath(path) DataManager._dataPath = path end

-- no one call this function
function DataManager.GetInfoManagerList()
	local self = DataManager
    local list = {}
    for k, model in pairs(self._dataMap) do
        list[k] = model._mgr
    end
    return list
end	


function DataManager.Register(cmgr, cinfo, ...)
	local self = DataManager
    -- parameter check
	local mgrName = cmgr.__fullname
	local infoName = cinfo.__fullname
    if not mgrName or not cmgr or not cinfo then
        GError("parameters check error")
        if dbtable then
            print("table %s error", dbtable)
        end
        return
    end
    -- multiple guard
    if self._dataMap[mgrName] then
        GError("model " .. mgrName .. " exsist")
        return
    end

    local list          = {...}
    local infoMgr       = cmgr()
    local data          = { _mgr = infoMgr, _cinfo = cinfo, _dbtableList = list }
    self._dataMap[mgrName] = data

    table.insert(self._dataList, data)

	local GetInfoMgr = function() return infoMgr end
	local GetInfo = function(key) return GetInfoMgr():GetInfo(key) end

    self["Get" .. mgrName] = GetInfoMgr
	self["Get" .. infoName] = GetInfo
end	


function DataManager.Load()
	local self = DataManager
	
	local function loadByData(data)
        local list  = data._dbtableList
        local cinfo = data._cinfo
        local mgr   = data._mgr

        for j = 1, #list do
            -- check table exist
            local tname = list[j]
            local tpath = self._dataPath .. "/" .. tname
            if not tpath then
                GError("can not find table %s", tname)
                return
            end
            -- load
            if self.debugOnOff then
                GDebug("DB %s loading ...", tname)
            end

            local rowList   = Util.LoadScript(tpath)
            local count     = table.getn(rowList)

            -- init by rows
            for i = 1, table.getn(rowList) do
                if table.getn(rowList) ~= count then
                    GError("data error, current count %d, sum count %d", table.getn(rowList), count)
                end
                local row   = rowList[i]
                local info  = nil
                if cinfo.CreateInfo then
                    info = cinfo.CreateInfo(tname)
                else
                    info = cinfo(tname)
                end
                info:LoadData(row)
                local saveOK = mgr:SaveData(info)
                if not saveOK then 
                    GError("info load failed " .. tname .. " row : " .. i) 
                end
            end
        end
    end

    for i = 1, #self._dataList do
        local data = self._dataList[i]
        loadByData(data)
    end
end