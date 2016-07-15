
stl = {}

-- =====> map

stl.map = class("map")
local map = stl.map

function map:ctor()
	self._map   = {}
	self._size  = 0
end

function map:size() 
    return self._size 
end

function map:insert(k,v)
	if not k or not v then 
        return 
    end

	local p = self._map[k]
	self._map[k] = v
	if not p then 
        self._size = self._size + 1 
    end
end

function map:erase(k)
	local p = self._map[k]
	if p ~= nil then 
		self._map[k] = nil
        self._size = self._size - 1 
    end
	return p
end

function map:find(k) 
    return self._map[k] 
end

function map:foreach(callback) 
	if not callback then 
        return 
    end
	for k,v in pairs(self._map) do 
		local isBreak = callback(k,v) 
		if isBreak then 
            break 
        end
	end
end


-- =====> list
stl.list = class("list")
local list = stl.list

function list:ctor()
	self._list = {}
end

function list:copy()
	local other = stl.list.new()
	self:foreach(function(k, v) other:push_back(v) end)
end	

function list:size() 
    return table.getn(self._list) 
end

function list:front()
	if table.getn(self._list) == 0 then 
        return 
    end
	return self._list[1]
end

function list:back()
	local s = table.getn(self._list)
	if s == 0 then 
        return 
    end
	return self._list[s]
end

function list:push_back(v)
	if not v then 
        return 
    end
	return table.insert(self._list, v)
end

function list:push_front(v)
	if not v then 
        return 
    end
	return table.insert(self._list, 1, v)
end

function list:pop_back()
	return table.remove(self._list, table.getn(self._list))
end

function list:pop_front()
	return table.remove(self._list, 1)
end

function list:clear()
	self._list = {}
end

function list:insert(index, v)
	return table.insert(self._list, index, v)
end

function list:erase(index)
	return table.remove(self._list, index)
end

function list:swap( index1, index2)
	local x = self._list[index1]
	self._list[index1] = self._list[index2]
	self._list[index2] = x
end

function list:sort(f) table.sort(self._list, f) end

function list:move( src, dst )
	local x = self._list[src]
	local data = table.remove(self._list, src)
	if dst > #self._list then
		table.insert(self._list, data)
	else
		table.insert(self._list, dst, data)
	end
end

function list:foreach(index, callback) 
	local start = index
	local call = callback

	if call == nil then 
		start = 1
		call = index
	end	

	for i=start, #self._list do  
		local v         = self._list[i]
		local isBreak   = call(i, v)
		if isBreak then 
            break 
        end
	end
end

function list:foreach_reverse(index, callback) 
	local start = index
	local call = callback

	if call == nil then 
		start = #self._list
		call = index
	end	
	
	for i=start, 1, -1 do  
		local v         = self._list[i]
		local isBreak   = call(i, v)
		if isBreak then 
            break 
        end
	end
end

-- =====> flatmap
stl.flatmap = class("flatmap")
local flatmap = stl.flatmap

function flatmap:ctor(level)
	self._map   = {}
	self._level = level
	self._size  = 0
end

function flatmap:level() 
    return self._level 
end

function flatmap:size() 
    return self._size 
end

function flatmap:insert(v, ...)	
	local keys = {...}
	if not v or #keys ~= self._level or #keys == 0 then 
        return 
    end
	
	local level = 1
	local function _insert(map)
		local k = keys[level]		
		-- end of level
		if level >= self._level then
			if not map[k] then 
                self._size = self._size + 1 
            end
			map[k] = v
			return
		end		
		-- find next 
		local nmap = map[k]
		if not nmap then 
			nmap = {}
			map[k] = nmap
		end 
		
		level = level + 1
		return _insert(nmap)
	end
	
	_insert(self._map)
end

function flatmap:erase(...)
	local keys = {...}
	if #keys ~= self._level or #keys == 0 then 
        return 
    end
	
	local level = 1
	local function _erase(map)
		local k = keys[level]
		
		-- end of level
		if level >= self._level then
			if map[k] then self._size = self._size - 1 end
			map[k] = nil
			return
		end
		
		-- find next 
		local nmap = map[k]
		if not nmap then 
            return 
        end
		
		level = level + 1
		return _insert(nmap)
	end
	
	return _erase(self._map)
end

function flatmap:find(...)
	local keys = {...}
	if #keys ~= self._level or #keys == 0 then 
        return 
    end
	
	local level = 1
	local function _find(map)
		local k = keys[level]		
		-- end of level
		if level == self._level then 
            return map[k] 
        end
		
		-- find next 
		local nmap = map[k]
		if not nmap then 
            return 
        end
		
		level = level + 1
		return _find(nmap)
	end
	
	return _find(self._map)
end